using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace BrowserGameBackend.Enums
{
    public enum Factions
    {
        //robots at bottom center
        [EnumMember(Value = "Natural Order")]
        Robots,
        //vega at bottom right
        [EnumMember(Value = "Vega Legion")]
        Green,
        //parasites at top right
        [EnumMember(Value = "Swarm")]
        Parasites,
        //solar at top
        [EnumMember(Value = "Solar Empire")]
        Red,
        //anarchists at top left
        [EnumMember(Value = "Pandemonium")]
        Anarchists,
        //azure at bottom left
        [EnumMember(Value = "Azure Nebula")]
        Blue
    }
}
