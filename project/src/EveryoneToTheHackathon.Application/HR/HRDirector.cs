using log4net;

namespace EveryoneToTheHackathon.HR;

public class HRDirector 
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HRDirector));

    private readonly List<Hackathon> _hackathonHistory = new();
    
    public void HoldHackathon(Hackathon hackathon)
    {
        Logger.Info("HR director is holding a series of hackathons...");
        
        hackathon.Hold();
        Console.WriteLine(hackathon);
        _hackathonHistory.Add(hackathon);
        
        Logger.Info("All done...");
    }

    public void ShowAverageHarmonic()
    {
        double res = 0;
        foreach (var hackathon in _hackathonHistory)
        {
            res += hackathon.HarmonicMean;
        }
        
        Console.WriteLine($"Average harmonic is { res / _hackathonHistory.Count }");
    }
    
}