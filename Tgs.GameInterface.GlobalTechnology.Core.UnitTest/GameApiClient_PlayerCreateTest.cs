using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PlayerCreateTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerId = "4ucmqplxb029";
		var currency = "CNY";
		var language = "CH";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/create").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId &&
				content["currency"]!.GetValue<string>() == currency &&
				content["language"]!.GetValue<string>() == language)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"error": "0",
				"message": "SUCCESS",
				"brandId": 908,
				"requestId": "1288948353c84b99b87c144a762d6b98",
				"playerId": "4ucmqplxb029",
				"currency": "IDR",
				"language": "CH",
				"balance": 0,
				"bonusBalance": 0,
				"createdAt": "2024-07-05 09:22:00.140"
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

		await apiClient.PlayerCreateAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			currency: currency,
			language: language,
			cancellationToken: default)
			.ConfigureAwait(true);
	}

	[Fact]
	public async Task Player_exist()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerId = "4ucmqplxb029";
		var currency = "CNY";
		var language = "CH";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/create").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId &&
				content["currency"]!.GetValue<string>() == currency &&
				content["language"]!.GetValue<string>() == language)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "840bb93ddc414168b47269b8e5d95857",
				"error": "B_01",
				"message": "Player exist"
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

		await apiClient.PlayerCreateAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			currency: currency,
			language: language,
			cancellationToken: default)
			.ConfigureAwait(true);
	}
}
