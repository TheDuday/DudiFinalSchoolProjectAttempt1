using SQLite;
using MauiMvvmSample.Models;
using MauiMvvmSample.Repositories.Interfaces;

namespace MauiMvvmSample.Repositories.Sqlite;

public class SqliteUserRepository : IUserRepository
{
    private readonly SQLiteAsyncConnection _db;

    public SqliteUserRepository(string dbPath)
    {
        _db = new SQLiteAsyncConnection(dbPath);
        _db.CreateTableAsync<User>().Wait();
    }

    public Task AddAsync(User user)
        => _db.InsertAsync(user);

    public Task<User?> GetByUsernameAsync(string username)
        => _db.Table<User?>()
              .Where(u => u.Username == username)
              .FirstOrDefaultAsync();

    public Task UpdateAsync(User user)
        => _db.UpdateAsync(user);
}

