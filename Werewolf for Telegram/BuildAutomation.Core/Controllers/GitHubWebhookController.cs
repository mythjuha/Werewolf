using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BuildAutomation.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuildAutomation.Core.Controllers;

[ApiController]
[Route("api/webhooks/incoming/github")]
public sealed class GitHubWebhookController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<GitHubWebhookController> _logger;

    public GitHubWebhookController(
        IConfiguration configuration,
        IWebHostEnvironment environment,
        ILogger<GitHubWebhookController> logger)
    {
        _configuration = configuration;
        _environment = environment;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CancellationToken cancellationToken)
    {
        using var buffer = new MemoryStream();
        await Request.Body.CopyToAsync(buffer, cancellationToken);
        var bodyBytes = buffer.ToArray();

        var secret = _configuration["BuildAutomation:GitHubWebhookSecret"];
        if (!string.IsNullOrWhiteSpace(secret) && !IsValidSignature(bodyBytes, secret))
        {
            _logger.LogWarning("Rejected GitHub webhook with an invalid signature.");
            return Unauthorized();
        }

        PushEvent? push;
        try
        {
            push = JsonSerializer.Deserialize<PushEvent>(bodyBytes, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException exception)
        {
            _logger.LogWarning(exception, "Rejected malformed GitHub webhook payload.");
            return BadRequest("Invalid GitHub webhook JSON.");
        }

        if (push is null)
        {
            return BadRequest("The GitHub webhook payload was empty.");
        }

        var appDataPath = Path.Combine(_environment.ContentRootPath, "App_Data");
        Directory.CreateDirectory(appDataPath);
        var auditPath = Path.Combine(appDataPath, "last.json");
        await System.IO.File.WriteAllBytesAsync(auditPath, bodyBytes, cancellationToken);

        if (push.commits is not null)
        {
            _logger.LogInformation("Received GitHub push with {CommitCount} commits for {Branch}.",
                push.commits.Length, push._ref);
        }

        return Ok();
    }

    private bool IsValidSignature(byte[] payload, string secret)
    {
        var signature = Request.Headers["X-Hub-Signature-256"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(signature) || !signature.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var expected = "sha256=" + Convert.ToHexString(hmac.ComputeHash(payload)).ToLowerInvariant();
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signature));
    }
}
