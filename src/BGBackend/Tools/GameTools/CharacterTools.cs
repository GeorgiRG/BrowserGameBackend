using BrowserGameBackend.Models;

namespace BrowserGameBackend.Tools.GameTools
{
    public static class CharacterTools
    {
        const int SkillPoints = 10; //skills points on level up
        public static UserSkills? LevelUp(UserSkills oldUserSkills, UserSkills userSkills)
        {
            if(ValidSkills(oldUserSkills, userSkills))
            {
                //assigning on old skills so there is no need for more validation
                oldUserSkills.Level += 1;
                oldUserSkills.Experience = 0;
                oldUserSkills.SpaceWarfare = userSkills.SpaceWarfare;
                oldUserSkills.LandWarfare = userSkills.LandWarfare;
                oldUserSkills.Research = userSkills.Research;
                oldUserSkills.Engineering = userSkills.Engineering;
                oldUserSkills.Economy = userSkills.Economy;
                return oldUserSkills;
            }
            else return null;
        }

        public static bool ValidSkills(UserSkills oldUserSkills, UserSkills userSkills)
        {
            int spaceWarfare = userSkills.SpaceWarfare - oldUserSkills.SpaceWarfare;
            int landWarfare = userSkills.LandWarfare - oldUserSkills.LandWarfare;
            int research = userSkills.Research - oldUserSkills.Research;
            int engineering = userSkills.Engineering - oldUserSkills.Engineering;
            int economy = userSkills.Economy - oldUserSkills.Economy;

            bool validRange = (0 <= spaceWarfare && spaceWarfare <= SkillPoints)
                            && (0 <= landWarfare && landWarfare <= SkillPoints)
                            && (0 <= research && research <= SkillPoints)
                            && (0 <= engineering && engineering <= SkillPoints)
                            && (0 <= economy && economy <= SkillPoints);
            return validRange && (spaceWarfare + landWarfare + research + engineering + economy == SkillPoints);
        }
        
    }
}
