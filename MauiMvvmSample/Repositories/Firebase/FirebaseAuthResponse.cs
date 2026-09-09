namespace MauiMvvmSample.Services;

public class FirebaseAuthResponse
{
    public string LocalId { get; set; } = string.Empty;

    public string IdToken { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;
}