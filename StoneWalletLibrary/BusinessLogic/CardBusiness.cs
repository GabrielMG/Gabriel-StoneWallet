using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    class CardBusiness
    {
        private readonly CardRepository _CardRepository;
        private readonly WalletBusiness _WalletBusiness;

        public CardBusiness(CardRepository cardRepository, WalletBusiness walletBusiness)
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
            return _CardRepository.Find(c => c.Number == number && c.Deleted == false).FirstOrDefault();
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
