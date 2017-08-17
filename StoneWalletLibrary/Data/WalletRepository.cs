using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;
using System.Data.Entity;

namespace StoneWalletLibrary.Data
{
    class WalletRepository
    {
        private readonly StoneWalletContext _context;

        public WalletRepository(StoneWalletContext context)
        {
            _context = context;
        }

        public Wallet Add(Wallet Wallet)
        {
            var entry = _context.Wallets.Add(Wallet);
            _context.SaveChanges();
            return entry;
        }

        public Wallet FindById(int WalletId)
        {
            return _context.Wallets.Include(w => w.CardHolder).Include(w => w.Cards).Where(w => w.WalletId.Equals(WalletId)).FirstOrDefault();
        }

        public List<Wallet> Find(Func<Wallet, bool> filter)
        {
            return _context.Wallets.Include(w => w.CardHolder).Include(w => w.Cards).Where(filter).ToList();
        }

        public List<Wallet> FindByCardHolder(int CardHolderId)
        {
            return _context.Wallets.Include(w => w.CardHolder).Include(w => w.Cards).Where(w => w.CardHolder.CardHolderId == CardHolderId).ToList();
        }
                
        public Wallet Edit(Wallet Wallet)
        {
            _context.Wallets.Attach(Wallet);
            var entry = _context.Entry(Wallet);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(Wallet Wallet)
        {
            Wallet.Deleted = true;
            _context.Wallets.Attach(Wallet);
            var entry = _context.Entry(Wallet);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
