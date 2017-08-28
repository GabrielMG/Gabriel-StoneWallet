using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletService.ViewModels
{
    public class Cardholder
    {
        public Cardholder()
        {

        }

        public Cardholder(int cardholderId, string name, string nationalIdNumber, string email, List<Card> cards, Wallet wallet)
        {
            CardholderId = cardholderId;
            Name = name;
            NationalIdNumber = nationalIdNumber;
            Email = email;
            Cards = cards;
            Wallet = wallet;
        }

        public int CardholderId { get; set; }
        public string Name { get; set; }
        public string NationalIdNumber { get; set; }
        public string Email { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Wallet Wallet { get; set; }

        public StoneWalletLibrary.Models.Cardholder ToModel()
        {
            var cards = new List<StoneWalletLibrary.Models.Card>();
            if (Cards != null)
            {
                foreach (var card in Cards)
                {
                    cards.Add(card.ToModel());
                }
            }
            if (Wallet == null)
            {
                return new StoneWalletLibrary.Models.Cardholder(Name, NationalIdNumber, Email, cards, null);
            }
            return new StoneWalletLibrary.Models.Cardholder(Name, NationalIdNumber, Email, cards, Wallet.ToModel());
        }

        //public static Cardholder ToViewModel(StoneWalletLibrary.Models.Cardholder cardholder)
        //{
        //    if (cardholder == null)
        //    {
        //        return null;
        //    }
        //    var cards = new List<Card>();
        //    if (cardholder.Cards != null)
        //    {
        //        foreach (var card in cardholder.Cards)
        //        {
        //            cards.Add(Card.ToViewModel(card));
        //        }
        //    }
        //    if (cardholder.Wallet == null)
        //    {
        //        return new Cardholder(cardholder.CardholderId, cardholder.Name, cardholder.NationalIdNumber, cardholder.Email, cards, null);
        //    }

        //    return new Cardholder(cardholder.CardholderId, cardholder.Name, cardholder.NationalIdNumber, cardholder.Email, cards, Wallet.ToViewModel(cardholder.Wallet));
        //}
    }
}
