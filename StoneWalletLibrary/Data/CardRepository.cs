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

        public Card Add(Card card)
        {
            var entry = _context.Cards.Add(card);
            _context.SaveChanges();
            return entry;
        }

        public Card FindById(int cardId)
        {
            return _context.Cards.Include(c => c.Cardholder).Include(c => c.Wallet).Where(c => c.CardId.Equals(cardId)).FirstOrDefault();
        }

        public List<Card> Find(Func<Card, bool> filter)
        {
            return _context.Cards.Include(c => c.Cardholder).Include(c => c.Wallet).Where(filter).ToList();
        }

        public List<Card> FindByCardholder(int cardholderId)
        {
            return _context.Cards.Include(c => c.Cardholder).Include(c => c.Wallet).Where(c => c.Cardholder.CardholderId == cardholderId && c.Deleted == false).ToList();
        }

        public List<Card> FindByWallet(int walletId)
        {
            return _context.Cards.Include(c => c.Cardholder).Include(c => c.Wallet).Where(c => c.Wallet.WalletId == walletId && c.Deleted == false).ToList();
        }

        public Card Edit(Card card)
        {
            _context.Cards.Attach(card);
            var entry = _context.Entry(card);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
            return entry.Entity;
        }

        public void Delete(Card card)
        {
            card.Deleted = true;
            _context.Cards.Attach(card);
            var entry = _context.Entry(card);
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}
