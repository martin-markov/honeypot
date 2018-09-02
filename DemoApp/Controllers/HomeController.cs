using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Honeypot.Filter;
using Honeypot.Helpers;

namespace DemoApp
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [HoneypotFilter("HolderName", "ValidDate", "CardNumber", "SecurityCode")]
        public ActionResult PostCreditCardData2(CreditCard model)
        {
            if (Request.Form.AllKeys.Contains("HasHoneypotTrapped"))
            {
                return Json("/Home/BotPage");
            }
            else
            {
                return View("~/Views/Home/ThankYou.cshtml");
            }
        }
        [HttpPost]
        [HoneypotFilter(typeof(CreditCard))]
        public ActionResult PostCreditCardData(CreditCard model)
        {
            if (HttpContext.Request.HasHoneypotTrapped())
            {
                return Json("/Home/BotPage");

            }
            else
            {
                return View("~/Views/Home/ThankYou.cshtml");
            }
        }

        public ActionResult BotPage()
        {
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }

    }
}
