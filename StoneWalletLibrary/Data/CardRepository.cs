using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoneWalletLibrary.Models;
using System.Data.Entity;

namespace StoneWalletLibrary.Data
{
    class CardRepository
    {
        private readonly StoneWalletContext _context;

        public CardRepository(StoneWalletContext context)
        {
            _context = context;
        }

        public Card Add(Card Card)
        {
            var entry = _context.Cards.Add(Card);
            _context.SaveChanges();
            return entry;
        }

        public Card FindById(int CardId)
        {
            return _context.Cards.Include(c => c.CardHolder).Include(c => c.Wallet).Where(c => c.CardId.Equals(CardId)).FirstOrDefault();
        }

        public List<Card> Find(Func<Card, bool> filter)
        {
            return _context.Cards.Include(c => c.CardHolder).Include(c => c.Wallet).Where(filter).ToList();
        }

        public List<Card> FindByCardHolder(int CardHolderId)
        {
            return _context.Cards.Include(c => c.CardHolder).Include(c => c.Wallet).Where(c => c.CardHolder.CardHolderId == CardHolderId).ToList();
        }

        public List<Card> FindByWallet(int WalletId)
        {
            return _context.Cards.Include(c => c.CardHolder).Include(c => c.Wallet).Where(c => c.Wallet.WalletId == WalletId).ToList();
        }

        public Card Edit(Card Card)
        {
            _context.Cards.Attach(Card);
            var entry = _context.Entry(Card);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(Card Card)
        {
            Card.Deleted = true;
            _context.Cards.Attach(Card);
            var entry = _context.Entry(Card);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}
