using System;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    public interface ICardBusiness
    {
        Card CreateCard(Card card);
        Card CreateCard(string number, string cvv, int dueDate, DateTime expirationDate, decimal limit, decimal credit, Cardholder cardholder);
        bool DeleteCard(Card card);
        Card GetCard(string number);
    }
}