using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletService.ViewModels
{
    public class Cardholder
    {
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
    }
}
