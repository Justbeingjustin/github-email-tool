using Github.Models.Responses.Avatar;
using Github.Models.Responses.User;
using Github_Email_Finder;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Github.Services
{
    public class GithubUserRepository : IGithubUserRepository
    {
        public readonly string _authorization;

        public GithubUserRepository(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) {
                throw new ArgumentNullException("expected a username or password");
            }
            _authorization =  Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
        }

        public async Task<List<GithubUserWithImage>> GetGithubUsersByUserName(string userName)
        {
            var gitHubUsers = await Task.FromResult(GetUsersLatestCommits(userName));

            List<GithubUserWithImage> listOfGithubUsers = new List<GithubUserWithImage>();

            foreach (var githubUser in gitHubUsers)
            {
                listOfGithubUsers.Add(GetGithubUserWithImage(githubUser));
            }

            return listOfGithubUsers;
        }


        private GithubUserWithImage GetGithubUserWithImage(GithubUser githubUser) {

            var client = new RestClient(githubUser.url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("authorization", "Basic " + _authorization);
            var response = client.Execute<GithubUserInfoContainer>(request).Data;
            WebClient webClient = new WebClient();
            var result = webClient.DownloadData(response.author.avatar_url); // this is the link the avatar image
            return new GithubUserWithImage()
            {
                email = githubUser.email,
                image = result,
                login = response.author.login
            };
        }

        private List<GithubUser> GetUsersLatestCommits(string userName)
        {

            var client = new RestClient($"https://api.github.com/users/{userName}/events/public");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("authorization", "Basic " + _authorization);
            var response = client.Execute<List<GithubUserContainer>>(request);

            var listOfCommitts = response.Data.Where(x => x.payload != null
            && x.payload.commits != null && x.payload.commits.Count > 0)
            .SelectMany(x => x.payload.commits).ToList(); // flatten to get list of commits

            return listOfCommitts.Select(item => new GithubUser()
            {
                email = item.author.email,
                url = item.url // this url is used to find the image
            }).GroupBy(x => x.email).Select(x => x.FirstOrDefault()).ToList();

        }
    }
}
