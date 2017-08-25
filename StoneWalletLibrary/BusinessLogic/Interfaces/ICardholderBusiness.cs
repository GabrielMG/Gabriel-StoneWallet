using System.Collections.Generic;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    public interface ICardholderBusiness
    {
        Cardholder CreateCardholder(Cardholder cardholder);
        Cardholder CreateCardholder(string name, string nationalIdNumber, string email, List<Card> cards);
        Cardholder EditCardholderName(int cardholderId, string name);
        Cardholder EditCardholderNationalIdNumber(int cardholderId, string nationalIdNumber);
        bool DeleteCardHolder(int cardholderId);
        bool DeleteCardholder(Cardholder cardholder);
        Cardholder GetCardholderById(int cardholderId);
        List<Cardholder> GetCardholderByName(string name);
        Cardholder GetCardholderByEmail(string email);
        Cardholder GetCardholderByNationalIdNumber(string nationalIdNumber);
    }
}