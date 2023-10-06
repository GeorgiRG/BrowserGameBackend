namespace BrowserGameBackend.Models
{
    public class ResourceRequirements
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public Resource TargetResource { get; set; } = null!;
        public int RequiredResource1Id { get; set; }
        public int RequiredResource1IdAmount { get; set; } = 0;
        public int RequiredResource2Id { get; set; }
        public int Res2Req { get; set; } = 0;
        public int RequiredResource3Id { get; set; }
        public int Res3Req { get; set; } = 0;

    }
}
