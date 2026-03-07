using System;
using CustomerService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.DAL;
public class CustomerContext : DbContext
{
    public CustomerContext(DbContextOptions<CustomerContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRoleMapping> UserRoleMappings => Set<UserRoleMapping>();

}

