namespace EveryoneToTheHackathon.Application.UnitTests;

public class HackathonTests
{
    [Fact]
    public async Task When_SpecificParticipantsAndWishLists_Expect_SpecificHarmonicLevel()
    {
        //Arrange
        List<int> satisfactions = [15, 20, 17, 20, 19, 23, 28, 25, 21, 20];
        var dreamTeamList = HRDirectorTests.DreamTeamFactory(satisfactions);
        
        //Act
        double harmonicMean = dreamTeamList.ComputeHarmonicMean();
        
        //Assert
        Assert.InRange(harmonicMean, 15, 28);
    }
}