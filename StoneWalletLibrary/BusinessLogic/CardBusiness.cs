using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    public class CardBusiness : ICardBusiness
    {
        private readonly ICardRepository _CardRepository;
        private readonly IWalletBusiness _WalletBusiness;

        public CardBusiness(ICardRepository cardRepository, IWalletBusiness walletBusiness)
        {
            _CardRepository = cardRepository;
            _WalletBusiness = walletBusiness;
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

        public Card GetCard(string number)
        {
            try
            {
                return _CardRepository.Find(c => c.Number == number && c.Deleted == false).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Card GetCardById(int cardId)
        {
            try
            {
                return _CardRepository.FindById(cardId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Card> GetCardsByCardholder(int cardholderId)
        {
            try
            {
                return _CardRepository.Find(c => c.Cardholder.CardholderId == cardholderId && c.Deleted == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Card PayCredit(int cardId, decimal value)
        {
            var card = GetCardById(cardId);
            if (card == null)
            {
                return null;
            }
            card.Credit = card.Credit + value;
            if (card.Credit > card.Limit)
            {
                //only here because I can't limit how much money is sent through "value"
                //somente aqui porque eu não posso limitar quanto dinheiro está sendo enviado através do "value"
                card.Credit = card.Limit;
            }
            try
            {
                return _CardRepository.Edit(card);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool VerifyCardOwnership(int cardId, int cardholderId)
        {
            try
            {
                var card = _CardRepository.FindById(cardId);
                if (card != null)
                {
                    return (card.Cardholder.CardholderId == cardholderId);
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteCard(int cardId)
        {
            return DeleteCard(GetCardById(cardId));
        }

        public bool DeleteCard(Card card)
        {
            try
            {
                _WalletBusiness.RemoveCardFromWallet(card);
                _CardRepository.Delete(card);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
