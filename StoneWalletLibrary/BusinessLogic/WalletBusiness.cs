using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    class WalletBusiness
    {
        private readonly WalletRepository _WalletRepository;
        private readonly CardRepository _CardRepository;
        private readonly CardholderRepository _CardholderRepository;

        public WalletBusiness(WalletRepository walletRepository, CardRepository cardRepository, CardholderRepository cardholderRepository)
        {
            _WalletRepository = walletRepository;
            _CardRepository = cardRepository;
            _CardholderRepository = cardholderRepository;
        }

        public Card CreateCard(string number, string cvv, int dueDate, DateTime expirationDate, decimal limit, decimal credit, Cardholder cardholder)
        {
            var card = new Card();
            card.Number = number;
            card.CVV = cvv;
            card.DueDate = dueDate;
            card.ExpirationDate = expirationDate;
            card.Limit = limit;
            card.Credit = credit;
            card.Cardholder = cardholder;
            card.Deleted = false;
            return CreateCard(card);
        }

        public Card CreateCard(Card card)
        {
            try
            {
                if (card.Cardholder == null || card.Deleted)
                {
                    return null;
                }
                if (card.Limit < card.Credit)
                {
                    card.Credit = card.Limit;
                }
                return _CardRepository.Add(card);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Wallet CreateWallet(decimal userLimit, decimal maximumLimit, decimal credit, Cardholder cardholder, List<Card> cards)
        {
            var wallet = new Wallet();
            wallet.UserLimit = userLimit;
            wallet.MaximumLimit = maximumLimit;
            wallet.Credit = credit;
            wallet.Cardholder = cardholder;
            wallet.Cards = cards;
            wallet.Deleted = false;
            return CreateWallet(wallet);
        }

        public Wallet CreateWallet(Wallet wallet)
        {
            try
            {
                if (wallet.Cardholder == null || wallet.Deleted)
                {
                    return null;
                }
                if (wallet.Cards != null)
                {
                    var tempWallet = _WalletRepository.Add(wallet);
                    setWalletMaximumLimit(tempWallet);
                    setWalletAvailableCredit(tempWallet);
                    return tempWallet;
                }
                else
                {
                    return _WalletRepository.Add(wallet);
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public Cardholder CreateCardholder(string name, string nationalIdNumber, List<Card> cards)
        {
            var cardholder = new Cardholder();
            cardholder.Name = name;
            cardholder.NationalIdNumber = nationalIdNumber;
            cardholder.Cards = cards;
            cardholder.Deleted = false;
            return CreateCardholder(cardholder);
        }

        public Cardholder CreateCardholder(Cardholder cardholder)
        {
            try
            {
                return _CardholderRepository.Add(cardholder);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public Wallet GetWallet(Cardholder cardholder)
        {
            return GetWallet(cardholder.CardholderId);
        }

        public Wallet GetWallet(int cardholderId)
        {
            return _WalletRepository.FindByCardholder(cardholderId);
        }

        public Card GetCard(string number)
        {
            return _CardRepository.Find(c => c.Number == number && c.Deleted == false).FirstOrDefault();
        }

        public List<Card> SortCardsByPriority(Wallet wallet)
        {
            var now = DateTime.Now; //verificar como fazer pro now não pegar hora minuto segundo
            List<Card> thisMonth = wallet.Cards.Where(c => c.DueDate >= now.Day && c.ExpirationDate > now).OrderByDescending(c => c.DueDate).ThenByDescending(c => c.Credit).ToList();
            List<Card> nextMonth = wallet.Cards.Where(c => c.DueDate < now.Day && c.ExpirationDate > now).OrderByDescending(c => c.DueDate).ThenByDescending(c => c.Credit).ToList();
            return thisMonth.Concat(nextMonth).ToList();
        }

        private bool PurchaseLoop(decimal value, List<Card> cardsByPriority)
        {
            try
            {
                foreach (var card in cardsByPriority)
                {
                    if (value <= card.Credit)
                    {
                        card.Credit = card.Credit - value;
                        _CardRepository.Edit(card);
                        setWalletAvailableCredit(card.Wallet);
                        return true;
                    }
                    else
                    {
                        value = value - card.Credit;
                        card.Credit = 0;
                        _CardRepository.Edit(card);
                        setWalletAvailableCredit(card.Wallet);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExecutePurchase(decimal value, Wallet wallet)
        {
            if (value <= wallet.UserLimit && value <= wallet.Credit)
            {
                return PurchaseLoop(value, SortCardsByPriority(wallet));
            }
            else
            {
                return false;
            }
        }

        public bool AddCardToWallet(Card card, Wallet wallet)
        {
            try
            {
                wallet.Cards.Add(card);
                _WalletRepository.Edit(wallet);
                card.Wallet = wallet;
                _CardRepository.Edit(card);
                setWalletMaximumLimit(wallet);
                setWalletAvailableCredit(wallet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveCardFromWallet(Card card)
        {
            try
            {
                if (card.Wallet != null)
                {
                    if (card.Wallet.Cards.Contains(card))
                    {
                        var wallet = card.Wallet;
                        card.Wallet.Cards.Remove(card);
                        _WalletRepository.Edit(card.Wallet);
                        card.Wallet = null;
                        _CardRepository.Edit(card);
                        setWalletMaximumLimit(wallet);
                        setWalletAvailableCredit(wallet);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool setWalletMaximumLimit(Wallet wallet)
        {
            try
            {
                List<Card> cards = _CardRepository.FindByWallet(wallet.WalletId);
                wallet.MaximumLimit = cards.Sum(c => c.Limit);
                if (wallet.UserLimit > wallet.MaximumLimit)
                {
                    wallet.UserLimit = wallet.MaximumLimit;
                }
                _WalletRepository.Edit(wallet);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool setWalletAvailableCredit(Wallet wallet)
        {
            try
            {
                List<Card> cards = _CardRepository.FindByWallet(wallet.WalletId);
                wallet.Credit = cards.Sum(c => c.Credit);
                if (wallet.UserLimit > wallet.MaximumLimit)
                {
                    wallet.UserLimit = wallet.MaximumLimit;
                }
                _WalletRepository.Edit(wallet);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool SetWalletUserDefinedLimit(Wallet wallet, decimal limit)
        {
            if (wallet.MaximumLimit > limit)
            {
                wallet.UserLimit = limit;
                _WalletRepository.Edit(wallet);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteCard(Card card)
        {
            try
            {
                RemoveCardFromWallet(card);
                _CardRepository.Delete(card);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteWallet(Wallet wallet)
        {
            try
            {
                foreach (Card card in wallet.Cards)
                {
                    RemoveCardFromWallet(card);
                }
                _WalletRepository.Delete(wallet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteCardholder(Cardholder cardholder)
        {
            try
            {
                DeleteWallet(cardholder.Wallet);
                foreach (Card card in cardholder.Cards)
                {
                    DeleteCard(card);
                }
                _CardholderRepository.Delete(cardholder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
