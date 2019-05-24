using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPSnippets.FaceBookAPI;

namespace DeluxeProject.Controllers.UserController
{
    public class ThirdPartyController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public EmptyResult Login()
        {
            FaceBookConnect.Authorize("email", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, "Controllers/UserActions/SignUp/"));

            return new EmptyResult();
        }
    }
}