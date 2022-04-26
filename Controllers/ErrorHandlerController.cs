using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMVC.Controllers
{
    public class ErrorHandlerController : Controller
    {
        [Route("ErrorHandler/{statusCode}")]
        public IActionResult StatusCodeErrorHandler(int statusCode)
        {
            switch (statusCode)
            {

                case 400:
                    ViewBag.Title = "Bad Request";
                    ViewBag.ErrorMessage = "The requested operation failed!!";
                    break;

                case 401:
                    ViewBag.Title = "Unauthorized";
                    ViewBag.ErrorMessage = "You must first authenticate to access";
                    break;

                case 403:
                    ViewBag.Title = "Forbidden";
                    ViewBag.ErrorMessage = "You do not have authorization to perform this operation!!";
                    break;


                case 404:
                    ViewBag.Title = "Not Found";
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found!!";
                    break;
            }

            
            return View("ErrorInfo");
        }
    }
}
