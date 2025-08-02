namespace Remidy.Models
{
    public class RespondedToTreatmentType : ILookup
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public override string ToString() => $"{Title}";
    }
}
