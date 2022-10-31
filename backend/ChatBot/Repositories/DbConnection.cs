using ChatBot.Extensions;
using Microsoft.Extensions.Configuration;

namespace ChatBot.Repositories;

// May be better to inject IConfiguration directly to the repositories,
// although that would require specifying the connection string in every repository (as far as I know).
[SingletonService]
public class DbConnection : IDbConnection
{
    private IConfiguration Configuration { get; }

    public DbConnection(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public string GetConnectionString() => Configuration.GetConnectionString("DefaultConnection");
}
