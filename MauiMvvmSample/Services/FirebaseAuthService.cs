using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Firebase;
using MauiMvvmSample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;

public class FirebaseAuthService : IAuthService
{
    private readonly HttpClient _http = new();

    private const string ApiKey = "AIzaSyCKdZ-T2ISGNoJoCqmWuY5Ud7XfEpIP4yw";
    private const string ProjectId = "chat-a80e2"; 

    public async Task RegisterAsync(string username, string email, string phone, string password)
    {
        var authResponse = await _http.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={ApiKey}",
            new
            {
                email,
                password,
                returnSecureToken = true
            });

        authResponse.EnsureSuccessStatusCode();

        var data = await authResponse.Content
            .ReadFromJsonAsync<FirebaseAuthResponse>();

        await SaveUserProfile(data.LocalId, username, email, phone);
    }

    public async Task<User?> LoginAsync(string username, string password)
    {
        var response = await _http.PostAsJsonAsync(
            $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={ApiKey}",
            new
            {
                email = username,
                password,
                returnSecureToken = true
            });

        if (!response.IsSuccessStatusCode)
            return null;

        var data = await response.Content
            .ReadFromJsonAsync<FirebaseAuthResponse>();

        return await LoadUserProfile(data.LocalId);
    }
    private async Task SaveUserProfile(string uid, string username, string email, string phone)
    {
        await _http.PatchAsJsonAsync(
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/users/{uid}",
            new
            {
                fields = new
                {
                    username = new { stringValue = username },
                    email = new { stringValue = email },
                    phone = new { stringValue = phone }
                }
            });
    }
    private async Task<User?> LoadUserProfile(string uid)
    {
        var response = await _http.GetAsync(
            $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/users/{uid}");

        if (!response.IsSuccessStatusCode)
            return null;

        //var json = await response.Content.ReadFromJsonAsync<dynamic>();
        var doc = await response.Content.ReadFromJsonAsync<FirestoreDocument>();


        return new User
        {
            Id = uid,
            Username = doc.fields.username.stringValue,
            Email = doc.fields.email.stringValue,
            Phone = doc.fields.phone.stringValue
        };
    }
}

public class FirestoreString
{
    public string stringValue { get; set; }
}

public class FirestoreFields
{
    public FirestoreString username { get; set; }
    public FirestoreString email { get; set; }
    public FirestoreString phone { get; set; }
}

public class FirestoreDocument
{
    public FirestoreFields fields { get; set; }
}






