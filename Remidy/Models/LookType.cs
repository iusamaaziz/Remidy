namespace Remidy.Models
{
    public class LookType : ILookup
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public override string ToString() => $"{Title}";
    }
}
