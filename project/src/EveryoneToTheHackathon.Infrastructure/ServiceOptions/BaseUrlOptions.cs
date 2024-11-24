namespace EveryoneToTheHackathon.Infrastructure.ServiceOptions;

public class BaseUrlOptions
{
    public string ManagerUrl { get; set; } = string.Empty;
    public string DirectorUrl { get; set; } = string.Empty;
    public List<string> EmployeeUrls { get; set; } = [];
}