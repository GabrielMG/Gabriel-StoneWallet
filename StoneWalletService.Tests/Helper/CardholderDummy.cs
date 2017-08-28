using StoneWalletLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoneWalletService.Tests
{
    public class CardholderDummy
    {
        public static Cardholder InitializeCardholderDummy()
        {
            var cardholder = new Cardholder("TestCardholder", "123456789", "user@test.com", null, null);
            var wallet = new Wallet(10000.00M, 60000.00M, 46000.00M, null, cardholder);
            var cards = new List<Card>();
            var card1 = new Card("123456", "123", 15, new DateTime(2050, 05, 01), 5000.00M, 5000.00M, wallet, cardholder);
            var card2 = new Card("234567", "234", 5, new DateTime(2050, 05, 01), 15000.00M, 10000.00M, wallet, cardholder);
            var card3 = new Card("156478", "456", 31, new DateTime(2050, 05, 01), 20000.00M, 20000.00M, wallet, cardholder);
            var card4 = new Card("654123", "789", 7, new DateTime(2050, 05, 01), 10000.00M, 1000.00M, wallet, cardholder);
            var card5 = new Card("523987", "951", 20, new DateTime(2050, 05, 01), 5000.00M, 5000.00M, wallet, cardholder);
            var card6 = new Card("789456", "753", 1, new DateTime(2050, 05, 01), 5000.00M, 5000.00M, wallet, cardholder);
            cards.Add(card1);
            cards.Add(card2);
            cards.Add(card3);
            cards.Add(card4);
            cards.Add(card5);
            cards.Add(card6);
            cardholder.Cards = cards;
            wallet.Cards = cards;
            cardholder.Wallet = wallet;
            return cardholder;
        }
    }
}
