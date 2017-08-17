using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Card
    {
        public int CardId { get; set; }
        public string Number { get; set; }
        public string CVV { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Limit { get; set; }
        public decimal Credit { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual CardHolder CardHolder { get; set; }
        public bool Deleted { get; set; }
    }
}