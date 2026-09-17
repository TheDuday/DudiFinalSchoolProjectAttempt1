using SQLite;

namespace MauiMvvmSample.Models;

public class User
{
    [PrimaryKey, AutoIncrement]
    //public int Id { get; set; }          // SQLite
    // public string RemoteId { get; set; } // Firebase

    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    
}

