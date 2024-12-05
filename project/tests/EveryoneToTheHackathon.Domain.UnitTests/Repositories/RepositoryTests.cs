using EveryoneToTheHackathon.Domain.Contexts;
using EveryoneToTheHackathon.Domain.Entities;
using EveryoneToTheHackathon.Domain.Entities.Comparers;
using EveryoneToTheHackathon.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EveryoneToTheHackathon.Domain.UnitTests.Repositories;

public class RepositoryTests
{
    
    private static HackathonContext CreateInMemoryDbContext()
    {
        var options = CreateInMemoryDbContextOptions();
        var context = new HackathonContext(options);
        
        context.Database.OpenConnection();
        context.Database.EnsureCreated();
        
        return context;
    }
    
    private static DbContextOptions<HackathonContext> CreateInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<HackathonContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;
    }

    [Fact]
    public void AddHackathonInfo_ShouldSucceed()
    {
        using var context = CreateInMemoryDbContext();
        {
            
            // Make test data
            var hackathon = new Hackathon();

            var juniors = new List<Employee>
            {
                new()
                {
                    EmployeeId = 1,
                    Role = EmployeeRole.Junior,
                    FullName = "Александр Фомин",
                    HackathonEmployeeWishListMappings = new()
                    {
                        new()
                        {
                            EmployeeId = 1,
                            EmployeeRole = EmployeeRole.Junior,
                            HackathonId = hackathon.HackathonId,
                            WishLists = new()
                            {
                                new()
                                {
                                    PreferenceValue = 2,
                                    PreferredEmployeeId = 1,
                                    PreferredEmployeeRole = EmployeeRole.TeamLead
                                },
                                new()
                                {
                                    PreferenceValue = 1,
                                    PreferredEmployeeId = 2,
                                    PreferredEmployeeRole = EmployeeRole.TeamLead
                                }
                            }
                        }
                    }
                },
                new()
                {
                    EmployeeId = 2,
                    Role = EmployeeRole.Junior,
                    FullName = "Яна Черновская",
                    HackathonEmployeeWishListMappings = new()
                    {
                        new()
                        {
                            EmployeeId = 2,
                            EmployeeRole = EmployeeRole.Junior,
                            HackathonId = hackathon.HackathonId,
                            WishLists = new()
                            {
                                new()
                                {
                                    PreferenceValue = 1,
                                    PreferredEmployeeId = 2,
                                    PreferredEmployeeRole = EmployeeRole.TeamLead
                                },
                                new()
                                {
                                    PreferenceValue = 2,
                                    PreferredEmployeeId = 1,
                                    PreferredEmployeeRole = EmployeeRole.TeamLead
                                }
                            }
                        }
                    }
                }
            };

            var teamLeads = new List<Employee>
            {
                new()
                {
                    EmployeeId = 1,
                    Role = EmployeeRole.TeamLead,
                    FullName = "Александр Комаров",
                    HackathonEmployeeWishListMappings = new()
                    {
                        new()
                        {
                            EmployeeId = 1,
                            EmployeeRole = EmployeeRole.TeamLead,
                            HackathonId = hackathon.HackathonId,
                            WishLists = new()
                            {
                                new()
                                {
                                    PreferenceValue = 2,
                                    PreferredEmployeeId = 1,
                                    PreferredEmployeeRole = EmployeeRole.Junior
                                },
                                new()
                                {
                                    PreferenceValue = 1,
                                    PreferredEmployeeId = 2,
                                    PreferredEmployeeRole = EmployeeRole.Junior
                                }
                            }
                        }
                    }
                },
                new()
                {
                    EmployeeId = 2,
                    Role = EmployeeRole.TeamLead,
                    FullName = "Сергей Петров",
                    HackathonEmployeeWishListMappings = new()
                    {
                        new()
                        {
                            EmployeeId = 2,
                            EmployeeRole = EmployeeRole.TeamLead,
                            HackathonId = hackathon.HackathonId,
                            WishLists = new()
                            {
                                new()
                                {
                                    PreferenceValue = 1,
                                    PreferredEmployeeId = 2,
                                    PreferredEmployeeRole = EmployeeRole.Junior
                                },
                                new()
                                {
                                    PreferenceValue = 2,
                                    PreferredEmployeeId = 1,
                                    PreferredEmployeeRole = EmployeeRole.Junior
                                }
                            }
                        }
                    }
                }
            };

            var dreamTeams = new List<HackathonDreamTeam>
            {
                new()
                {
                    HackathonId = hackathon.HackathonId,
                    TeamLeadId = 1,
                    JuniorId = 1,
                },
                new()
                {
                    HackathonId = hackathon.HackathonId,
                    TeamLeadId = 2,
                    JuniorId = 2,
                },
            };
            
            // Action
            var employeeRepository = new EmployeeRepository(context);
            var hackathonRepository = new HackathonRepository(context);
            
            // Hackathon adding
            hackathonRepository.Add(hackathon);
            var existence = context.Hackathons.Find(hackathon.HackathonId);
            Assert.NotNull(existence);

            // Employees cascade adding mappings and wishlists
            var employees = juniors.Union(teamLeads).ToList();
            employees.ForEach(employeeRepository.Add);
            var employeeIntersection = employees
                .ToList().Intersect(context.Employees.ToList(), new EmployeeComparer());
            Assert.Equal(employees.Count, employeeIntersection.Count());
            
            // Employee existence check
            var employee = employeeIntersection.FirstOrDefault(e => 
                e.EmployeeId == 2 && e.FullName == "Яна Черновская" && e.Role == EmployeeRole.Junior);
            Assert.NotNull(employee);
            
            // Hackathon updating
            hackathonRepository.UpdateHarmonicById(hackathon.HackathonId, 10m);
            hackathonRepository.UpdateHackathonDreamTeams(hackathon.HackathonId, dreamTeams);
            var updated = context.Hackathons.Find(hackathon.HackathonId);
            Assert.Equal(10m, updated!.HarmonicMean);
            
            // DreamTeam check
            var dreamTeamIntersection = context.HackathonDreamTeams.
                ToList().Intersect(dreamTeams, new DreamTeamsComparer());
            Assert.Equal(dreamTeams.Count, dreamTeamIntersection.Count());
            var dreamTeam = dreamTeamIntersection.FirstOrDefault(dt => 
                dt.TeamLeadId == 2 && dt.JuniorId == 2);
            Assert.NotNull(dreamTeam);
        }
    }

    [Fact]
    public void GetOverallResult_ShouldReturnSpecificValue()
    {
        using var context = CreateInMemoryDbContext();
        {
            var hackathon1 = new Hackathon { HarmonicMean = 10 };
            var hackathon2 = new Hackathon { HarmonicMean = 10 };
            var hackathon3 = new Hackathon { HarmonicMean = 10 };
            
            var hackathonRepository = new HackathonRepository(context);
            hackathonRepository.Add(hackathon1);
            hackathonRepository.Add(hackathon2);
            hackathonRepository.Add(hackathon3);

            var expected = 10;
            var actual = hackathonRepository.GetAll()
                .Aggregate(0m, (x, y) => x + y.HarmonicMean) / 3;
            Assert.Equal(expected, actual);
        }
    }
    
}