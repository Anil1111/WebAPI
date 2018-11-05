using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/************************************************************************************
 *                      This File Marked for deletion.
 * **********************************************************************************/

namespace WebAPI.Controllers
{
    public class DataController : Controller
    {
        [OutputCache(Duration = 10)]
        public string Index()
        {
            return DateTime.Now.ToString("T");
        }
    }
}