using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class GenericAPIController : ApiController
    {
        // https://weblog.west-wind.com/posts/2017/Sep/14/Accepting-Raw-Request-Body-Content-in-ASPNET-Core-API-Controllers?Page=2

        // POST: api/GenericAPI
        public IHttpActionResult Post([FromBody]string value)
        {
            return Ok(value);
        }

        // POST: api/GenericAPI
        public string PostRetStr([FromBody]string value)
        {
            //this.ResponseMessage().Response.Headers.Remove("content-type");
            //this.ResponseMessage().Response.Headers.Add("content-type", "text/plain");
            return ""; // Ok(value,);
        }


        //// GET: api/GenericAPI
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/GenericAPI/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// PUT: api/GenericAPI/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/GenericAPI/5
        //public void Delete(int id)
        //{
        //}
    }
}
