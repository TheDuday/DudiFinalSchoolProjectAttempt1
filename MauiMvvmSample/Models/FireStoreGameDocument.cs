using System.Text.Json.Serialization;

public class FireStoreGameDocument
{
    [JsonPropertyName("fields")]
    public FirestoreGameFields? Fields { get; set; }
}

public class FirestoreGameFields
{
    [JsonPropertyName("gameString")]
    public FirestoreStringValue? GameString { get; set; }

    [JsonPropertyName("hostPlayerId")]
    public FirestoreStringValue? HostPlayerId { get; set; }

    [JsonPropertyName("nonHostPlayerId")]
    public FirestoreStringValue? NonHostPlayerId{ get; set; }

    [JsonPropertyName("gameStarted")]
    public FireStoreBooleanValue? GameStarted { get; set; }

    [JsonPropertyName("gameEnded")]
    public FireStoreBooleanValue? GameEnded { get; set; }
}