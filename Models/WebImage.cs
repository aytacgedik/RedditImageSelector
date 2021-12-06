using System;
using System.ComponentModel.DataAnnotations;

namespace RedditImgDownloader.Models
{
    ///<summary>
    /// The <c>WebImage</c> model holds selected image url and date
    ///</summary>
    public record WebImage
    {
        [Required]
        public DateTime selectDate { get; init; }
        [Required]
        public string url { get; init; }
    }
}