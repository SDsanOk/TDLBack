using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace simpleApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public UnauthorizedResult unauthorized()
        {
            return Unauthorized();
        }
    }
}