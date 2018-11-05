using System;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using WebAPI.Filters;
using WebAPI.Models;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ExceptionFilter]
    public class GrouperController : ApiController
    {

        // POST api/<controller>
        // *** IMPORTANT ***: If this signature is used:
        //    public async IHttpActionResult Post([FromBody]string value)
        // Then you are subjecting the post data to the default MVC parsing of the
        // content before it gets to this method.  This is impractical esp since
        // it does not have a content-type: "Text/plain" parser.  If a request does 
        // come in with that content type, no error is created and string value is
        // always empty.
        /// <summary>
        /// Calls class to take input from post and calculate a score base on xy&z.
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Post([NakedBody] string testStr) //[FromBody] string data)
        {
            if (String.IsNullOrEmpty(testStr))
            {
                testStr = Request.Content.ReadAsStringAsync().Result;
            }

            if (String.IsNullOrEmpty(testStr))
            {
                NameValueCollection nvc = Request.Content.ReadAsFormDataAsync().Result;
                foreach (string key in nvc)
                {
                    testStr += nvc[key].ToString();
                }
            }
            
            if (String.IsNullOrEmpty(testStr) && Request.Content.IsMimeMultipartContent())
            {
                Collection<HttpContent> hcc = Request.Content.ReadAsMultipartAsync().Result.Contents;
                foreach (var item in hcc)
                {
                    testStr += item.ReadAsStringAsync().Result;
                }
            }

            if (String.IsNullOrEmpty(testStr))
            {
#if DEBUG
                return BadRequest("Empty Input String");
#else
                return Ok("0");
#endif
            }
            else
            {
                try
                {
                    if (Request.Content.Headers.ContentType.MediaType.ToUpper().Contains("URLENCODED"))
                    {
                        testStr = WebUtility.UrlDecode(testStr);
                    }
                    Grouper grpr = new Grouper(testStr.ToString());
                    return Ok(grpr.score().ToString());
                }
                catch (Exception e)
                {
                    return BadRequest("Error with the supplied data:\r\n" + e.ToString() +
                           "\r\n" + testStr.ToString().Substring(0, (testStr.Length > 2000? 2000: testStr.Length)));
                }
            }
        }


        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}
        //// GET api/<controller>
        //public ActionResult Get()
        //{
        //    return null; // View("Grouper");
        //}
        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}