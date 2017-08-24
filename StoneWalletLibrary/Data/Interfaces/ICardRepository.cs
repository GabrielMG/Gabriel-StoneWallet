using System;
using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.Data
{
    interface ICardRepository
    {
        Card Add(Card card);
        void Delete(Card card);
        Card Edit(Card card);
        List<Card> Find(Func<Card, bool> filter);
        List<Card> FindByCardholder(int cardholderId);
        Card FindById(int cardId);
        List<Card> FindByWallet(int walletId);
    }
}