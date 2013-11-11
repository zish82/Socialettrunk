﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ClientLibrary.SocialMedia;

namespace Socialet.ViewModels
{
    public class FaceBookViewModel : AuthenticableSocialNetWorkViewModel
    {
        public FaceBookViewModel(IAmSocialNetworkAndICanAuthenticate socialNetwork)
            : base(socialNetwork)
        {
            Title = "facebuch";
            SubTitle = IsAuthenticated ? "Authenticated" : "Not authenticated";
            LogoUrl = @"Assets\FB-f-Logo__blue_512.png";

        }

        public override Task<IEnumerable<TweetViewModel>> GetTweets()
        {
            //try
            //{
            //    var client = new HttpClient();
            //    var response =  client.GetAsync(
            //      "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=zish");
            //    var text = response.Content.ReadAsStringAsync();

            //    XElement xmlTweets = XElement.Parse(text.Result);
            //    IEnumerable<TweetViewModel> data = from tweet in xmlTweets.Descendants("status")
            //                                       select new TweetViewModel
            //                                       {
            //                                           UserName = tweet.Element("user").Value,
            //                                           Message = tweet.Element("text").Value,
            //                                           Date = DateTime.Parse(tweet.Element("created_at").Value),
            //                                           StatusNumber = tweet.Element("id").Value
            //                                       };
            //    return data;
            //}
            //catch (Exception) // Not best practice
            //{
            //    return new TweetViewModel[0];
            //}
            return null;
        }
    }
}