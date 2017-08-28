using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletService.Tests.MockRepositories
{
    class MockWalletRepository : IWalletRepository
    {
        public Wallet Add(Wallet wallet)
        {
            throw new NotImplementedException();
        }

        public void Delete(Wallet wallet)
        {
            throw new NotImplementedException();
        }

        public Wallet Edit(Wallet wallet)
        {
            return wallet;
        }

        public List<Wallet> Find(Func<Wallet, bool> filter)
        {
            throw new NotImplementedException();
        }

        public Wallet FindByCardholder(int cardholderId)
        {
            var cardholder = CardholderDummy.InitializeCardholderDummy();
            return cardholder.Wallet;
        }

        public Wallet FindById(int walletId)
        {
            throw new NotImplementedException();
        }
    }
}
