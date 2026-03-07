using System;
using CustomerService.DAL.Models;
using CustomerService.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private CustomerContext customerContext;
		public UserRepository(CustomerContext customerContext)
		{
			this.customerContext = customerContext;
		}

		public async Task<User?> GetUserByUserName(string userName)
		{
            return await customerContext.Users.Include(u => u.UserRoles)
						.ThenInclude(r => r.Role)
						.FirstOrDefaultAsync(u => u.UserName == userName);
		}
		public async Task<bool> IsValidUserName(string userName)
		{
			return await customerContext.Users.AnyAsync(x => x.UserName == userName);
		}
        public async Task<bool> IsCustomerAsync(long userID)
        {
            var user = await customerContext.Users
						.Include(u => u.UserRoles)
                        .ThenInclude(r => r.Role)
                        .FirstOrDefaultAsync(u => u.ID == userID);
			if (user == null) return false;
			return user.UserRoles.Any(ur => ur.Role.Name == "Customer");
        }
        public async Task<User> CreateUser(User user, string role)
		{
			await customerContext.Users.AddAsync(user);
			await customerContext.SaveChangesAsync();

            // Assign default Customer role
            var roles = await customerContext.Roles.FirstAsync(r => r.Name == role);
            customerContext.UserRoleMappings.Add(new UserRoleMapping
            {
                UserID = user.ID,
                RoleID = roles.ID
            });

            await customerContext.SaveChangesAsync();
			return user;
        }

    }
}

