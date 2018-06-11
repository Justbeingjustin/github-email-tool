using Github_Email_Finder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Github.Services
{
    public interface IGithubUserRepository
    {
       Task<List<GithubUserWithImage>> GetGithubUsersByUserName(string userName);
    }
}
