using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;


namespace Infrastructure.Repositories
{
public class DynamoDbRepository<T> where T : class
{
    private readonly Table _table;

    public DynamoDbRepository(IAmazonDynamoDB dynamoDbClient, string tableName)
    {
        _table = Table.LoadTable(dynamoDbClient, tableName);
    }

    public async Task<T> GetAsync(string pk, string sk)
    {
        var document = await _table.GetItemAsync(pk, sk);
        return document != null ? JsonConvert.DeserializeObject<T>(document.ToJson()) : null;
    }

    public async Task<IEnumerable<T>> GetAllAsync(string partitionKey, string sortKeyPrefix)
    {

       var query = new QueryOperationConfig
        {
            KeyExpression = new Expression
            {
                ExpressionStatement = "PK = :pk and begins_with(SK, :skPrefix)",
                ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    { ":pk", partitionKey },
                    { ":skPrefix", sortKeyPrefix }
                }
            }
        };
        var items = await _table.Query(query).GetRemainingAsync();
        return JsonConvert.DeserializeObject<IEnumerable<T>>(SerializeDocuments(items));

    }

    public async Task AddAsync(T item)
    {
        var document = Document.FromJson(JsonConvert.SerializeObject(item));
        await _table.PutItemAsync(document);
    }

    public async Task UpdateAsync(T item)
    {
        var document = Document.FromJson(JsonConvert.SerializeObject(item));
        await _table.UpdateItemAsync(document);
    }

    public async Task DeleteAsync(string pk, string sk)
    {
        await _table.DeleteItemAsync(pk, sk);
    }

     private string SerializeDocuments(List<Document> documents) {
            var buffer = new System.Text.StringBuilder();
            var writer = new ThirdParty.Json.LitJson.JsonWriter(buffer);
            writer.WriteArrayStart();
            if (documents != null)
            {
                documents.ForEach(document => writer.WriteRaw(document.ToJson()));
            }
            writer.WriteArrayEnd();
            return buffer.ToString();
        }
}
}


