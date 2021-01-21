using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteAthon.Web.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteAthon.Web.Controllers
{
    [Authorize]
    [TypeFilter(typeof(GlobalExceptionFilter))]
    public class BaseController : Controller
    {
        [NonAction]
        protected string GetUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
        }

        [NonAction]
        protected string GetUserName()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value;
        }

    }
}
