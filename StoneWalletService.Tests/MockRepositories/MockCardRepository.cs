using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletService.Tests.MockRepositories
{
    class MockCardRepository : ICardRepository
    {
        public Card Add(Card card)
        {
            throw new NotImplementedException();
        }

        public void Delete(Card card)
        {
            throw new NotImplementedException();
        }

        public Card Edit(Card card)
        {
            return card;
        }

        public List<Card> Find(Func<Card, bool> filter)
        {
            throw new NotImplementedException();
        }

        public List<Card> FindByCardholder(int cardholderId)
        {
            throw new NotImplementedException();
        }

        public Card FindById(int cardId)
        {
            throw new NotImplementedException();
        }

        public List<Card> FindByWallet(int walletId)
        {
            throw new NotImplementedException();
        }
    }
}
