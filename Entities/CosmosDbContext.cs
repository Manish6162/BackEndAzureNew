using Microsoft.Azure.Cosmos;

public class CosmosDbContext
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseName;

    public CosmosDbContext(CosmosClient cosmosClient, IConfiguration configuration)
    {
        _cosmosClient = cosmosClient;
        _databaseName = configuration["CosmosDbSettings:DatabaseName"];

        UsersContainer = _cosmosClient.GetContainer(_databaseName, configuration["CosmosDbSettings:UsersContainerName"]);
        FeedsContainer = _cosmosClient.GetContainer(_databaseName, configuration["CosmosDbSettings:FeedsContainerName"]);
        ChatsContainer = _cosmosClient.GetContainer(_databaseName, configuration["CosmosDbSettings:ChatsContainerName"]);
        NotificationsContainer = _cosmosClient.GetContainer(_databaseName, configuration["CosmosDbSettings:NotificationsContainerName"]);
    }

    public Container UsersContainer { get; }
    public Container FeedsContainer { get; }
    public Container ChatsContainer { get; }
    public Container NotificationsContainer { get; }
}
