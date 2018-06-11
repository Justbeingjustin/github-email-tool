using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Github.Services
{
    public interface IGithubUserRepositoryFactory
    {
        IGithubUserRepository Create(string username, string password);
    }
}
