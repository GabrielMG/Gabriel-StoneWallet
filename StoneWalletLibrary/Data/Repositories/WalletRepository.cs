using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;
using System.Data.Entity;

namespace StoneWalletLibrary.Data
{
    public class WalletRepository : IWalletRepository
    {
        private readonly IStoneWalletContext _context;

        public WalletRepository(IStoneWalletContext context)
        {
            _context = context;
        }

        public Wallet Add(Wallet wallet)
        {
            var entry = _context.Wallets.Add(wallet);
            _context.SaveChanges();
            return entry;
        }

        public Wallet FindById(int walletId)
        {
            return _context.Wallets.Include(w => w.Cardholder).Include(w => w.Cards).Where(w => w.WalletId.Equals(walletId) && w.Deleted == false).FirstOrDefault();
        }

        public List<Wallet> Find(Func<Wallet, bool> filter)
        {
            return _context.Wallets.Include(w => w.Cardholder).Include(w => w.Cards).Where(filter).ToList();
        }

        public Wallet FindByCardholder(int cardholderId)
        {
            return _context.Wallets.Include(w => w.Cardholder).Include(w => w.Cards).Where(w => w.Cardholder.CardholderId == cardholderId && w.Deleted == false).FirstOrDefault();
        }
                
        public Wallet Edit(Wallet wallet)
        {
            _context.Wallets.Attach(wallet);
            var result = _context.MarkAsModified(wallet);
            _context.SaveChanges();
            return result;
        }

        public void Delete(Wallet wallet)
        {
            wallet.Deleted = true;
            _context.Wallets.Attach(wallet);
            _context.MarkAsModified(wallet);
            _context.SaveChanges();
        }
    }
}
