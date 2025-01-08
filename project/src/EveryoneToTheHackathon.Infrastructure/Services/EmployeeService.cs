using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Repositories;
using EveryoneToTheHackathon.Infrastructure.ServiceOptions;
using log4net;
using Microsoft.Extensions.Options;

namespace EveryoneToTheHackathon.Infrastructure.Services;

public interface IEmployeeService
{
    Employee? GetThisEmployee();
    void HandleParticipantsList(Guid hackathonId);
    void PrepareWishLists();
}

public class EmployeeService : IEmployeeService
{
    private static readonly ILog Logger = LogManager.GetLogger(typeof(EmployeeService));

    private readonly IEmployeeRepository _repository;
    private readonly IOptions<ConfigOptions> _options;

    private readonly Employee _currentEmployee;
    
    public List<Employee> Colleagues { get; set; } = [];

    public EmployeeService(IEmployeeRepository repository, IOptions<ConfigOptions> options)
    {
        _repository = repository;
        _options = options;
        var role = EmployeeRoleExtensions.GetRole(Environment.GetEnvironmentVariable("role"));
        var id = Convert.ToInt32(Environment.GetEnvironmentVariable("id"));
        if (role is EmployeeRole.Junior)
        {
            _currentEmployee = CsvParticipantsReader.ReadParticipant(id, role, _options.Value.Hackathon!.JuniorsListPath);
        }
        else if (role is EmployeeRole.TeamLead)
        {
            _currentEmployee = CsvParticipantsReader.ReadParticipant(id, role, _options.Value.Hackathon!.TeamLeadsListPath);
        }
        else
        {
            Logger.Fatal($"Service constructor: [ROLE].{ role } invalid!");
            Environment.Exit(5);
        }
    }
    
    public Employee? GetThisEmployee()
    {
        return _currentEmployee;
    }

    public void HandleParticipantsList(Guid hackathonId)
    {
        _currentEmployee.HackathonEmployeeWishListMappings.Add(new HackathonEmployeeWishListMapping
        {
            HackathonId = hackathonId,
            EmployeeId = _currentEmployee.EmployeeId,
            EmployeeRole = _currentEmployee.Role,
        });
        if (_currentEmployee.Role is EmployeeRole.Junior)
        {
            Colleagues = GetParticipants(hackathonId, _options.Value.Hackathon!.TeamLeadsListPath, EmployeeRole.TeamLead);
        }
        else
        {
            Colleagues = GetParticipants(hackathonId, _options.Value.Hackathon!.JuniorsListPath, EmployeeRole.Junior);
        }
    }
    
    public void PrepareWishLists()
    {
        var mapping = _currentEmployee.HackathonEmployeeWishListMappings.Last();
        mapping.WishLists = GetWishLists(mapping.MappingId, Colleagues);
        SaveEmployee();
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

    private void SaveEmployee()
    {
        _repository.Add(_currentEmployee);
        Logger.Info("SaveEmployee: Saved!");
    }
    
    public static class CsvParticipantsReader
    {
    
        private static readonly ILog Log = LogManager.GetLogger(typeof(CsvParticipantsReader));
    
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
                Log.Fatal($"ReadParticipants: [PATH].{ filePath }.");
                Log.Fatal("Exception: ", e);
                Environment.Exit(10);
            }
            return participants;
        }
        
        public static Employee ReadParticipant(int employeeId, EmployeeRole role, string filePath)
        {
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
                        var id = int.Parse(participantInfo[0]);
                        if (id == employeeId)
                        {
                            return new Employee
                            {
                                EmployeeId = id, 
                                Role = role, 
                                FullName = participantInfo[1]
                            };
                        }
                    }
                    Log.Fatal($"ReadParticipant: [PATH].{ filePath }.");
                    Log.Fatal($"Employee: [ID].{ employeeId } - [ROLE].{ role } not found.");
                    Environment.Exit(10);
                }
            }
            catch (Exception e)
            {
                Log.Fatal($"ReadParticipant: [PATH].{ filePath }.");
                Log.Fatal("Exception: ", e);
                Environment.Exit(10);
            }
            return new();
        }
        
    }
}