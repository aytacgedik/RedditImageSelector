using System;
using Microsoft.AspNetCore.Mvc;
using RedditImgDownloader.Repositories;

namespace RedditImgDownloader.Controllers
{
    ///<summary>
    /// The <c>ImageDownloadController</c> provides two method to
    /// first method selects random image from given repository and saves it's select date,
    /// second method returns all selected images and its selected date as json
    ///</summary>
    [ApiController]
    [Route("")]
    public class ImageDownloadController : ControllerBase
    {
        private readonly IDownloadRepository repository;

        ///<summary>
        /// Initializes a new instance of a
        /// <c>ImageDownloadController</c> type
        ///</summary>
        /// <param name="repository">
        ///   A <see cref="IDownloadRepository"/> type representing a value.
        /// </param>
        public ImageDownloadController(IDownloadRepository repository) //Inject the repository dependency
        {
            this.repository = repository;
        }

        ///<summary>
        /// Returns selected images and their select dates.
        /// In case of error, it returns error message.
        ///</summary>
        /// <exception cref="Exception">
        ///   if repository.ReturnJsonFiles method throws an exception.
        /// </exception>
        /// <returns>A <see cref="ContentResult" or A <see cref="NotFoundObjectResult"/></returns>
        [HttpGet]
        [Route("history")]
        public ActionResult ReturnJsonFiles()
        {
            try
            {
                var jsonFiles = repository.ReturnJsonFiles();
                return Content(jsonFiles, "application/json");
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        ///<summary>
        /// Takes subreddit as argument and saves image and its select date.
        /// In case of error, it returns error message.
        ///</summary>
        /// <param name="subreddit">
        ///   A <see cref="string"/> type representing a value.
        /// </param>
        ///</summary>
        /// <exception cref="Exception">
        ///   if repository.CreateJsonFile method throws an exception.
        /// </exception>
        /// <returns>A <see cref="string" or A <see cref="NotFoundObjectResult"/></returns>
        [HttpPost]
        [Route("random")]
        public ActionResult<string> CreateJsonFile([FromBody] string subreddit)
        {
            try
            {
                var url = repository.CreateJsonFile(subreddit);
                return Ok(url);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }

        }


    }
}