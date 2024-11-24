namespace EveryoneToTheHackathon.Infrastructure.ServiceOptions;

public class ConfigOptions
{
    public HackathonOptions? Hackathon { get; set; }
    public DatabaseOptions? Database { get; set; }
    public LoggingOptions? Logging { get; set; }
    public ServicesOptions? Services { get; set; }
}