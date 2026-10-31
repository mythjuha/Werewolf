using System.Text.Json.Serialization;

namespace DonationSite.Core.Models;

public sealed class XsollaEvent
{
    [JsonPropertyName("notification_type")] public string? NotificationType { get; set; }
    public XsollaUser? User { get; set; }
    public XsollaPurchase? Purchase { get; set; }
    [JsonPropertyName("refund_details")] public XsollaRefundDetails? RefundDetails { get; set; }
}

public sealed class XsollaUser { public string? Id { get; set; } }
public sealed class XsollaPurchase
{
    [JsonPropertyName("virtual_currency")] public XsollaVirtualCurrency? VirtualCurrency { get; set; }
    public XsollaTotal? Total { get; set; }
}
public sealed class XsollaVirtualCurrency { public float Amount { get; set; } }
public sealed class XsollaTotal { public float Amount { get; set; } }
public sealed class XsollaRefundDetails { public string? Reason { get; set; } }
