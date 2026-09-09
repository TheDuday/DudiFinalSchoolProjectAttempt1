using System.Text.Json.Serialization;

public class FirestoreDocument
{
    [JsonPropertyName("fields")]
    public FirestoreFields? Fields { get; set; }
}

public class FirestoreFields
{
    [JsonPropertyName("username")]
    public FirestoreStringValue? Username { get; set; }

    [JsonPropertyName("email")]
    public FirestoreStringValue? Email { get; set; }

    [JsonPropertyName("phone")]
    public FirestoreStringValue? Phone { get; set; }
}

public class FirestoreStringValue
{
    [JsonPropertyName("stringValue")]
    public string? StringValue { get; set; }
}