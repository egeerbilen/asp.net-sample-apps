using AutomapperWebAPI.Entities;
namespace AutomapperWebAPI.Services
{
    public class UserRepository : IUserRepository
    {
        public static List<User> users = new List<User>()
        {
            new User { Id = new Guid("45408c64-1af9-4ea4-a9e0-f835b13d980b") ,
            Email = "janedoe@test.com" , FirstName = "Jane" , LastName = "Doe",Gender="Female",
            Password = "testPassword"},
            new User { Id = new Guid("804b906d-c6bc-4e58-b454-f3ce7cf7c3f1")  ,
            Email = "Johndoe@test.com" , FirstName = "John" , LastName = "Doe",Gender="Male",
            Password = "testPassword2"},
            new User { Id = new Guid("8335de6c-e315-492d-82b3-18f07aa150a9") ,
            Email = "maxwellpeter@test.com" , FirstName = "Maxwell" , LastName = "Peter",Gender="Male",
            Password = "testPassword3"},
            new User { Id = new Guid("8335de6c-e315-492d-82b3-18f07aa150a9") ,
            Email = "jjoyce@test.com" , FirstName = "Joyce" , LastName = "jane",Gender="Female",
            Password = "testPassword4"}
        };
        public User CreateUser(User user)
        {
            user.Id = Guid.NewGuid();
            users.Add(user);
            return user;
        }

        public List<User> GetAllUser()
        {
            return users;
        }

        public User GetUserById(Guid guid)
        {
            var user = users.FirstOrDefault(u => u.Id == guid);
            return user;
        }
    }
}
