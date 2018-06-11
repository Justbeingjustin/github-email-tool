using Github.Models.Responses.Avatar;
using Github.Models.Responses.User;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Github_Email_Finder
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.github.com/users/jskeet/events/public");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            var response = client.Execute<List<GithubUserContainer>>(request);

            var data = response.Data.Where(x => x.payload != null
            && x.payload.commits != null  && x.payload.commits.Count > 0)
            .SelectMany(x => x.payload.commits).ToList();

            var gitHubUsers = data.Select(item => new GithubUser()
            {
                email = item.author.email,
                url = item.url
            }).GroupBy(x => x.email).Select(x => x.FirstOrDefault()).ToList();


            List<GithubUserWithImage> githubUsersWithImage = new List<GithubUserWithImage>();


            foreach (var item in gitHubUsers) {
                var client2 = new RestClient(item.url);
                var request2 = new RestRequest(Method.GET);
                request2.AddHeader("Cache-Control", "no-cache");
                var response2 = client2.Execute<GithubUserInfoContainer>(request2).Data;

                WebClient webClient = new WebClient();
                var result = webClient.DownloadData(response2.author.avatar_url);
                githubUsersWithImage.Add(new GithubUserWithImage() { email = item.email, image = result });
            }



          
        }
    }
}
