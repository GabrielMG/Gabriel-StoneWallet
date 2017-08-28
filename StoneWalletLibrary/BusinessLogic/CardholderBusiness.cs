using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.Models;

namespace StoneWalletLibrary.BusinessLogic
{
    public class CardholderBusiness : ICardholderBusiness
    {
        private readonly ICardholderRepository _CardholderRepository;
        private readonly ICardBusiness _CardBusiness;
        private readonly IWalletBusiness _WalletBusiness;

        public CardholderBusiness(ICardholderRepository cardholderRepository, ICardBusiness cardBusiness, IWalletBusiness walletBusiness)
        {
            _CardholderRepository = cardholderRepository;
            _CardBusiness = cardBusiness;
            _WalletBusiness = walletBusiness;
        }

        public Cardholder CreateCardholder(string name, string nationalIdNumber, string email, List<Card> cards)
        {
            var cardholder = new Cardholder
            {
                Name = name,
                NationalIdNumber = nationalIdNumber,
                Cards = cards,
                Deleted = false,
                Email = email
            };
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
            try
            {
                return _CardholderRepository.Find(ch => ch.Name.Contains(name.Trim()) && ch.Deleted == false).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Cardholder GetCardholderByNationalIdNumber(string nationalIdNumber)
        {
            try
            {
                var cardholder = _CardholderRepository.Find(ch => ch.NationalIdNumber == nationalIdNumber.Trim() && ch.Deleted == false).FirstOrDefault();
                if (cardholder != null && cardholder?.Cards != null)
                {
                    cardholder.Cards = cardholder.Cards.Where(c => c.Deleted == false).ToList();
                    if (cardholder.Wallet != null && cardholder?.Wallet?.Cards != null)
                    {
                        cardholder.Wallet.Cards = cardholder.Wallet.Cards.Where(c => c.Deleted == false).ToList();
                    }
                }

                return cardholder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Cardholder GetCardholderByEmail(string email)
        {
            try
            {
                var cardholder = _CardholderRepository.Find(ch => ch.Email.Contains(email.Trim()) && ch.Deleted == false).FirstOrDefault();
                if (cardholder != null && cardholder?.Cards != null)
                {
                    cardholder.Cards = cardholder.Cards.Where(c => c.Deleted == false).ToList();
                    if (cardholder.Wallet != null && cardholder?.Wallet?.Cards != null)
                    {
                        cardholder.Wallet.Cards = cardholder.Wallet.Cards.Where(c => c.Deleted == false).ToList();
                    }
                }
                return cardholder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Cardholder GetCardholderById(int cardholderId)
        {
            try
            {
                var cardholder = _CardholderRepository.FindById(cardholderId);
                if (cardholder != null && cardholder?.Cards != null)
                {
                    cardholder.Cards = cardholder.Cards.Where(c => c.Deleted == false).ToList();
                    if (cardholder.Wallet != null && cardholder?.Wallet?.Cards != null)
                    {
                        cardholder.Wallet.Cards = cardholder.Wallet.Cards.Where(c => c.Deleted == false).ToList();
                    }
                }
                return cardholder;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Cardholder EditCardholder(Cardholder cardholder)
        {
            try
            {
                return _CardholderRepository.Edit(cardholder);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteCardHolder(int cardholderId)
        {
            return DeleteCardholder(GetCardholderById(cardholderId));
        }

        public bool DeleteCardholder(Cardholder cardholder)
        {
            try
            {
                if (cardholder == null)
                {
                    return false;
                }
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
