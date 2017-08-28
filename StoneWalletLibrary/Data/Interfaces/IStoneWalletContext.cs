using System.Data.Entity;
using StoneWalletLibrary.Models;
using System;

namespace StoneWalletLibrary.Data
{
    public interface IStoneWalletContext : IDisposable
    {
        DbSet<Cardholder> Cardholders { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<Wallet> Wallets { get; set; }
        int SaveChanges();
        Wallet MarkAsModified(Wallet item);
        Card MarkAsModified(Card item);
        Cardholder MarkAsModified(Cardholder item);
    }
}