using Microsoft.Extensions.Options;

namespace Fake_API.DAL.Repository.EFC.Options;

//resolved at runtime 
//di is used to resolve the options
//to apply/remove options update app-setting and restart the application
public class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions> 
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration; 
    }

    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString("Database");
        
        options.ConnectionString = connectionString!; 
        
        _configuration.GetSection(ConfigurationSectionName).Bind(options); //binds the options
    }
}