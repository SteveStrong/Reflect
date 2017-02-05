using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Foundry.Extensions;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using HtmlAgilityPack;

namespace Reflect.Controllers
{



    [Route("api/[controller]")]
    public class GrievanceController : Controller
    {
        private EntityFactory _factory = new EntityFactory();



        //https://blogs.msdn.microsoft.com/dotnet/2016/02/10/porting-to-net-core/
        // GET api/values
        [HttpGet]
        [Route("MergeToObj")]
        public IActionResult GetMergeTo()
        {
            var path = @"./ConfigData/grievances.json";
            var document = JsonConvert.DeserializeObject<JToken>(System.IO.File.ReadAllText(path));
            
            //push the string into the document
            //then extract the fields from the document and fill the object

            var test = new Grievance()
            {
                Name = "Steve",
                Document = JsonConvert.SerializeObject(document.First),
            };

            _factory.Hydrate<Grievance>(test, test.Document);

            return Json(test);
        }

        [HttpGet]
        [Route("ExtractFromObj")]
        public IActionResult GetExtractFrom()
        {
            var path = @"./ConfigData/grievances.json";
            var document = JsonConvert.DeserializeObject<JToken>(System.IO.File.ReadAllText(path));
            var source = document.First;

            //push the string into the document
            //set some values into the object then serialize the object and
            //push this into the string

            var test = new Grievance()
            {
                Id = "Steve",
                DateFiled = DateTime.Now.AddDays(-10),
                Document = JsonConvert.SerializeObject(source),
            };

            _factory.Rehydrate<Grievance>(test, test.Document);

            return Json(test);
        }

        // GET: api/Config/Grievance
        [HttpGet]
        [Route("All")]
        public IActionResult GetAll()
        {
            var path = @"./ConfigData/grievances.json";
            var result = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));
            return Json(result);
        }


        

        // GET: api/Config/Grievance
        [HttpGet]
        [Route("Doc")]
        public IActionResult GetDoc()
        {
            var path = @"./ConfigData/grievances.json";
            var result = JsonConvert.DeserializeObject(System.IO.File.ReadAllText(path));

            var test = new Grievance()
            {
                Name = "Steve",
                Document = JsonConvert.SerializeObject( result),
            };

            try
            {
                //var json = JsonConvert.DeserializeObject(test.Document);
                return Json(test);
            } catch(Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpGet]
        [Route("GirlGamers")]
        public async Task<String> GetGirlGamers()
        {
            var path = @"https://www.reddit.com/r/GirlGamers/comments/5rvftn/did_a_particular_person_get_you_started_in_gaming/";

            String product = null;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                product = await response.Content.ReadAsStringAsync();
                var html = new HtmlDocument();
                return product;
            }
            return product;
        }

    }
}
