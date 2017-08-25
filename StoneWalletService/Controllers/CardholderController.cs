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
        public IHttpActionResult GetCardholderById(int cardholderId)
        {
            return Ok(_CardholderBusiness.GetCardholderById(cardholderId));
        }

        [HttpGet]
        public IHttpActionResult GetCardholderByName(string name)
        {
            return Ok(_CardholderBusiness.GetCardholderByName(name));
        }

        [HttpGet]
        public IHttpActionResult GetCardholderByNationalIdNumber(string nationalIdNumber)
        {
            return Ok(_CardholderBusiness.GetCardholderByNationalIdNumber(nationalIdNumber));
        }

        [HttpGet]
        public IHttpActionResult GetCardholderByEmail(string email)
        {
            return Ok(_CardholderBusiness.GetCardholderByEmail(email));
        }

        [HttpPost]
        public IHttpActionResult CreateCardholder([FromBody]Cardholder cardholder)
        {
            if (String.IsNullOrEmpty(cardholder.Email?.Trim()))
            {
                cardholder.Email = RequestContext.Principal.Identity.Name;
            }
            return Ok(_CardholderBusiness.CreateCardholder(cardholder.ToModel()));
        }

        [HttpPut]
        public IHttpActionResult EditCardholderName(int cardholderId, string name)
        {
            return Ok(_CardholderBusiness.EditCardholderName(cardholderId, name));
        }

        [HttpPut]
        public IHttpActionResult EditCardholderNationalIdNumber(int cardholderId, string nationalIdNumber)
        {
            return Ok(_CardholderBusiness.EditCardholderNationalIdNumber(cardholderId, nationalIdNumber));
        }

        [HttpDelete]
        public IHttpActionResult DeleteCardholder(int cardholderId)
        {
            return Ok(_CardholderBusiness.DeleteCardHolder(cardholderId));
        }
    }
}
