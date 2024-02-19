using AutomapperWebAPI.Entities;
namespace AutomapperWebAPI.Services
{
    public interface IUserRepository
    {
        User CreateUser(User user);
        List<User> GetAllUser();
        User GetUserById(Guid guid);
    }
}
