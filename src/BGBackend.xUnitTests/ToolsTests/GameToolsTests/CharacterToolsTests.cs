using BrowserGameBackend.Models;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Tools.GameTools;
using System.Diagnostics;

namespace BGBackend.xUnitTests.ToolsTests.GameToolsTests
{
    public class CharacterToolsTests
    {
        [InlineData(5, 5, 5, 15, 5)]
        [InlineData(5, 15, 5, 5, 5)]
        [InlineData(5, 5, 15, 5, 5)]
        [Theory]
        public void AssertUserSkillsAreValidForLevelUp(int spaceWarfare, int landWarfare, int research, int engineering, int economy)
        {
            UserSkills oldUserSkills = new() { Economy = 5, LandWarfare = 5, Engineering = 5, Research = 5, SpaceWarfare = 5 }; 
            UserSkills newUserSkills = new() { Economy = economy, LandWarfare = landWarfare, Engineering= engineering, Research = research, SpaceWarfare = spaceWarfare };
            Assert.True(CharacterTools.ValidSkills(oldUserSkills, newUserSkills));
        }

        [InlineData(5, 5, 15, 10, 5)]
        [InlineData(5, 10, 0, -123, 5)]
        [InlineData(5, 5, 10, 5, 25)]
        [Theory]
        public void AssertUserSkillsAreInvalidForLevelUp(int spaceWarfare, int landWarfare, int research, int engineering, int economy)
        {
            UserSkills oldUserSkills = new() { Economy = 5, LandWarfare = 5, Engineering = 5, Research = 5, SpaceWarfare = 5 };
            UserSkills newUserSkills = new() { Economy = economy, LandWarfare = landWarfare, Engineering = engineering, Research = research, SpaceWarfare = spaceWarfare };
            Assert.False(CharacterTools.ValidSkills(oldUserSkills, newUserSkills));
        }
    }

}
