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
    public class CardholderController : BaseController
    {
        private IWalletBusiness _WalletBusiness;
        private ICardBusiness _CardBusiness;
        private ICardholderBusiness _CardholderBusiness;

        public CardholderController()
        {
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public CardholderController(IStoneWalletContext context) : base(context)
        {
            Configure(new WalletRepository(Context), new CardRepository(Context), new CardholderRepository(Context), new Clock());
        }

        public CardholderController(bool testUserFlag, IWalletRepository walletRepository, ICardRepository cardRepository, ICardholderRepository cardholderRepository, IClock clock)
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
        public IHttpActionResult GetCardholder()
        {
            StoneWalletLibrary.Models.Cardholder result = null;
            result = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult CreateCardholder([FromBody]Cardholder cardholder)
        {
            StoneWalletLibrary.Models.Cardholder result = null;
            if (String.IsNullOrEmpty(cardholder.Email?.Trim()))
            {
                cardholder.Email = GetCurrentUserEmail();
                result = _CardholderBusiness.CreateCardholder(cardholder.ToModel());
            }
            else if (cardholder.Email == GetCurrentUserEmail())
            {
                result = _CardholderBusiness.CreateCardholder(cardholder.ToModel());
            }

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not created.");
            }
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult EditCardholderName(string name)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            cardholder.Name = name;
            var result = _CardholderBusiness.EditCardholder(cardholder);
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to edit cardholder.");
            }
            return Ok(result);
        }

        [HttpPut]
        public IHttpActionResult EditCardholderNationalIdNumber(string nationalIdNumber)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            cardholder.NationalIdNumber = nationalIdNumber;
            var result = _CardholderBusiness.EditCardholder(cardholder);
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Error: Unable to edit cardholder.");
            }
            return Ok(result);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCardholder()
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(GetCurrentUserEmail());
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            return Ok(_CardholderBusiness.DeleteCardHolder(cardholder.CardholderId));
        }
    }
}
