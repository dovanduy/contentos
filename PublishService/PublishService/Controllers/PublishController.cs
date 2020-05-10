using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublishService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync(string code)
        {
            string url = "";

            string user_token;

            string page_token;

            using (var http = new HttpClient())
            {
                url = "https://graph.facebook.com/oauth/access_token?client_id=1733081480160604&redirect_uri=https://localhost:44348/api/publish/&client_secret=69301b3208e86a66d904de4e7bd7bc6d&code=" + code;

                var httpResponse = await http.GetAsync(url);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<dynamic>(httpContent);
                user_token = result["access_token"];
            }

            using (var http = new HttpClient())
            {
                url = "https://graph.facebook.com/me/accounts?access_token=" + user_token;

                var httpResponse = await http.GetAsync(url);
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<dynamic>(httpContent);
                page_token = result["data"][0]["access_token"];
            }

            return Ok(page_token);
        }
    }
}
