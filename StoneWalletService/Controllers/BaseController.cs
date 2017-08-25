using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using StoneWalletLibrary.Data;


namespace StoneWalletService.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected StoneWalletContext context { get; set; }

        public BaseController()
        {
            context = new StoneWalletContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}