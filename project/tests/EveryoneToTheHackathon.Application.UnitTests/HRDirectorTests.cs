using EveryoneToTheHackathon.HackathonParticipants;
using EveryoneToTheHackathon.HR;
using EveryoneToTheHackathon.Strategy;
using Microsoft.VisualBasic;

namespace EveryoneToTheHackathon.Application.UnitTests;

public class HRDirectorTests : DreamTeamList
{

    [Fact]
    public async Task When_EqualHarmonicOfAllParticipants_Expect_ThisHarmonic()
    {
        //Arrange
        int expectedHarmonic = 10;
        var wishListMock = new Mock<IWishList>();
        var participantMock = new Mock<HackathonParticipant>();
        var strategy = new Mock<ITeamBuildingStrategy>();

        wishListMock.Setup(s => s.GetSatisfaction(It.IsAny<HackathonParticipant>())).Returns(10);
        participantMock.Setup(s => s.WishList).Returns(wishListMock.Object);
            
        var participants = Enumerable.Repeat(participantMock.Object, 3)
            .Select(p => new Tuple<HackathonParticipant, HackathonParticipant>(p, p))
            .ToList();
        strategy.Setup(s => s.MakeDreamTeamList(
            It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>())).Returns(
            new DreamTeamList(participants)
        );
        
        var dreamTeam = strategy.Object.MakeDreamTeamList(
            It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>());
        
        //Act
        double harmonic = dreamTeam.ComputeHarmonicMean();

        //Assert
        Assert.Equal(expectedHarmonic, harmonic);
    }

    [Fact]
    public async Task When_SpecificSatisfaction_Expect_SpecificHarmonic()
    {
        //Arrange 
        double expected1 = 3;
        double expected2 = 5;
        List<double> satisfaction1 = [2, 6];
        List<double> satisfaction2 = [2, 6, 15, 15];
        
        //Act
        double actual1 = ApplyFormula(satisfaction1);
        double actual2 = ApplyFormula(satisfaction2);
        
        //Assert
        Assert.Equal(expected1, actual1);
        Assert.Equal(expected2, actual2);
    }

    [Fact]
    public async Task When_SpecificWishListsAndDreamTeam_Expect_SpecificHarmonic()
    {
        //Arrange
        double[] expectedHarmonics = { 3, 5 };
        var dreamTeam1 = DreamTeamFactory([2, 6]);
        var dreamTeam2 = DreamTeamFactory([2, 6, 15, 15]);
            
        //Act
        double actual1 = dreamTeam1.ComputeHarmonicMean();
        double actual2 = dreamTeam2.ComputeHarmonicMean();
        
        //Assert
        Assert.Equal(expectedHarmonics[0], actual1);
        Assert.Equal(expectedHarmonics[1], actual2);
    }
    
    public static DreamTeamList DreamTeamFactory(List<int> satisfactions)
    {
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
        
        return strategy.Object.MakeDreamTeamList(
            It.IsAny<List<HackathonParticipant>>(), It.IsAny<List<HackathonParticipant>>());
    }
}