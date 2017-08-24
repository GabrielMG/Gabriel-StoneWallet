using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public decimal UserLimit { get; set; }
        public decimal MaximumLimit { get; set; }
        public decimal Credit { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Cardholder Cardholder { get; set; }
        public bool Deleted { get; set; }
    }
}
