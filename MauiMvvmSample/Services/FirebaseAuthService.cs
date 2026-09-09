using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories;
using MauiMvvmSample.Repositories.Interfaces;
using System.Net.Http.Json;

namespace MauiMvvmSample.Services;

public class FirebaseAuthService : IAuthService
{
    private readonly HttpClient _http = new();
    private readonly IUserRepository _userRepository;

    private const string ApiKey =
       "AIzaSyCqflpIy3cmIdXXgFbuA3ii4ucZOZa_9JM";

    public FirebaseAuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // -----------------------------
    // REGISTER
    // -----------------------------
    public async Task RegisterAsync(
        string username,
        string email,
        string phone,
        string password)
    {
        var response = await _http.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}",
            new
            {
                email,
                password,
                returnSecureToken = true
            });

        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            await Shell.Current.DisplayAlert("Error", "registeration failed, try different fields", "OK");
            return;
        }

        var data =
            await response.Content
                .ReadFromJsonAsync<FirebaseAuthResponse>();

        if (data == null)
            throw new Exception("Invalid Firebase response.");

        // המשתמש ב-Authentication
        // פרופיל המשתמש ב-Firestore
        var user = new User
        {
            Id = data.LocalId,
            Username = username,
            Email = email,
            Phone = phone
        };

        await _userRepository.AddAsync(user);
    }


    // -----------------------------
    // LOGIN
    // -----------------------------
    public async Task<User?> LoginAsync(
        string email,
        string password)
    {
        var response = await _http.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}",
            new
            {
                email,
                password,
                returnSecureToken = true
            });

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var data =
            await response.Content
                .ReadFromJsonAsync<FirebaseAuthResponse>();

        if (data == null)
            return null;

        // Firebase Authentication נתן לנו UID
        // עם ה-UID נביא את פרופיל המשתמש מ-Firestore
        return await _userRepository.GetByIdAsync(data.LocalId);
    }
}




