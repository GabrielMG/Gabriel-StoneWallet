using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletLibrary.Models
{
    public class Cardholder
    {
        public Cardholder()
        {

        }

        public Cardholder(string name, string nationalIdNumber, string email, List<Card> cards, Wallet wallet)
        {
            Name = name;
            NationalIdNumber = nationalIdNumber;
            Cards = cards;
            Email = email;
            Wallet = wallet;
            Deleted = false;
        }

        public int CardholderId { get; set; }
        public string Name { get; set; }
        public string NationalIdNumber { get; set; }
        public string Email { get; set; }
        public virtual List<Card> Cards { get; set; }
        public virtual Wallet Wallet { get; set; }
        public bool Deleted { get; set; }
    }
}
