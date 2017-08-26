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
    public class WalletController : BaseController
    {
        private IWalletBusiness _WalletBusiness;
        private ICardBusiness _CardBusiness;
        private ICardholderBusiness _CardholderBusiness;

        public WalletController()
        {
            var walletRepository = new WalletRepository(context);
            var cardRepository = new CardRepository(context);
            var cardholderRepository = new CardholderRepository(context);
            _WalletBusiness = new WalletBusiness(walletRepository, cardRepository);
            _CardBusiness = new CardBusiness(cardRepository, _WalletBusiness);
            _CardholderBusiness = new CardholderBusiness(cardholderRepository, _CardBusiness, _WalletBusiness);
        }

        [HttpGet]
        public IHttpActionResult GetWallet()
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (result != null)
            {
                return Ok(Wallet.ToViewModel(result));
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
        }

        [HttpPost]
        public IHttpActionResult CreateWallet([FromBody]Wallet wallet)
        {
            StoneWalletLibrary.Models.Wallet result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            if (wallet.Cardholder == null)
            {
                wallet.Cardholder = Cardholder.ToViewModel(cardholder);
                result = _WalletBusiness.CreateWallet(wallet.ToModel());
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "The cardholder property should be null to create a new wallet. The cardholder associated with the current user will be set to this wallet automatically");
            }

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Wallet not created.");
            }
            return Ok(Wallet.ToViewModel(result));
        }

        [HttpPost]
        public IHttpActionResult ExecutePurchase(decimal value)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var wallet = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (wallet == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
            }

            if (_WalletBusiness.ExecutePurchase(value, wallet))
            {
                return Ok(true);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to pay credit.");
            }
        }

        [HttpPut]
        public IHttpActionResult AddCardToWallet(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var wallet = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (wallet == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
            }
            var card = _CardBusiness.GetCardById(cardId);
            if (card == null)
            {
                return Content(HttpStatusCode.BadRequest, "Unable to find a card with this cardId.");
            }
            if (_WalletBusiness.AddCardToWallet(card, wallet))
            {
                return Ok(true);
            }
            return Content(HttpStatusCode.BadRequest, "Error: Unable to add this card to this user's wallet.");
        }

        [HttpPut]
        public IHttpActionResult RemoveCardFromWallet(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var wallet = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (wallet == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
            }
            var card = _CardBusiness.GetCardById(cardId);
            if (card == null)
            {
                return Content(HttpStatusCode.BadRequest, "Unable to find a card with this cardId.");
            }
            if (_WalletBusiness.RemoveCardFromWallet(card))
            {
                return Ok(true);
            }
            return Content(HttpStatusCode.BadRequest, "Error: Unable to remove this card from this user's wallet.");
        }

        [HttpDelete]
        public IHttpActionResult DeleteWallet()
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            return Ok(_WalletBusiness.DeleteWallet(cardholder.CardholderId));
        }
    }
}
