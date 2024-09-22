using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Options;
using EveryoneToTheHackathon.Strategy;
using log4net;
using Microsoft.Extensions.Hosting;

namespace EveryoneToTheHackathon;

public class HackathonHost : BackgroundService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(HackathonHost));
    
    private readonly IServiceProvider _serviceProvider;
    
    public HackathonHost(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var hrDirector = (HRDirector)_serviceProvider.GetService(typeof(HRDirector));

            for (int i = 0; i < ConfigOptions.timesToHold; ++i)
            {
                var hrManager = (HRManager)_serviceProvider.GetService(typeof(HRManager));
                var hackathon = (Hackathon)_serviceProvider.GetService(typeof(Hackathon));

                hrManager.AskParticipantsWishLists();
                hrDirector.HoldHackathon(hackathon);
            }
            
            hrDirector.ShowAverageHarmonic();
        }
        catch (NullReferenceException e)
        {
            Logger.Fatal("Null reference service");
            Logger.Fatal(e);
            Environment.Exit(-1);
        }

        await Task.CompletedTask;
    }

}