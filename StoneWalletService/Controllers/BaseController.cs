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
        protected IStoneWalletContext Context { get; set; }
        protected bool TestUserFlag;

        public BaseController()
        {
            Context = new StoneWalletContext();
        }

        public BaseController(IStoneWalletContext context)
        {
            Context = context;
        }

        protected override void Dispose(bool disposing)
        {
            Context.Dispose();
            base.Dispose(disposing);
        }

        protected string GetCurrentUserEmail()
        {
            if (TestUserFlag)
            {
                return "user@test.com";
            }
            else
            {
                return RequestContext.Principal.Identity.Name;
            }
        }
    }
}