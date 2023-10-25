namespace BrowserGameBackend.Enums
{
    public class SpeciesEnum : IEnumeration
    {
        private readonly Dictionary<int, string> _enumeration = new()
        {
            { 0 , "Humans" },
            { 1 , "Aquatics"}, 
            { 2 , "Insects"}, 
            { 3 , "Liths"}, 
            { 4 , "Robots"}, 
            { 5 , "Parasites"},
        };
        public Dictionary<int, string> Enumeration => _enumeration;
      
        public string Humans { get; } = "Humans";
        public string Aquatics { get; } = "Aquatics";
        public string Insects { get; } = "Insects";
        public string Liths { get; } = "Liths";
        public string Robots { get; } = "Robots";
        public string Parasites { get; } = "Parasites";

        public int Count()
        {
            return _enumeration.Count;
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
