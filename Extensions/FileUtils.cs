using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RedditImgDownloader.Models;

namespace RedditImgDownloader.Extensions
{
    public class FileUtils
    {
        private static readonly string directoryName = "JsonFiles";

        ///<summary>
        /// If <c>directoryName</c> directory does not exist, creates it
        ///</summary>
        public static void CreateDirectory()
        {
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }
        ///<summary>
        /// Creates json by given <c>WebImage</c> model and saves it to directory
        ///</summary>
        /// <param name="webImage">
        ///   A <see cref="WebImage"/> type representing a value.
        /// </param>
        /// <exception cref="Exception">
        ///   throws expcetion if error occurs during writing json file to the directory
        /// </exception>
        public static void CreateJsonFile(WebImage webImage)
        {
            if (webImage == null)
            {
                return;
            }
            try
            {
                string json = JsonSerializer.Serialize(webImage);
                var filePath = Path.Combine(directoryName, webImage.selectDate.ToString("yyyy-MM-dd-HH-mm-ss") + ".json");
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        ///<summary>
        /// Reads all json files from the directory to list, combines and returns it
        ///</summary>
        /// <exception cref="Exception">
        ///   throws expcetion if error occurs during reading json files from the directory
        /// </exception>
        /// <returns>A <see cref="string"/></returns>
        public static string GetAllJsonFilesFromDirectory()
        {
            try
            {
                List<string> jsonList = new List<string>();
                foreach (string fileName in Directory.GetFiles(directoryName, "*.json"))
                {
                    using (StreamReader r = new StreamReader(fileName))
                    {
                        string json = r.ReadToEnd();
                        jsonList.Add(json);
                    }
                }
                // this could handled better but I did not wanted to deserialize and serialize again
                var combinedJsons = "[" + String.Join(",", jsonList) + "]";
                return combinedJsons;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}