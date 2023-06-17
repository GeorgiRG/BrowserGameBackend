namespace BrowserGameBackend.Models
{
    /// <summary>
    /// Bot Player 
    ///<br> FightingTrait - decides attacking and decisions in combatbr</br>
    ///<br> EconomyTrait - decides economy</br>
    ///<br> PowerLevel - controls what "fleet and army" has the AI stockpiled, influenced by economy and age</br>
    ///<br> AvailableResources - res from available planets</br>
    ///<br> FleetTemplate - decided fleet by template and power level instead of doing anything too complex</br>
    ///<br> PlanetsAmout - P    lanets that the bot can have </br>
    ///<br> AttackCooldown - days between attacks</br>
    ///<br> PlayerIdPriorityTop - who will be the target to be attacked</br>
    ///<br> Age - age in days , years in game</br>
    /// </summary>
    public class Bot
    {
     
        public int Id { get; set; }
        public string Name { get; set; }
        public string FightingTrait { get; set; } 
        public string EconomyTrait { get; set; } 
        public int PowerLevel { get; set; }
        public int AvailableResources { get; set; } 
        public string Faction { get; set; }
        public int FleetTemplate { get; set; } 
        public int PlanetsAmount { get; set; }
        public int AttackCooldown { get; set; } = 0; 
        public int PlayerIdPriorityTop { get; set; } = 0; 
        public int PlayerIdPrioritySecondary { get; set; } = 0; 
        public DateTime LastChecked { get; set; }
        public int Age { get; set; }
    }
}
