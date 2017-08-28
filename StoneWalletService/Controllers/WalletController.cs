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
    public class WalletController : BaseController
    {
        private IWalletBusiness _WalletBusiness;
        private ICardBusiness _CardBusiness;
        private ICardholderBusiness _CardholderBusiness;

        public WalletController()
        {
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public WalletController(IStoneWalletContext context) : base(context)
        {
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public WalletController(bool testUserFlag, IWalletRepository walletRepository, ICardRepository cardRepository, ICardholderRepository cardholderRepository, IClock clock)
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
        public IHttpActionResult GetWallet()
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            var result = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (result != null)
            {
                return Ok(result);
            }
            return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
        }

        [HttpPost]
        public IHttpActionResult CreateWallet()
        {
            StoneWalletLibrary.Models.Wallet result = null;
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var walletModel = new StoneWalletLibrary.Models.Wallet();
            walletModel.Cardholder = cardholder;
            result = _WalletBusiness.CreateWallet(walletModel);

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Wallet not created.");
            }

            _CardholderBusiness.EditCardholder(result.Cardholder);
            
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult SetUserDefinedLimit(decimal value)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var wallet = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (wallet == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
            }

            if (_WalletBusiness.SetWalletUserDefinedLimit(wallet, value))
            {
                return Ok(true);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to set a credit limit");
            }
        }

        [HttpPost]
        public IHttpActionResult ExecutePurchase(decimal value)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }

            var wallet = _WalletBusiness.GetWallet(cardholder.CardholderId);
            if (wallet == null)
            {
                return Content(HttpStatusCode.BadRequest, "Couldn't find any wallet associated with this user account.");
            }

            var result = _WalletBusiness.ExecutePurchase(value, wallet);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to execute purchase");
            }
        }

        [HttpPut]
        public IHttpActionResult AddCardToWallet(int cardId)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
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
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
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
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "There is no cardholder associated with the current user.");
            }
            return Ok(_WalletBusiness.DeleteWallet(cardholder.CardholderId));
        }
    }
}
