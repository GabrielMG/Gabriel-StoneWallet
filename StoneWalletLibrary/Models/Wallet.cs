using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Wallet
    {
        public Wallet()
        {

        }

        public Wallet(decimal userLimit, decimal maximumLimit, decimal credit, List<Card> cards, Cardholder cardholder)
        {
            UserLimit = userLimit;
            MaximumLimit = maximumLimit;
            Credit = credit;
            Cards = cards;
            Cardholder = cardholder;
        }

        public int WalletId { get; set; }
        public decimal UserLimit { get; set; }
        public decimal MaximumLimit { get; set; }
        public decimal Credit { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Cardholder Cardholder { get; set; }
        public bool Deleted { get; set; }
    }
}
