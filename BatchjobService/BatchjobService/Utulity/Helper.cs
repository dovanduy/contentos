using HtmlAgilityPack;
using iText.StyledXmlParser.Jsoup;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;

namespace BatchjobService.Utulity
{
    public static class Helper
    {
        public static string removeHtml(string content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var imgs = doc.DocumentNode.Descendants("img");

            foreach (var img in imgs)
            {
                var tr = img.ParentNode.ParentNode.ParentNode;
                if (tr != null)
                {
                    HtmlNode sibling = tr.NextSibling;

                    while (sibling != null)
                    {
                        if (sibling.NodeType == HtmlNodeType.Element)
                        {
                            sibling.RemoveAll();
                            break;
                        }

                        sibling = sibling.NextSibling;
                    }
                }
            }

            var con = Regex.Replace(doc.DocumentNode.InnerHtml, "<[br/].*?>", Environment.NewLine);

            var con1 = Regex.Replace(con, "<[a,i,strong,b/].*?>", "");

            //var con1 = Regex.Replace(con, "<br>", Environment.NewLine);

            var con2 = Regex.Replace(con1, "<[a-zA-Z/].*?>", Environment.NewLine);

            var con3 = Regex.Replace(con2, "&nbsp;", "");
            return con3;
        }

        public static List<string> getImage(string content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var imgs = doc.DocumentNode.Descendants("img");
            List<string> listImg = new List<string>();

            foreach(var img in imgs)
            {
                listImg.Add(img.GetAttributeValue("src", null));
            }

            return listImg;
        }

        public static async Task<string> FBTokenValidate(string token)
        {
            using (var http = new HttpClient())
            {
                var httpResponse = await http.GetAsync("https://graph.facebook.com/me?access_token=" + token);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(httpContent);

                if (results["error"] != null)
                {
                    return "";
                }
                string url = "https://www.facebook.com/" + results["id"];

                return url;
            }
        }

        public static async Task<string> WPTokenValidate(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var iis = handler.ReadToken(token).Issuer;
                var host = iis + "/wp-json/";
                var client = new WordPressClient(host);

                client.AuthMethod = AuthMethod.JWT;
                client.SetJWToken(token);

                if (await client.IsValidJWToken())
                {
                    return iis;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            } 
        }

        public static async Task<string> GetInteraction(string id, string token)
        {
            using (var http = new HttpClient())
            {
                var httpResponse = await http.GetAsync("https://graph.facebook.com/" + id + 
                    "?fields=shares,reactions.limit(0).summary(true),comments.summary(true)&access_token=" + token);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(httpContent);

                if (results["error"] != null)
                {
                    return "";
                }

                return httpContent;
            }
        }

        public static async Task<string> GetConversation(string id, string token)
        {
            using (var http = new HttpClient())
            {
                var httpResponse = await http.GetAsync("https://graph.facebook.com/" + id +
                    "/conversations?folder=inbox&access_token=" + token);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(httpContent);

                if (results["error"] != null)
                {
                    return "";
                }

                return httpContent;
            }
        }

        public static async Task<string> GetView(string id, string token)
        {
            using (var http = new HttpClient())
            {
                var httpResponse = await http.GetAsync("https://graph.facebook.com/" + id +
                    "/insights/post_engaged_users?access_token=" + token);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(httpContent);

                if (results["error"] != null)
                {
                    return "";
                }

                return httpContent;
            }
        }

        public static async Task<string> GetNewLike(string id, string token, DateTime? since)
        {
            using (var http = new HttpClient())
            {
                var httpResponse = await http.GetAsync("https://graph.facebook.com/" + id +
                    "/insights/page_fan_adds?since="+ since.Value.Date +"&access_token=" + token);

                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var results = JsonConvert.DeserializeObject<dynamic>(httpContent);

                if (results["error"] != null)
                {
                    return "";
                }

                return httpContent;
            }
        }
    }
}
