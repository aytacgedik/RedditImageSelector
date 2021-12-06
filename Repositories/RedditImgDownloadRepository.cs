using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using RedditImgDownloader.Extensions;
using RedditImgDownloader.Models;
using RedditImgDownloader.Models.RedditJson;

namespace RedditImgDownloader.Repositories
{
    ///<summary>
    /// The <c>RedditImgDownloadRepository</c> class implements
    /// the <c>IDownloadRepository</c>
    ///</summary>
    public class RedditImgDownloadRepository : IDownloadRepository
    {
        private readonly string sortBy;

        ///<summary>
        /// Initializes a new instance of a
        /// <c>RedditImgDownloadRepository</c>  
        /// assing sortBy to order posts
        /// Calls CreateDirectory method from <c>FileUils</c> 
        ///</summary>
        public RedditImgDownloadRepository()
        {
            sortBy = "top";
            FileUtils.CreateDirectory();

        }

        ///<summary>
        /// Creates an url to given subreddit and returns it
        ///</summary>
        /// <param name="subreddit">
        ///   A <see cref="string"/> type representing a value.
        /// </param>
        /// <returns>a <see cref="string"/></returns>
        private string CreateUrl(string subreddit)
        {
            return $"http://www.reddit.com/r/{subreddit}/{sortBy}.json"; // ?t=week can be added to only include last week
        }
        ///<summary>
        /// Returns all subreddit posts from given url
        ///</summary>
        /// <param name="url">
        ///   A <see cref="string"/> type representing a value.
        /// </param>
        /// <param name="webClient">
        ///   A <see cref="WebClient"/> type representing a value.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   throws ArgumentException if url is not correct
        /// </exception>
        /// <returns> a <see cref="IList<PostObject>" or null/>.</returns>
        private IList<PostObject> GetPosts(string url, WebClient webClient)
        {
            try
            {
                var jsonString = webClient.DownloadString(url);
                var posts = JsonSerializer.Deserialize<SubredditObject>(jsonString).data.children;
                return posts;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                throw new ArgumentException($"{url} does not exist");
            }
        }
        ///<summary>
        /// Selects random posts and saves its image url and select date as json.
        ///</summary>
        /// <param name="subreddit">
        ///   A <see cref="string"/> type representing a value.
        /// </param>
        /// <exception cref="ArgumentException">
        ///   throws ArgumentException if image does not exist in given subreddit
        /// </exception>
        /// <exception cref="Exception">
        ///   throws the exceptions throwed from used methods
        /// </exception>
        public void CreateJsonFile(string subreddit)
        {
            var webClient = new WebClient();
            var rand = new Random();
            try
            {
                string url = CreateUrl(subreddit);
                var posts = GetPosts(url, webClient);

                while (posts.Count > 0)
                {
                    var ranNumber = rand.Next(posts.Count);
                    var randomPost = posts[ranNumber].data;
                    if (randomPost.url.EndWithJpg())
                    {
                        var webImage = new WebImage { url = randomPost.url, selectDate = DateTime.Now };
                        FileUtils.CreateJsonFile(webImage);
                        return;
                    }
                    posts.RemoveAt(ranNumber);
                }
                throw new ArgumentException($"Image does not exist in given subreddit '{subreddit}'");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        ///<summary>
        /// Returns all the json files by calling GetAllJsonFilesFromDirectory method from FileUtils
        ///</summary>
        /// <exception cref="Exception">
        ///   throws the exceptions throwed from used method
        /// </exception>
        /// <returns>a <see cref="string"/></returns>
        public string ReturnJsonFiles()
        {
            try
            {
                var jsonFiles = FileUtils.GetAllJsonFilesFromDirectory();
                return jsonFiles;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}