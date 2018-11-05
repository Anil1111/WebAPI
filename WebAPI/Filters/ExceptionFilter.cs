using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;


namespace WebAPI.Filters
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        /// <summary>
        /// In iExceptionFilter that HandleErrorAttribute implements.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            Log(filterContext.Exception);
            //base.OnException(filterContext);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    filterContext.ExceptionHandled = true;

        //    // Redirect on error:
        //    filterContext.Result = RedirectToAction("Index", "Error");

        //    // OR set the result without redirection:
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewName = "~/Views/Error/Index.cshtml"
        //    };
        //}

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="exception"></param>
        private void Log(Exception exception)
        {
            //log exception here..

        }

    }
}