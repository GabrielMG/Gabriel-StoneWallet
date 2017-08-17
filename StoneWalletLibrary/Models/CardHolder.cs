using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class CardHolder
    {
        public int CardHolderId { get; set; }
        public string Name { get; set; }
        public string NationalIdNumber { get; set; }
        public virtual IEnumerable<Card> Cards { get; set; }
        public virtual IEnumerable<Wallet> Wallet { get; set; }
        public bool Deleted { get; set; }
    }
}
