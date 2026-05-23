using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

public class MachineThreshold
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public Guid MachineId { get; set; }
    public Guid TenantId { get; set; }
    public string MachineName { get; set; } = string.Empty;
    public string MetricName { get; set; } = string.Empty;
    public double WarningThreshold { get; set; }
    public double CriticalThreshold { get; set; }
    public string Unit { get; set; } = string.Empty;
}