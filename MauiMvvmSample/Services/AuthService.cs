using System.Security.Cryptography;
using System.Text;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;

namespace MauiMvvmSample.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;

    public AuthService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task RegisterAsync(string username, string email, string phone, string password)
    {
        var user = new User
        {
            Username = username,
            Email = email,
            Phone = phone,
            PasswordHash = Hash(password)
        };

        await _repo.AddAsync(user);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return null;

        return Hash(password) == user.PasswordHash ? user : null;
    }

    private string Hash(string input)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
    public async Task UpdateProfileAsync(
    User user,
    string email,
    string phone)
    {
        user.Email = email;
        user.Phone = phone;

        await _repo.UpdateAsync(user);
    }
}

