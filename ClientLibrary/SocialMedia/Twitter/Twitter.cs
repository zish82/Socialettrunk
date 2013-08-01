using System;

namespace ClientLibrary.SocialMedia.Twitter
{
    /// <summary>
    ///     Summary description for Tweet
    /// </summary>
    public class Twitter : BaseEntity
    {
        private const int TweetLength = 140;

        public Twitter(string tweet)
        {
            if (string.IsNullOrEmpty(tweet) && tweet.Length > TweetLength)
                throw new Exception("Tweet cannot be null, empty or less than 140 characters.");


        }

        public string Tweet { get; set; }

        public void Post()
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}