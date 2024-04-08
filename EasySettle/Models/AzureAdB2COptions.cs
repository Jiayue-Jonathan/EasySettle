namespace EasySettle.Models;
public class AzureAdB2COptions
{
    public string? Instance { get; set; }
    public string? ClientId { get; set; }
    public string? Domain { get; set; }
    public string? SignedOutCallbackPath { get; set; }
    public string? SignUpSignInPolicyId { get; set; }
    public string? ResetPasswordPolicyId { get; set; }
    public string? EditProfilePolicyId { get; set; }
    public string? CallbackPath { get; set; }
}
