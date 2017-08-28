using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletService.Tests.MockRepositories
{
    class MockCardholderRepository : ICardholderRepository
    {
        public Cardholder Add(Cardholder cardholder)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cardholder cardholder)
        {
            throw new NotImplementedException();
        }

        public Cardholder Edit(Cardholder cardholder)
        {
            throw new NotImplementedException();
        }

        public List<Cardholder> Find(Func<Cardholder, bool> filter)
        {
            var cardholders = new List<Cardholder>();
            cardholders.Add(CardholderDummy.InitializeCardholderDummy());
            return cardholders;
        }

        public Cardholder FindById(int cardholderId)
        {
            throw new NotImplementedException();
        }
    }
}
