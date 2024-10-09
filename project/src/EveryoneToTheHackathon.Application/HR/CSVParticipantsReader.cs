using log4net;
using  EveryoneToTheHackathon.HackathonParticipants;


namespace EveryoneToTheHackathon.HR;

public static class CSVParticipantsReader
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(CSVParticipantsReader));
    
    public static List<HackathonParticipant> ReadParticipants(string filePath)
    {
        List<HackathonParticipant> participants = new();
        
        try
        {
            using (var reader = new StreamReader(filePath))
            {
                if (!reader.EndOfStream)
                {
                    reader.ReadLine();
                }

                while (!reader.EndOfStream)
                {
                    string[] participantInfo = reader.ReadLine().Split(';');
                    participants.Add(new HackathonParticipant(
                        Convert.ToInt32(participantInfo[0]), participantInfo[1])
                    );
                }
            }
        }
        catch (Exception e)
        {
            Logger.Fatal("Parsing CSV failure!");
            Logger.Fatal(e);
            Logger.Info("Abort!");
            Environment.Exit(-2);
        }

        return participants;
    }
}