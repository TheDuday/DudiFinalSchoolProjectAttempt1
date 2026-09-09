using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MauiMvvmSample.Repositories;

public class FirebaseUserRepository : IUserRepository
{
    private readonly HttpClient _http = new();

    private const string ProjectId = "finalschoolprojectattempt1fb";

    public async Task AddAsync(User user)
    {
        var url =
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
            $"/databases/(default)/documents/users/{user.Id}";

        var document = new
        {
            fields = new
            {
                username = new { stringValue = user.Username },
                email = new { stringValue = user.Email },
                phone = new { stringValue = user.Phone }
            }
        };

        var response = await _http.PatchAsJsonAsync(url, document);

        response.EnsureSuccessStatusCode();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var url =
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}" +
            $"/databases/(default)/documents/users/{id}";

        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        var document =
            await response.Content.ReadFromJsonAsync<FirestoreDocument>();

        if (document?.Fields == null)
            return null;

        return new User
        {
            Id = id,
            Username = document.Fields.Username?.StringValue ?? "",
            Email = document.Fields.Email?.StringValue ?? "",
            Phone = document.Fields.Phone?.StringValue ?? ""
        };
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}