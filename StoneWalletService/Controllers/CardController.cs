using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using StoneWalletLibrary.Data;
using StoneWalletLibrary.BusinessLogic;
using StoneWalletService.ViewModels;
using System.Threading.Tasks;

namespace StoneWalletService.Controllers
{
    [Authorize]
    public class CardController : BaseController
    {
        private IWalletBusiness _WalletBusiness;
        private ICardBusiness _CardBusiness;
        private ICardholderBusiness _CardholderBusiness;

        public CardController()
        {
            var walletRepository = new WalletRepository(context);
            var cardRepository = new CardRepository(context);
            var cardholderRepository = new CardholderRepository(context);
            _WalletBusiness = new WalletBusiness(walletRepository, cardRepository);
            _CardBusiness = new CardBusiness(cardRepository, _WalletBusiness);
            _CardholderBusiness = new CardholderBusiness(cardholderRepository, _CardBusiness, _WalletBusiness);
        }

        [HttpGet]
        public IHttpActionResult GetCardById(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _CardBusiness.GetCardById(cardId);
            if (result != null)
            {
                if (_CardBusiness.VerifyCardOwnership(result.CardId, cardholder.CardholderId))
                {
                    return Ok(Card.ToViewModel(result));
                }
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any card with this cardId associated with this user account.");
        }

        [HttpGet]
        public IHttpActionResult GetCardByNumber(string number)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _CardBusiness.GetCard(number);
            if (result != null)
            {
                if (_CardBusiness.VerifyCardOwnership(result.CardId, cardholder.CardholderId))
                {
                    return Ok(Card.ToViewModel(result));
                }
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any card with this number associated with this user account.");
        }

        [HttpGet]
        public IHttpActionResult GetUserCards()
        {
            List<StoneWalletLibrary.Models.Card> result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            if (cardholder != null)
            {
                result = _CardBusiness.GetCardsByCardholder(cardholder.CardholderId);
            }
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any cards associated with this user account.");
            }
            var cards = new List<Card>();
            foreach (var card in result)
            {
                cards.Add(Card.ToViewModel(card));
            }
            return Ok(cards);
        }

        [HttpPost]
        public IHttpActionResult CreateCard([FromBody]Card card)
        {
            StoneWalletLibrary.Models.Card result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            if (card.Cardholder == null)
            {
                card.Cardholder = Cardholder.ToViewModel(cardholder);
                result = _CardBusiness.CreateCard(card.ToModel());
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "The cardholder property should be null to create a new card. The cardholder associated with the current user will be set to this card automatically");
            }

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Card not created.");
            }
            return Ok(Card.ToViewModel(result));
        }

        [HttpPost]
        public IHttpActionResult PayCredit(int cardId, decimal value)
        {
            StoneWalletLibrary.Models.Card result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);

            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            if (_CardBusiness.VerifyCardOwnership(cardId, cardholder.CardholderId))
            {
                result = _CardBusiness.PayCredit(cardId, value);
            }
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to pay credit.");
            }
            return Ok(Card.ToViewModel(result));
        }

        [HttpDelete]
        public IHttpActionResult DeleteCard(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var card = _CardBusiness.GetCardById(cardId);
            if (_CardBusiness.VerifyCardOwnership(card.CardId, cardholder.CardholderId))
            {
                return Ok(_CardBusiness.DeleteCard(cardId));
            }
            return Content(HttpStatusCode.BadRequest, "Error: unable to delete card");
        }
    }
}
