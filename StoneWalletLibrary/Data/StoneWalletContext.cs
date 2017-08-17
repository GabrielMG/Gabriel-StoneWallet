using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StoneWalletLibrary.Models;


namespace StoneWalletLibrary.Data
{
    class StoneWalletContext : DbContext
    {
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardHolder> CardHolders { get; set; }
    }
}
