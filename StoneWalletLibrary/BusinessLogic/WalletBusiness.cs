using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;
using System.Data.Entity;
using StoneWalletLibrary.BusinessLogic.Interfaces;

namespace StoneWalletLibrary.BusinessLogic
{
    public class WalletBusiness : IWalletBusiness
    {
        private readonly IWalletRepository _WalletRepository;
        private readonly ICardRepository _CardRepository;
        private readonly IClock _Clock;

        public WalletBusiness(IWalletRepository walletRepository, ICardRepository cardRepository, IClock clock)
        {
            _WalletRepository = walletRepository;
            _CardRepository = cardRepository;
            _Clock = clock;
        }


        public Wallet CreateWallet(decimal userLimit, decimal maximumLimit, decimal credit, Cardholder cardholder, List<Card> cards)
        {
            var wallet = new Wallet
            {
                UserLimit = userLimit,
                MaximumLimit = maximumLimit,
                Credit = credit,
                Cardholder = cardholder,
                Cards = cards,
                Deleted = false
            };
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

        public Wallet GetWallet(Cardholder cardholder)
        {
            try
            {
                return GetWallet(cardholder.CardholderId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Wallet GetWallet(int cardholderId)
        {
            try
            {
                var wallet = _WalletRepository.FindByCardholder(cardholderId);
                if (wallet != null && wallet?.Cards != null)
                {
                    wallet.Cards = wallet.Cards.Where(c => c.Deleted == false).ToList();
                }
                return wallet;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<Card> SortCardsByPriority(Wallet wallet)
        {
            var now = _Clock.Now;

            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

            List<Card> cardsDueEndOfTheMonth = wallet.Cards.Where(c => c.DueDate >= daysInMonth && c.ExpirationDate > now)
                    .OrderBy(c => c.Credit).ToList();
            List<Card> cardsDueThisMonth = wallet.Cards.Where(c => c.DueDate >= now.Day && c.DueDate < daysInMonth && c.ExpirationDate > now)
                    .OrderByDescending(c => c.DueDate).ThenBy(c => c.Credit).ToList();
            List<Card> cardsDueNextMonth = wallet.Cards.Where(c => c.DueDate < now.Day && c.ExpirationDate > now)
                    .OrderByDescending(c => c.DueDate).ThenBy(c => c.Credit).ToList();

            return cardsDueNextMonth.Concat(cardsDueEndOfTheMonth).Concat(cardsDueThisMonth).ToList();
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

        public Wallet ExecutePurchase(decimal value, Wallet wallet)
        {
            if (wallet == null)
            {
                return null;
            }
            if (value <= wallet.UserLimit && value <= wallet.Credit)
            {
                if (PurchaseLoop(value, SortCardsByPriority(wallet)))
                {
                    return wallet;
                }
                return null;
            }
            else
            {
                return null;
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

        public bool DeleteWallet(int cardholderId)
        {
            return DeleteWallet(GetWallet(cardholderId));
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
    }
}
