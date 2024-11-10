using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IEmployeeService
{
    List<Employee> Juniors { get; }
    List<Employee> TeamLeads { get; } 
    
    void HandleParticipantsList(Guid hackathonId);
    void PrepareWishLists();
}

public class EmployeeService(IEmployeeRepository repository, IOptions<ConfigOptions> options) : IEmployeeService
{
    
    private static readonly ILog Logger = LogManager.GetLogger(typeof(EmployeeService));
    
    public List<Employee> Juniors { get; set; } = [];
    public List<Employee> TeamLeads { get; set; } = [];
    
    public void HandleParticipantsList(Guid hackathonId)
    {
        Juniors = GetParticipants(hackathonId, options.Value.Hackathon!.JuniorsListPath, EmployeeRole.Junior);
        TeamLeads = GetParticipants(hackathonId, options.Value.Hackathon!.TeamLeadsListPath, EmployeeRole.TeamLead);
    }
    
    public void PrepareWishLists()
    {
        Juniors.ForEach(j =>
        {
            var mapping = j.HackathonEmployeeWishListMappings.Last();
            mapping.WishLists = GetWishLists(mapping.MappingId, TeamLeads);
        });
        TeamLeads.ForEach(tl =>
        {
            var mapping = tl.HackathonEmployeeWishListMappings.Last();
            mapping.WishLists = GetWishLists(mapping.MappingId, Juniors);
        });
        SaveEmployees();
    }

    private List<Employee> GetParticipants(Guid hackathonId, string path, EmployeeRole role)
    {
        return CsvParticipantsReader.ReadParticipants(hackathonId, path, role);
    }

    private static List<WishList> GetWishLists(Guid mappingId, List<Employee> colleagues)
    {
        var wishLists = new List<WishList>();
        var random = new Random();
        var distributionList = new List<int>();
        distributionList
            .AddRange(Enumerable.Range(0, colleagues.Count())
            .OrderBy(i => random.Next()));
        wishLists.AddRange
        (
            colleagues
                .Select((_, i) => 
                    WishList.Create(mappingId, i + 1, colleagues[distributionList[i]]))
        );
        return wishLists;
    }

    private void SaveEmployees()
    {
        var list = Juniors.Union(TeamLeads).ToList();
        repository.AddRange(list);
    }
    
    public static class CsvParticipantsReader
    {
    
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CsvParticipantsReader));
    
        public static List<Employee> ReadParticipants(Guid hackathonId, string filePath, EmployeeRole role)
        {
            var participants = new List<Employee>();
            try
            {
                using var reader = new StreamReader(filePath);
                {
                    if (!reader.EndOfStream)
                    {
                        reader.ReadLine();
                    }
                    while (!reader.EndOfStream)
                    {
                        var participantInfo = reader.ReadLine()!.Split(';');
                        var participant = new Employee
                        {
                            EmployeeId = int.Parse(participantInfo[0]),
                            FullName = participantInfo[1],
                            Role = role,
                        };
                        participant.HackathonEmployeeWishListMappings.Add
                        (
                            new()
                            {
                                HackathonId = hackathonId,
                                EmployeeId = participant.EmployeeId,
                                EmployeeRole = role
                            }
                        );
                        participants.Add(participant);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Fatal($"CsvReader: File path: {filePath}");
                Logger.Fatal("CsvReader: Exception: ", e);
                Environment.Exit(10);
            }
            return participants;
        }
    }
    
}