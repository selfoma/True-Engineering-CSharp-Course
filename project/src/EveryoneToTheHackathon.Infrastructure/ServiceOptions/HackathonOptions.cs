namespace EveryoneToTheHackathon.Infrastructure.ServiceOptions;

public class HackathonOptions
{
    public int TimesToHold { get; set; } = 0;
    public string JuniorsListPath { get; set; } = string.Empty;
    public string TeamLeadsListPath { get; set; } = string.Empty;
}