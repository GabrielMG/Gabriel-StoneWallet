using System;
using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.Data
{
    public interface ICardholderRepository
    {
        Cardholder Add(Cardholder cardholder);
        void Delete(Cardholder cardholder);
        Cardholder Edit(Cardholder cardholder);
        List<Cardholder> Find(Func<Cardholder, bool> filter);
        Cardholder FindById(int cardholderId);
    }
}