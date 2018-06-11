namespace Github.Services
{
    public class GithubUserRepositoryFactory : IGithubUserRepositoryFactory
    {
        public IGithubUserRepository Create(string username, string password)
        {
            return new GithubUserRepository(username, password);
        }
    }
}
