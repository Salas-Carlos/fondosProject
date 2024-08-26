using Amazon.DynamoDBv2;

using Amazon.DynamoDBv2.DocumentModel;

namespace Infrastructure.Database
{
    public class DynamoDbContext
    {
        public IAmazonDynamoDB Client { get; }

        public DynamoDbContext(IAmazonDynamoDB client)
        {
            Client = client;
        }

        // Aquí puedes agregar otros métodos para manejar operaciones específicas de DynamoDB si es necesario
    }
}
