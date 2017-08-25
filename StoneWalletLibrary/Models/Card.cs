using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Card
    {
        public Card()
        {

        }

        public Card(string number, string cVV, int dueDate, DateTime expirationDate, decimal limit, decimal credit, Wallet wallet, Cardholder cardholder)
        {
            Number = number;
            CVV = cVV;
            DueDate = dueDate;
            ExpirationDate = expirationDate;
            Limit = limit;
            Credit = credit;
            Wallet = wallet;
            Cardholder = cardholder;
        }

        public int CardId { get; set; }
        public string Number { get; set; }
        public string CVV { get; set; }
        public int DueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Limit { get; set; }
        public decimal Credit { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual Cardholder Cardholder { get; set; }
        public bool Deleted { get; set; }
    }
}