namespace BrowserGameBackend.Enums
{
    public interface IEnumeration
    {
        Dictionary<int, string> Enumeration { get; }
        public Dictionary<string, int> ReverseEnumeration();
        public string FromKey(int key);
        public int FromValue(string value);

        public int Count();
        public bool IsValidValue(string value);
    }
}
