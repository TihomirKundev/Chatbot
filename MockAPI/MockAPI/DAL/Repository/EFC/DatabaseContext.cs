using Microsoft.EntityFrameworkCore;
using MockAPI.Entities;

namespace MockAPI.DAL.Repository.EFC;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Company> Companies { get; set; }



}