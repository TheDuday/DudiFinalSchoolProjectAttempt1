using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;

namespace MauiMvvmSample.Repositories;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public async Task AddAsync(User user)
    {
        await Task.Delay(500); // סימולציית DB / רשת
        _users.Add(user);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        await Task.Delay(500);
        return _users.FirstOrDefault(u => u.Username == username);
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}
