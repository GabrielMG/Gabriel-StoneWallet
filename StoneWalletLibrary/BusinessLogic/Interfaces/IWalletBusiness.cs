using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    public interface IWalletBusiness
    {
        bool AddCardToWallet(Card card, Wallet wallet);
        Wallet CreateWallet(decimal userLimit, decimal maximumLimit, decimal credit, Cardholder cardholder, List<Card> cards);
        Wallet CreateWallet(Wallet wallet);
        bool DeleteWallet(Wallet wallet);
        bool ExecutePurchase(decimal value, Wallet wallet);
        Wallet GetWallet(Cardholder cardholder);
        Wallet GetWallet(int cardholderId);
        bool RemoveCardFromWallet(Card card);
        bool setWalletAvailableCredit(Wallet wallet);
        bool setWalletMaximumLimit(Wallet wallet);
        bool SetWalletUserDefinedLimit(Wallet wallet, decimal limit);
        List<Card> SortCardsByPriority(Wallet wallet);
    }
}