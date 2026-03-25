using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LabTrack.Core.Models
{
    public class SensorLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public int LabRunId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string SensorName { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}