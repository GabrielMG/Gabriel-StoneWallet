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
using StoneWalletLibrary.BusinessLogic.Interfaces;

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
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public CardController(IStoneWalletContext context) : base(context)
        {
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public CardController(bool testUserFlag, IWalletRepository walletRepository, ICardRepository cardRepository, ICardholderRepository cardholderRepository, IClock clock)
        {
            Configure(walletRepository, cardRepository, cardholderRepository, clock);
            TestUserFlag = testUserFlag;
        }

        private void Configure(IWalletRepository walletRepository, ICardRepository cardRepository, ICardholderRepository cardholderRepository, IClock clock)
        {
            _WalletBusiness = new WalletBusiness(walletRepository, cardRepository, clock);
            _CardBusiness = new CardBusiness(cardRepository, _WalletBusiness);
            _CardholderBusiness = new CardholderBusiness(cardholderRepository, _CardBusiness, _WalletBusiness);
        }

        [HttpGet]
        public IHttpActionResult GetCardById(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _CardBusiness.GetCardById(cardId);
            if (result != null)
            {
                if (_CardBusiness.VerifyCardOwnership(result.CardId, cardholder.CardholderId))
                {
                    return Ok(result);
                }
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any card with this cardId associated with this user account.");
        }

        [HttpGet]
        public IHttpActionResult GetCardByNumber(string number)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _CardBusiness.GetCard(number);
            if (result != null)
            {
                if (_CardBusiness.VerifyCardOwnership(result.CardId, cardholder.CardholderId))
                {
                    return Ok(result);
                }
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any card with this number associated with this user account.");
        }

        [HttpGet]
        public IHttpActionResult GetUserCards()
        {
            List<StoneWalletLibrary.Models.Card> result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
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
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult CreateCard([FromBody]Card card)
        {
            StoneWalletLibrary.Models.Card result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            if (card.Cardholder == null)
            {
                var cardModel = card.ToModel();
                cardModel.Cardholder = cardholder;
                result = _CardBusiness.CreateCard(cardModel);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "The cardholder property should be null to create a new card. The cardholder associated with the current user will be set to this card automatically");
            }

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Card not created.");
            }
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult PayCredit(int cardId, decimal value)
        {
            StoneWalletLibrary.Models.Card result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());

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
            return Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCard(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
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
