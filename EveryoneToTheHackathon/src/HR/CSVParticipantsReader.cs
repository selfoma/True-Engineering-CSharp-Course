using log4net;
using  EveryoneToTheHackathon.HackathonParticipants;


namespace EveryoneToTheHackathon.HR;

public static class CSVParticipantsReader
{
    
    private static readonly ILog _logger = LogManager.GetLogger(typeof(CSVParticipantsReader));
    
    public static List<HackathonParticipant>? ReadParticipants(string filePath)
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
                    string[] participantFullName = participantInfo[1].Split(' ');
                    participants.Add(new HackathonParticipant(
                        Convert.ToInt32(participantInfo[0]),
                        participantFullName[0],
                        participantFullName[1])
                    );
                }
            }
        }
        catch (Exception e)
        {
            _logger.Fatal("Parsing CSV failure!");
            _logger.Fatal(e);
            _logger.Info("Abort!");
            Environment.Exit(-150);
        }

        return participants;
    }
}