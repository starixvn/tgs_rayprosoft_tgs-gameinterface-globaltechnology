using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PaymentPlayerCheckTransTest
{
	[Fact]
	public async Task Status_is_approved()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var extTransId = "36B92C00-23E4-4209-B71E-2F7CB7527591";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/checkTrans").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["extTransId"]!.GetValue<string>() == extTransId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "f6c27c081b0245918d58ee842cd5b04b",
				"error": "0",
				"message": "SUCCESS",
				"currency": "CNY",
				"transId": "1810189791626473474",
				"extTransId": "36B92C00-23E4-4209-B71E-2F7CB752759A",
				"status": "approved",
				"amount": 10,
				"balance": 0,
				"transType": "withdrawal",
				"transTime": "2024-07-08 05:50:52.687",
				"playerId": "4ucmqplxb029",
				"bonusBalance": 0
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.PaymentPlayerCheckTransAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			extTransId: extTransId,
			cancellationToken: default)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task Transaction_does_not_exist()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var extTransId = "36B92C00-23E4-4209-B71E-2F7CB7527591";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/checkTrans").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["extTransId"]!.GetValue<string>() == extTransId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "73d9306d20054776a9c6599c8f36b61b",
				"error": "T_03",
				"message": "Transaction does not exist"
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.PaymentPlayerCheckTransAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			extTransId: extTransId,
			cancellationToken: default)
			.ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task Status_is_declined()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var extTransId = "36B92C00-23E4-4209-B71E-2F7CB7527591";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/checkTrans").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["extTransId"]!.GetValue<string>() == extTransId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "f6c27c081b0245918d58ee842cd5b04b",
				"error": "0",
				"message": "SUCCESS",
				"currency": "CNY",
				"transId": "1810189791626473474",
				"extTransId": "36B92C00-23E4-4209-B71E-2F7CB752759A",
				"status": "declined",
				"amount": 10,
				"balance": 0,
				"transType": "withdrawal",
				"transTime": "2024-07-08 05:50:52.687",
				"playerId": "4ucmqplxb029",
				"bonusBalance": 0
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		var actual = await apiClient.PaymentPlayerCheckTransAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			extTransId: extTransId,
			cancellationToken: default)
			.ConfigureAwait(true);

		Assert.False(actual);
	}

	[Fact]
	public async Task Status_is_pending()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var extTransId = "36B92C00-23E4-4209-B71E-2F7CB7527591";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/checkTrans").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["extTransId"]!.GetValue<string>() == extTransId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "f6c27c081b0245918d58ee842cd5b04b",
				"error": "0",
				"message": "SUCCESS",
				"currency": "CNY",
				"transId": "1810189791626473474",
				"extTransId": "36B92C00-23E4-4209-B71E-2F7CB752759A",
				"status": "pending",
				"amount": 10,
				"balance": 0,
				"transType": "withdrawal",
				"transTime": "2024-07-08 05:50:52.687",
				"playerId": "4ucmqplxb029",
				"bonusBalance": 0
			}
			""")));
		var httpClient = handler.ToHttpClient();
		var fakerEncryptor = Substitute.For<IParameterEncryptor>();
		fakerEncryptor.Encrypt(
			values: Arg.Any<JsonObject>(),
			secretKey: Arg.Is(secretKey))
			.Returns(fakerHash);
		var fakerLogger = NullLogger<GameApiClient>.Instance;
		var apiClient = new GameApiClient(fakerLogger, httpClient, fakerEncryptor);

		_ = await Assert.ThrowsAsync<Exception>(() => apiClient.PaymentPlayerCheckTransAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			extTransId: extTransId,
			cancellationToken: default)
		.AsTask());
	}
}
