using System;
using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.Data
{
    public interface IWalletRepository
    {
        Wallet Add(Wallet wallet);
        void Delete(Wallet wallet);
        Wallet Edit(Wallet wallet);
        List<Wallet> Find(Func<Wallet, bool> filter);
        Wallet FindByCardholder(int cardholderId);
        Wallet FindById(int walletId);
    }
}