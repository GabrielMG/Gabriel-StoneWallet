using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;
using System.Data.Entity;

namespace StoneWalletLibrary.Data
{
    class CardholderRepository
    {
        private readonly StoneWalletContext _context;

        public CardholderRepository(StoneWalletContext context)
        {
            _context = context;
        }

        public Cardholder Add(Cardholder cardholder)
        {
            var entry = _context.Cardholders.Add(cardholder);
            _context.SaveChanges();
            return entry;
        }

        public Cardholder FindById(int cardholderId)
        {
            return _context.Cardholders.Include(ch => ch.Wallet).Include(ch => ch.Cards).Where(ch => ch.CardholderId.Equals(cardholderId) && ch.Deleted == false).FirstOrDefault();
        }

        public List<Cardholder> Find(Func<Cardholder, bool> filter)
        {
            return _context.Cardholders.Include(ch => ch.Wallet).Include(ch => ch.Cards).Where(filter).ToList();
        }

        public Cardholder Edit(Cardholder cardholder)
        {
            _context.Cardholders.Attach(cardholder);
            var entry = _context.Entry(cardholder);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(Cardholder cardholder)
        {
            cardholder.Deleted = true;
            _context.Cardholders.Attach(cardholder);
            var entry = _context.Entry(cardholder);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
