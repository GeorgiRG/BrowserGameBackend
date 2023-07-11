using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace BrowserGameBackend.Enums
{
    public class Factions : IEnumeration
    {
        private readonly Dictionary<int, string> _enumeration = new()
                    {
            { 0 ,"Natural Order" }, //Robots
            { 1 , "Vega Legion"}, //Vega
            { 2 , "Swarm"}, //Parasites
            { 3 , "Solar Empire"}, //Solar
            { 4 , "Pandemonium"}, //Anarchists
            { 5 , "Azure Nebula"}, //Azure
        };
        public Dictionary<int, string> Enumeration => _enumeration;
        private readonly Dictionary<int, string> CapitalsEnumeration = new()
        {
            { 0 ,"Machine Name" }, //Robots
            { 1 , "The Preserve"}, //Vega
            { 2 , "Nom Nom"}, //Parasites
            { 3 , "Earth Magna"}, //Solar
            { 4 , "Piraty name"}, //Anarchists
            { 5 , "Wallstreet"}, //Azure
        };

        public int Count()
        {
            return _enumeration.Count;
        }

        public string FactionCapital(int key)
        {
            return CapitalsEnumeration[key];
        }
        public Dictionary<string, int> ReverseEnumeration()
        {
            
            Dictionary<string, int> reverse = new();
            foreach (var pair in _enumeration) reverse.Add(pair.Value, pair.Key);
            return reverse;
        }
        public string FromKey(int key)
        {
            return _enumeration[key];
        }
        public int FromValue(string value)
        {
            return ReverseEnumeration()[value];
        }
        public bool IsValidValue(string value)
        {
            return _enumeration.ContainsValue(value);
        }
    }

}