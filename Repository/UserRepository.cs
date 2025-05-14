using server.Data;
using server.Entities;
using Microsoft.EntityFrameworkCore;
using server.Interface.Repository;

namespace server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContex contex;

        public UserRepository(DataContex contex) 
        {
            this.contex = contex;
        }

        public async Task<bool> AddUser(User user)
        {
            await this.contex.users.AddAsync(user);
            return await this.contex.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await this.contex.users.ToListAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await this.contex.users
                .Where(u => u.Email == email)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            this.contex.users.Attach(user);
            return await this.contex.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Address>> GetAllAddressByUserId(int userId)
        {
            return await this.contex.Address
            .Where(u => u.UserId == userId)
            .ToListAsync();
        }
        public async Task<bool> AddAddress(Address address)
        {
            await this.contex.Address.AddAsync(address);
            return await this.contex.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            this.contex.Address.Attach(address);
            return await this.contex.SaveChangesAsync() > 0;
        }
        public async Task<bool> RemoveAddress(int addressId)
        {
            await this.contex.Address.Where(a=>a.Id==addressId).ExecuteDeleteAsync();
            return await this.contex.SaveChangesAsync() > 0;
        }
        public async Task<User?> GetUserByResetToken(string token)
        {
            return await contex.users.FirstOrDefaultAsync(u => u.ResetPasswordToken == token);
        }
        public async Task<List<User?>> GetUsersByRole(string role)
        {
            return await contex.users
                   .Where(u => u.Role == role)
                   .ToListAsync();
        }
    }
}
