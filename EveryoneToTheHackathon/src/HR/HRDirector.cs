using log4net;

namespace EveryoneToTheHackathon.HR;

public class HRDirector
{

    private readonly List<Hackathon> _hackathonHistory;
    private readonly ILog _logger = LogManager.GetLogger(nameof(HRDirector));

    public HRDirector()
    {
        _logger.Info("HR director started working.");
        _hackathonHistory = new List<Hackathon>();
    }

    public void HoldHackathon(int timesToHold)
    {
        _logger.Info("HR director is holding a series of hackathons.");
        for (int i = 0; i < timesToHold; i++)
        {
            Hackathon hackathon = new();
            hackathon.Hold();
            Console.WriteLine(hackathon);
            _hackathonHistory.Add(hackathon);
        }
        _logger.Info("All done.");
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