using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    class CardholderBusiness
    {
        private readonly CardholderRepository _CardholderRepository;
        private readonly CardBusiness _CardBusiness;
        private readonly WalletBusiness _WalletBusiness;

        public CardholderBusiness(CardholderRepository cardholderRepository, CardBusiness cardBusiness, WalletBusiness walletBusiness)
        {
            _CardholderRepository = cardholderRepository;
            _CardBusiness = cardBusiness;
            _WalletBusiness = walletBusiness;
        }

        public Cardholder CreateCardholder(string name, string nationalIdNumber, List<Card> cards)
        {
            var cardholder = new Cardholder();
            cardholder.Name = name;
            cardholder.NationalIdNumber = nationalIdNumber;
            cardholder.Cards = cards;
            cardholder.Deleted = false;
            return CreateCardholder(cardholder);
        }

        public Cardholder CreateCardholder(Cardholder cardholder)
        {
            try
            {
                return _CardholderRepository.Add(cardholder);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Cardholder> GetCardholderByName(string name)
        {
            return _CardholderRepository.Find(ch => ch.Name.Contains(name.Trim()) && ch.Deleted == false).ToList();
        }

        public Cardholder GetCardholderByNationalIdNumber(string nationalIdNumber)
        {
            return _CardholderRepository.Find(ch => ch.NationalIdNumber.Contains(nationalIdNumber.Trim()) && ch.Deleted == false).FirstOrDefault();
        }

        public Cardholder GetCardholderById(int cardholderId)
        {
            return _CardholderRepository.FindById(cardholderId);
        }

        public bool DeleteCardholder(Cardholder cardholder)
        {
            try
            {
                _WalletBusiness.DeleteWallet(cardholder.Wallet);
                foreach (Card card in cardholder.Cards)
                {
                    _CardBusiness.DeleteCard(card);
                }
                _CardholderRepository.Delete(cardholder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
