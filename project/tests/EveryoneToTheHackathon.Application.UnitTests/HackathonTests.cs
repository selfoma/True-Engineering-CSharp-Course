using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Options;
using EveryoneToTheHackathon.Strategy;
using Microsoft.Extensions.Configuration;

namespace EveryoneToTheHackathon.Application.UnitTests;

public class HackathonTests
{

    public HackathonTests()
    {
        ConfigOptions configOptions = new();
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        config.GetSection("ParticipantList").Bind(configOptions);
    }
    
    [Fact]
    public async Task When_SpecificParticipantsAndWishLists_Expect_SpecificHarmonicLevel()
    {
        double expected = 5;
        List<int> satisfactions = [2, 6, 15, 15];
        
        var wishListMockList = satisfactions.Select(s => new Mock<IWishList>()).ToList();
        var participantMockList = satisfactions.Select(s => new Mock<HackathonParticipant>()).ToList();;
        for (int i = 0; i < satisfactions.Count; i++)
        {
            wishListMockList[i].Setup(s => s.GetSatisfaction(It.IsAny<HackathonParticipant>()))
                .Returns(satisfactions[i]);
            participantMockList[i].Setup(s => s.WishList).Returns(wishListMockList[i].Object);
        }

        var participants = new List<Tuple<HackathonParticipant, HackathonParticipant>>();
        for (int i = 0; i < satisfactions.Count; i += 2)
        {
            var pair = participantMockList.Skip(i).Take(2).ToList();
            participants.Add(new(pair[0].Object, pair[1].Object));
        }
        
        var strategy = new Mock<ITeamBuildingStrategy>();
        strategy.Setup(s => s.MakeDreamTeamList(
                It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>()))
            .Returns(new DreamTeamList(participants));

        var dreamTeam = strategy.Object.MakeDreamTeamList(It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>());
        var hrManager = new Mock<IHRManager>();
        hrManager.Setup(s => s.BuildDreamTeam(
            It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>()))
            .Returns(dreamTeam);
        var hackathon = new Mock<Hackathon>(hrManager.Object);
        
        //Act
        hackathon.Object.Hold();
        var actual = hackathon.Object.HarmonicMean;
        
        //Assert
        Assert.Equal(expected, actual);
    }
}