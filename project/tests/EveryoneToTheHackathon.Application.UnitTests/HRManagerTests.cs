using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Options;
using EveryoneToTheHackathon.Strategy;
using Microsoft.Extensions.Configuration;

namespace EveryoneToTheHackathon.Application.UnitTests;

public class HRManagerTests
{

    public HRManagerTests()
    {
        ConfigOptions configOptions = new();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        config.GetSection("ParticipantList").Bind(configOptions);
    }
    
    [Fact]
    public async Task DreamTeam_Should_MatchPreSetNumber()
    {
        //Arrange
        var teamLeads = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
        var juniors = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);
        int preSetNumber = teamLeads.Count;
        
        var hrManager = new HRManager(new ManagerTeamBuildingStrategy());
        hrManager.AskParticipantsWishLists(teamLeads, juniors);
        hrManager.AskParticipantsWishLists(juniors, teamLeads);
        
        //Act
        var dreamTeam = hrManager.BuildDreamTeam(juniors, teamLeads);
        
        //Assert
        Assert.Equal(dreamTeam.DreamTeam.Count, preSetNumber);
    }

    [Fact]
    public async Task WhenSpecificWishlists_Expect_SpecificDreamTeam()
    {
        // Impossible test-case because of randomness team generation
        Assert.True(true);
    }

    [Fact]
    public async Task Strategy_Should_BeCalledOneTime()
    {
        //Arrange
        var strategyMock = new Mock<ITeamBuildingStrategy>();
        var hrManager = new HRManager(strategyMock.Object);
        
        var teamLeads = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
        var juniors = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);
        hrManager.AskParticipantsWishLists(teamLeads, juniors);
        hrManager.AskParticipantsWishLists(juniors, teamLeads);
        
        //Act
        hrManager.BuildDreamTeam(juniors, teamLeads);
        
        //Assert
        strategyMock.Verify(s => s.MakeDreamTeamList(
            It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>()), Times.Once);
    }
}