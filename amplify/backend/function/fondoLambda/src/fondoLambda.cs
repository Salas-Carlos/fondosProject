using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Services;
using Infrastructure.Repositories;
using Domain.Models;
using Domain.DTO;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace fondoLambda
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class fondoLambda
    {
      private readonly ClientService _clientService;
    private readonly FondoService _fondoService;
    private readonly TransactionService _transactionService;

      public fondoLambda()
        {
          string region = Environment.GetEnvironmentVariable("REGION");
          string tableName = "fondos-dev-dev";


          var dynamoDbClient = new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region));

          var clientRepository = new DynamoDbRepository<Client>(dynamoDbClient, tableName);
          var fondoRepository = new DynamoDbRepository<Fondo>(dynamoDbClient, tableName);
          var transactionRepository = new DynamoDbRepository<Transaction>(dynamoDbClient, tableName);

          _clientService = new ClientService(clientRepository);
          _fondoService = new FondoService(fondoRepository);
          _transactionService = new TransactionService(transactionRepository, _clientService);
        }
        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        /// <remarks>
        /// If you rename this function, you will need to update the invocation shim
        /// to match if you intend to test the function with 'amplify mock function'
        /// </remarks>
#pragma warning disable CS1998
        public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var response = new APIGatewayProxyResponse {
                Headers = new Dictionary<string, string> {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept" }
                }
            };

            string contentType = null;
            request.Headers?.TryGetValue("Content-Type", out contentType);
            switch (request.HttpMethod) {
                case "GET":
                if (request.Path.StartsWith("/clients"))
                {
                    var pathParts = request.Path.Split('/');
                    var id = pathParts.Length > 2 ? pathParts[2] : null;


                    if (id != null)
                    {
                        var client = await _clientService.GetClientAsync(id);
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(client);
                    }
                    else
                    {
                        var clients = await _clientService.GetAllClientsAsync();
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(clients);
                    }
                }
                else if (request.Path.StartsWith("/fondos"))
                {
                    var pathParts = request.Path.Split('/');
                    var id = pathParts.Length > 2 ? pathParts[2] : null;


                    if (id != null)
                    {
                        var fondo = await _fondoService.GetFondoAsync(id);
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(fondo);
                    }
                    else
                    {
                        var fondos = await _fondoService.GetAllFondosAsync();
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(fondos);
                    }
                }
                else if (request.Path.StartsWith("/transactions"))
                {
                    var pathParts = request.Path.Split('/');
                    var id = pathParts.Length > 2 ? pathParts[2] : null;


                    if (id != null)
                    {
                        var transaction = await _transactionService.GetTransactionAsync(id);
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(transaction);
                    }
                    else
                    {
                        var transactions = await _transactionService.GetAllTransactionsAsync();
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.Body = JsonConvert.SerializeObject(transactions);
                    }
                }
                break;
                case "POST":
                if (request.Path == "/clients")
                {
                    var client = JsonConvert.DeserializeObject<Client>(request.Body);
                    await _clientService.AddClientAsync(client);
                    response.StatusCode = (int)HttpStatusCode.Created;


                }
                else if (request.Path == "/fondos")
                {
                    var fund = JsonConvert.DeserializeObject<Fondo>(request.Body);
                    await _fondoService.AddFondoAsync(fund);
                    response.StatusCode = (int)HttpStatusCode.Created;
                }
                else if (request.Path == "/transactions")
                {
                    var transaction = JsonConvert.DeserializeObject<TransactionDTO>(request.Body);
                    var result =  await _transactionService.AddTransactionAsync(transaction);
                    response.StatusCode = (int)HttpStatusCode.Created;
                    response.Body = JsonConvert.SerializeObject(result);
                }
                break;

            case "PUT":
                if (request.Path.StartsWith("/clients"))
                {
                    var client = JsonConvert.DeserializeObject<Client>(request.Body);
                    await _clientService.UpdateClientAsync(client);
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else if (request.Path.StartsWith("/fondos"))
                {
                    var fund = JsonConvert.DeserializeObject<Fondo>(request.Body);
                    await _fondoService.UpdateFondoAsync(fund);
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else if (request.Path.StartsWith("/transactions"))
                {
                    var transaction = JsonConvert.DeserializeObject<Transaction>(request.Body);
                    await _transactionService.UpdateTransactionAsync(transaction);
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                break;

            case "DELETE":
                if (request.Path.StartsWith("/clients"))
                {
                    var pathParts = request.Path.Split('/');
                    var pk = pathParts.Length > 2 ? pathParts[2] : null;
                    var sk = pathParts.Length > 3 ? pathParts[3] : null;

                    if (pk != null && sk != null)
                    {
                        await _clientService.DeleteClientAsync(pk, sk);
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                }
                else if (request.Path.StartsWith("/fondos"))
                {
                    var pathParts = request.Path.Split('/');
                    var pk = pathParts.Length > 2 ? pathParts[2] : null;
                    var sk = pathParts.Length > 3 ? pathParts[3] : null;

                    if (pk != null && sk != null)
                    {
                        await _fondoService.DeleteFondoAsync(pk, sk);
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                }
                else if (request.Path.StartsWith("/transactions"))
                {
                    var pathParts = request.Path.Split('/');
                    var pk = pathParts.Length > 2 ? pathParts[2] : null;
                    var sk = pathParts.Length > 3 ? pathParts[3] : null;

                    if (pk != null && sk != null)
                    {
                        await _transactionService.DeleteTransactionAsync(pk, sk);
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                }
                break;


                default:
                    context.Logger.LogLine($"Unrecognized verb {request.HttpMethod}\n");
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
            }

            return response;
        }
    }



}
