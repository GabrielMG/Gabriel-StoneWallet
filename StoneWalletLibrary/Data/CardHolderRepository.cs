using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;
using System.Data.Entity;

namespace StoneWalletLibrary.Data
{
    class CardHolderRepository
    {
        private readonly StoneWalletContext _context;

        public CardHolderRepository(StoneWalletContext context)
        {
            _context = context;
        }

        public CardHolder Add(CardHolder CardHolder)
        {
            var entry = _context.CardHolders.Add(CardHolder);
            _context.SaveChanges();
            return entry;
        }

        public CardHolder FindById(int CardHolderId)
        {
            return _context.CardHolders.Include(ch => ch.Wallet).Include(ch => ch.Cards).Where(ch => ch.CardHolderId.Equals(CardHolderId)).FirstOrDefault();
        }

        public List<CardHolder> Find(Func<CardHolder, bool> filter)
        {
            return _context.CardHolders.Include(ch => ch.Wallet).Include(ch => ch.Cards).Where(filter).ToList();
        }

        public CardHolder Edit(CardHolder CardHolder)
        {
            _context.CardHolders.Attach(CardHolder);
            var entry = _context.Entry(CardHolder);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(CardHolder CardHolder)
        {
            CardHolder.Deleted = true;
            _context.CardHolders.Attach(CardHolder);
            var entry = _context.Entry(CardHolder);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
