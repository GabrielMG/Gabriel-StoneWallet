using System;
using StoneWalletLibrary.Models;
using System.Collections.Generic;

namespace StoneWalletLibrary.BusinessLogic
{
    public interface ICardBusiness
    {
        Card CreateCard(Card card);
        Card CreateCard(string number, string cvv, int dueDate, DateTime expirationDate, decimal limit, decimal credit, Cardholder cardholder);
        Card PayCredit(int cardId, decimal value);
        bool VerifyCardOwnership(int cardId, int cardholderId);
        bool DeleteCard(int cardId);
        bool DeleteCard(Card card);
        Card GetCard(string number);
        Card GetCardById(int cardId);
        List<Card> GetCardsByCardholder(int cardholderId);
    }
}