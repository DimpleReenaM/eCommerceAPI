using server.Entities;

namespace server.Interface.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(string email);

        Task<bool> AddUser(User user);

        Task<bool> UpdateUser(User user);

        Task<List<User>> GetAllUsers();
        Task<IEnumerable<Address>> GetAllAddressByUserId(int userId);
        Task<bool> AddAddress(Address address);
        Task<bool> UpdateAddress(Address address);
        Task<bool> RemoveAddress(int addressId);
        Task<User?> GetUserByResetToken(string token);

        Task<List<User>> GetUsersByRole(string role);


    }
}
