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
        public Wallet()
        {

        }

        public Wallet(int walletId, decimal userLimit, decimal maximumLimit, decimal credit, List<Card> cards, Cardholder cardholder)
        {
            WalletId = walletId;
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

        //public static Wallet ToViewModel(StoneWalletLibrary.Models.Wallet wallet)
        //{
        //    if (wallet == null)
        //    {
        //        return null;
        //    }
        //    var cards = new List<Card>();
        //    if (wallet.Cards != null)
        //    {
        //        foreach (var card in wallet.Cards)
        //        {
        //            cards.Add(Card.ToViewModel(card));
        //        }
        //    }
        //    return new Wallet(wallet.WalletId, wallet.UserLimit, wallet.MaximumLimit, wallet.Credit, cards, Cardholder.ToViewModel(wallet.Cardholder));
        //}
    }
}
