using RedditImgDownloader.Models;

namespace RedditImgDownloader.Repositories
{
    ///<summary>
    /// The <c>IDownloadRepository</c> interface used for dependency injection
    ///</summary>
    public interface IDownloadRepository
    {
        string CreateJsonFile(string subreddit);
        string ReturnJsonFiles();
    }
}