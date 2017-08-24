using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    interface ICardholderBusiness
    {
        Cardholder CreateCardholder(Cardholder cardholder);
        Cardholder CreateCardholder(string name, string nationalIdNumber, List<Card> cards);
        bool DeleteCardholder(Cardholder cardholder);
        Cardholder GetCardholderById(int cardholderId);
        List<Cardholder> GetCardholderByName(string name);
        Cardholder GetCardholderByNationalIdNumber(string nationalIdNumber);
    }
}