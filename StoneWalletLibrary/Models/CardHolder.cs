using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Cardholder
    {
        public int CardholderId { get; set; }
        public string Name { get; set; }
        public string NationalIdNumber { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Wallet Wallet { get; set; }
        public bool Deleted { get; set; }
    }
}
