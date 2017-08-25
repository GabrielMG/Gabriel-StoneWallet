using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletService.ViewModels
{
    public class Card
    {
        public int CardId { get; set; }
        public string Number { get; set; }
        public string CVV { get; set; }
        public int DueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Limit { get; set; }
        public decimal Credit { get; set; }
        public virtual Wallet Wallet { get; set; }
        public virtual Cardholder Cardholder { get; set; }

        public StoneWalletLibrary.Models.Card ToModel()
        {
            return new StoneWalletLibrary.Models.Card(Number, CVV, DueDate, ExpirationDate, Limit, Credit, Wallet.ToModel(), Cardholder.ToModel());
        }
    }
}