using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;

namespace StoneWalletService.ViewModels
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public decimal UserLimit { get; set; }
        public decimal MaximumLimit { get; set; }
        public decimal Credit { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Cardholder Cardholder { get; set; }

        public StoneWalletLibrary.Models.Wallet ToModel()
        {
            var cards = new List<StoneWalletLibrary.Models.Card>();
            if (Cards != null)
            {
                foreach (var card in Cards)
                {
                    cards.Add(card.ToModel());
                }
            }
            return new StoneWalletLibrary.Models.Wallet(UserLimit, MaximumLimit, Credit, cards, Cardholder.ToModel());
        }
    }
}
