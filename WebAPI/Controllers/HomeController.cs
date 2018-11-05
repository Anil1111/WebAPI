using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace WebAPI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public HomeController()
        {
            Debug.WriteLine("HomeController::ctor");
        }

        public ActionResult Index()
        {
            StackFrame sf = new StackFrame();
            StackTrace st = new StackTrace();

            Debug.WriteLine("HomeController::Index");
            ViewBag.Title = "Home Page";

            return View();
        }

        // [Route("Grouper/{action}?")]
        public ActionResult Grouper(string action = "Grouper")
        {
            if (action.ToUpper().Contains("GROUPER"))
            {
                // ViewBag.Title = "Grouping Scorer";
                return View(); // "~/View/Grouper/Grouper.cshtml");
            }
            else
            {
                //ViewBag.Title = "Assignment";
                return View("~/Views/Grouper/Assignment.md");
            }
        }


        protected override void HandleUnknownAction(string actionName)
        {
            try
            {
                View(actionName).ExecuteResult(ControllerContext);
            }
            catch
            {
                View(this.HttpNotFound());
            }
        }

        protected override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            this.OnException(filterContext);
        }

        //protected override void HandleUnknownAction(string actionName)
        //{
        //    // this.HandleUnknownAction(actionName);
        //    try
        //    {
        //        View(actionName).ExecuteResult(ControllerContext);
        //    }
        //    catch (InvalidOperationException ieox)
        //    {
        //        ViewData["error"] = "Unknown Action: \"" + Server.HtmlEncode(actionName) + "\"";
        //        ViewData["exMessage"] = ieox.Message;
        //    }
        //}

        //protected override System.Web.Mvc.IActionInvoker CreateActionInvoker()
        //{
        //    return this.CreateActionInvoker();
        //}
        ////protected override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext filterContext)
        //{
        //    this.OnActionExecuted(filterContext);
        //}

    }
}
