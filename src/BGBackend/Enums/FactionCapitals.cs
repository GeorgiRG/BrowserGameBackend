using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace BrowserGameBackend.Enums
{
    public enum FactionCapitals
    {
        [EnumMember(Value = "Machine Name")]
        Robots,
        [EnumMember(Value = "The Preserve")]
        Green,
        [EnumMember(Value = "Nom Nom")]
        Parasites,
        [EnumMember(Value = "Earth Magna")]
        Red,
        [EnumMember(Value = "Piraty name")]
        Anarchists,
        [EnumMember(Value = "Wallstreet")]
        Blue
    }
}
