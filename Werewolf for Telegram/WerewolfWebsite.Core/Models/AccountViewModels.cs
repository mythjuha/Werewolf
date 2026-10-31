using System.ComponentModel.DataAnnotations;

namespace WerewolfWebsite.Core.Models;

public sealed class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}

public sealed class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class ChangePasswordViewModel
{
    [Required, DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6), DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;

    [Compare(nameof(NewPassword)), DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public sealed class ManageAccountViewModel
{
    public bool HasPassword { get; init; }
    public string? PhoneNumber { get; init; }
    public bool TwoFactorEnabled { get; init; }
}
