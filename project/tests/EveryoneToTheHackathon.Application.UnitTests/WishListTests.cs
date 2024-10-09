using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Options;
using Microsoft.Extensions.Configuration;

namespace EveryoneToTheHackathon.Application.UnitTests;

public class WishListTests
{

    public WishListTests()
    {
        ConfigOptions configOptions = new();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        config.GetSection("ParticipantList").Bind(configOptions);
    }

    [Fact]
    public async Task WishList_Should_MatchNumberJuniorsOrTeamLeads()
    {
        //Arrange
        var teamLeads = CSVParticipantsReader.ReadParticipants(ConfigOptions.TeamLeadsListPath);
        var juniors = CSVParticipantsReader.ReadParticipants(ConfigOptions.JuniorsListPath);

        //Act
        var someJuniorWishList = new WishList(teamLeads);
        var someTeamLeadWishList = new WishList(juniors);
        
        //Assert
        Assert.Equal(someJuniorWishList.ParticipantWishList.Count(), teamLeads.Count);
        Assert.Equal(someTeamLeadWishList.ParticipantWishList.Count(), juniors.Count);
    }
    
    [Fact]
    public async Task SpecificParticipant_Should_BeInWishList()
    {
        //Arrange
        Random rand = new();
        var someParticipantList = new List<HackathonParticipant>
        {
            new(1, "Aleksander Fomin"),
            new(2, "Yana Chernovskaya"),
            new(3, "Michael Jackson"),
        }.OrderBy(_ => rand.Next()).ToList();
        var specificParticipant = someParticipantList.First();
        
        //Act
        var someWishList = new WishList(someParticipantList);
        
        //Assert
        Assert.True(someWishList.ParticipantWishList.ContainsKey(specificParticipant));
    }
}