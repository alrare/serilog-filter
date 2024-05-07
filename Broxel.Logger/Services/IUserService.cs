
using Broxel.Logger.Models;

namespace Broxel.Logger.Services
{
    public interface IUserService
    {
        public bool IsUser(string email, string password);
    }
}