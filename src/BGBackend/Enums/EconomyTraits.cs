using System.Collections.Generic;

namespace BrowserGameBackend.Enums
{
    public class EconomyTraits : IEnumeration
    {
        private readonly Dictionary<int, string> _enumeration = new()
        {
            { 0 ,"Researcher" },
            { 1 , "Expansionist"},
            { 2 , "Trader"},
            { 3 , "Manufacturer"},
            { 4 , "Fabricator"},
            { 5 , "Commander"},
        };
        public Dictionary<int, string> Enumeration => _enumeration;

        public Dictionary<string, int> ReverseEnumeration()
        {
            Dictionary < string, int> reverse = new ();
            foreach (var pair in _enumeration ) reverse.Add(pair.Value, pair.Key);
            return reverse;
        }

        public int Count()
        { 
            return _enumeration.Count;
        }

        public string FromKey(int key)
        {
            return _enumeration[key];
        }

        public int FromValue(string value) 
        {
            Dictionary<string, int> reverse = ReverseEnumeration();
            return reverse[value];
        }
        
        public bool IsValidValue(string value)
        {
            return _enumeration.ContainsValue(value);
        }
    }
    
}