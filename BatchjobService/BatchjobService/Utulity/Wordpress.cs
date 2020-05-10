using BatchjobService.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;

namespace BatchjobService.Utulity
{
    public class Wordpress
    {
        public async Task PublishSimplePost(Contents postText, string token) {

            var handler = new JwtSecurityTokenHandler();
            var iis = handler.ReadToken(token).Issuer;
            var host = iis + "/wp-json/";
            var client = new WordPressClient(host);

            client.AuthMethod = AuthMethod.JWT;
            client.SetJWToken(token);


            var post = new Post
            {
                Title = new Title(postText.Name),
                Content = new Content(postText.TheContent)
                //,
                //Tags = new int[] { 3, 4, 5, 6 }
            };

            if (await client.IsValidJWToken())
            {
                await client.Posts.Create(post);
            }
        }
    }
}
