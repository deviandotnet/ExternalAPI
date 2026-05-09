namespace ExternalAPI.DTOs
{
    public class RecordResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RecordNumber { get; set; }

    }

    public class RecordRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int RecordNumber { get; set; }
    }
}
