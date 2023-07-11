namespace BrowserGameBackend.Enums
{
    public class FightingTraits : IEnumeration
    {
        private readonly Dictionary<int, string> _enumeration = new()
        {
            { 0 ,"Peaceful" },
            { 1 , "Relentless"},
            { 2 , "Careful"},
            { 3 , "Vengeful"},
            { 4 , "Opportunistic"},
        };
        public Dictionary<int, string> Enumeration => _enumeration;

        public Dictionary<string, int> ReverseEnumeration()
        {
            Dictionary<string, int> reverse = new();
            foreach (var pair in _enumeration) reverse.Add(pair.Value, pair.Key);
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
            return ReverseEnumeration()[value];
        }

        public bool IsValidValue(string value)
        {
            return _enumeration.ContainsValue(value);
        }
    }
}