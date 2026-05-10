using ExternalAPI.DTOs;

namespace ExternalAPI.Helpers
{
    public class RecordCreatedEvent
    {
        public Guid CorrelationId { get; set; }
        //public int Version { get; set; } = 1;

        public RecordResponse Data { get; set; } = default!;
    }
}
