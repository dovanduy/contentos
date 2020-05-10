using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FBTest
{
    class Facebook
    {
        readonly string _accessToken;
        readonly string _pageID;
        readonly string _facebookAPI = "https://graph.facebook.com/";
        readonly string _pageEdgeFeed = "feed";
        readonly string _pageEdgePhotos = "photos";
        readonly string _postToPageURL;
        readonly string _postToPagePhotosURL;

        public Facebook(string accessToken, string pageID)
        {
            _accessToken = accessToken;
            _pageID = pageID;
            _postToPageURL = $"{_facebookAPI}{pageID}/{_pageEdgeFeed}";
            _postToPagePhotosURL = $"{_facebookAPI}{pageID}/{_pageEdgePhotos}";
        }

        /// <summary>
        /// Publish a simple text post
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="postText">Text for posting</param>
        public async Task<string> PublishSimplePost(string postText)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "message", postText }
                //{ "published", "false" },
                //{ "scheduled_publish_time", ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + 600).ToString()}//,
                // { "formatting", "MARKDOWN" } // doesn't work
            };

                var httpResponse = await http.PostAsync(
                    _postToPageURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return httpContent;
            }
        }

        /// <summary>
        /// Publish a post to Facebook page
        /// </summary>
        /// <returns>Result</returns>
        /// <param name="postText">Post to publish</param>
        /// <param name="pictureURL">Post to publish</param>
        public string PublishToFacebook(string postText, List<string> listPictureURL)
        {
            try
            {
                List<string> listPictureURLFB = new List<string>();

                foreach (var pictureURL in listPictureURL)
                {
                    // upload picture first
                    var rezImage = Task.Run(async () =>
                    {
                        using (var http = new HttpClient())
                        {
                            return await UploadPhoto(pictureURL);
                        }
                    });
                    var rezImageJson = JObject.Parse(rezImage.Result.Item2);

                    if (rezImage.Result.Item1 != 200)
                    {
                        try
                        {
                            return $"Error uploading photo to Facebook. {rezImageJson["error"]["message"].Value<string>()}";
                        }
                        catch (Exception ex)
                        {
                            return $"Unknown error uploading photo to Facebook. {ex.Message}";
                        }
                    }
                    // get post ID from the response
                    string postID = rezImageJson["id"].Value<string>();

                    listPictureURLFB.Add(postID);
                }

                // and update this post (which is actually a photo) with your text
                var rezText = Task.Run(async () =>
                {
                    using (var http = new HttpClient())
                    {
                        return await PublishPostWithPhotos(listPictureURLFB, postText);
                    }
                });
                var rezTextJson = JObject.Parse(rezText.Result.Item2);

                if (rezText.Result.Item1 != 200)
                {
                    try // return error from JSON
                    {
                        return $"Error posting to Facebook. {rezTextJson["error"]["message"].Value<string>()}";
                    }
                    catch (Exception ex) // return unknown error
                    {
                        // log exception somewhere
                        return $"Unknown error posting to Facebook. {ex.Message}";
                    }
                }

                return rezTextJson.ToString();
            }
            catch (Exception ex)
            {
                // log exception somewhere
                return $"Unknown error publishing post to Facebook. {ex.Message}";
            }
        }

        /// <summary>
        /// Upload a picture (photo)
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="photoURL">URL of the picture to upload</param>
        public async Task<Tuple<int, string>> UploadPhoto(string photoURL)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "url", photoURL },
                { "published", "false"}
            };

                var httpResponse = await http.PostAsync(
                    _postToPagePhotosURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }

        /// <summary>
        /// Update the uploaded picture (photo) with the given text
        /// </summary>
        /// <returns>StatusCode and JSON response</returns>
        /// <param name="postID">Post ID</param>
        /// <param name="postText">Text to add tp the post</param>
        public async Task<Tuple<int, string>> PublishPostWithPhotos(List<string> listPhotos, string postText)
        {
            using (var http = new HttpClient())
            {
                var postData = new Dictionary<string, string> {
                { "access_token", _accessToken },
                { "message", postText }
            };
                int count = 0;
                foreach(var photo in listPhotos)
                {
                    postData.Add("attached_media[" + count + "]", "{\"media_fbid\":\"" + photo + "\"}");
                    count++;
                }

                var httpResponse = await http.PostAsync(
                    _postToPageURL,
                    new FormUrlEncodedContent(postData)
                    );
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                return new Tuple<int, string>(
                    (int)httpResponse.StatusCode,
                    httpContent
                    );
            }
        }
    }
}
