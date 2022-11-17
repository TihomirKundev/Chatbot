using ChatBot.Extensions;
using ChatBot.Models;
using ChatBot.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace ChatBot.Repositories.EFC;



public class DatabaseContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}
       
        
    public DbSet<User> Users { get; set; }
    public DbSet<Conversation> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<TicketDTO> Participants { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Conversation>(builder =>
    //     {
    //         builder.HasKey(c => c.ID);
    //         builder.HasMany(c => c.Messages);
    //         builder.HasMany(c => c.Participants);
    //     });
    //
    //     modelBuilder.Entity<Message>(builder =>
    //     {
    //         builder.HasKey(m => m.ID);
    //         builder.HasOne(m => m.Author);
    //         builder.Property(m => m.Timestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
    //     });
    //
    //     modelBuilder.Entity<TicketDTO>(builder =>
    //     {
    //         builder.HasKey(m => m.ticketnumber);
    //     });
    //
    //     modelBuilder.Entity<Participant>(builder =>
    //     {
    //         builder.Property(p => p.ID);
    //     });
    // }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseMySql(ServerVersion.AutoDetect("server=127.0.0.1;uid=admin;pwd=12345;database=basworld")); 
    //
    // }
}
