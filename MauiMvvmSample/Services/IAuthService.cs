using MauiMvvmSample.Models;

namespace MauiMvvmSample.Services;

public interface IAuthService
{
    Task RegisterAsync(
        string username,
        string email,
        string phone,
        string password);

    Task<User?> LoginAsync(
        string email,
        string password);
}
