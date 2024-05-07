using Broxel.Logger.Models;

namespace Broxel.Logger.Services
{
     public class UserService : IUserService
    {
        List<User> users = new List<User>()
        {
            new User(){Email= "alrare@hotmail.com", Password="12345"}
        };

        public bool IsUser(string email, string password) =>
            users.Where(d=>d.Email == email && d.Password == password).Count() > 0;
    }
}