using System.Text.Json;

namespace BuildAutomation.Core.Models;

public sealed class BuildWebhookPayload
{
    public string? SubscriptionId { get; set; }
    public JsonElement Resource { get; set; }
}
