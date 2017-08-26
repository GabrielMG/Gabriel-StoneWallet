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
    public class CardholderController : BaseController
    {
        private IWalletBusiness _WalletBusiness;
        private ICardBusiness _CardBusiness;
        private ICardholderBusiness _CardholderBusiness;

        public CardholderController()
        {
            var walletRepository = new WalletRepository(context);
            var cardRepository = new CardRepository(context);
            var cardholderRepository = new CardholderRepository(context);
            _WalletBusiness = new WalletBusiness(walletRepository, cardRepository);
            _CardBusiness = new CardBusiness(cardRepository, _WalletBusiness);
            _CardholderBusiness = new CardholderBusiness(cardholderRepository, _CardBusiness, _WalletBusiness);
        }

        [HttpGet]
        public IHttpActionResult GetCardholder()
        {
            StoneWalletLibrary.Models.Cardholder result = null;
            result = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            return Ok(Cardholder.ToViewModel(result));
        }

        [HttpPost]
        public IHttpActionResult CreateCardholder([FromBody]Cardholder cardholder)
        {
            StoneWalletLibrary.Models.Cardholder result = null;
            if (String.IsNullOrEmpty(cardholder.Email?.Trim()))
            {
                cardholder.Email = RequestContext.Principal.Identity.Name;
                result = _CardholderBusiness.CreateCardholder(cardholder.ToModel());
            }
            else if (cardholder.Email == RequestContext.Principal.Identity.Name)
            {
                result = _CardholderBusiness.CreateCardholder(cardholder.ToModel());
            }

            if (result == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not created.");
            }
            return Ok(Cardholder.ToViewModel(result));
        }

        [HttpPut]
        public IHttpActionResult EditCardholderName(string name)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
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
            return Ok(Cardholder.ToViewModel(result));
        }

        [HttpPut]
        public IHttpActionResult EditCardholderNationalIdNumber(string nationalIdNumber)
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
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
            return Ok(Cardholder.ToViewModel(result));
        }

        [HttpDelete]
        public IHttpActionResult DeleteCardholder()
        {
            var cardholder = _CardholderBusiness.GetCardholderByEmail(RequestContext.Principal.Identity.Name);
            if (cardholder == null)
            {
                return Content(HttpStatusCode.BadRequest, "Cardholder not found.");
            }
            return Ok(_CardholderBusiness.DeleteCardHolder(cardholder.CardholderId));
        }
    }
}
