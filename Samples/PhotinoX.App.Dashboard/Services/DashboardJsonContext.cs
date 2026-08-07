using System.Text.Json.Serialization;

namespace PhotinoX.App.Dashboard;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(DashboardHostMessage))]
internal sealed partial class DashboardJsonContext : JsonSerializerContext;