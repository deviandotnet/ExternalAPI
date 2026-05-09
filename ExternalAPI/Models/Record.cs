namespace ExternalAPI.Models
{
    public class Record
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RecordNumber { get; set; }
    }
}
