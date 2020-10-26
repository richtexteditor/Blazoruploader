using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blazor_Upload_App.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        public SampleDataController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        [Route("api/[controller]")]


        [HttpPost("[action]")]
        public async Task<string> Save()
        {
            string path = string.Empty;
            if (HttpContext.Request.Form.Files.Any())
            {
                foreach (var file in HttpContext.Request.Form.Files)
                {
                    path = Path.Combine(environment.ContentRootPath, "uploads", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            byte[] ByteArray = System.IO.File.ReadAllBytes(path);

            return Convert.ToBase64String(ByteArray);
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<SampleDataController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SampleDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SampleDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SampleDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
