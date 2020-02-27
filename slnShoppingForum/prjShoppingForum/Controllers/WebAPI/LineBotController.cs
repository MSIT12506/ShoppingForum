using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace tw.com.essentialoil.Controllers.WebAPI
{
    public class LineBotController : ApiController
    {
        public class Test
        {
            public string p1 { get; set; }
            public string p2 { get; set; }
            public string p3 { get; set; }
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public object Post(Test t)
        {
            return new { t.p1, t.p2, t.p3 };
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}