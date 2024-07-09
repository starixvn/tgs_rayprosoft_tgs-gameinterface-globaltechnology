using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PlayerGetTokenTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerId = "4ucmqplxb029";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/getToken").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "f99a9a645af4470990a22a6c8c7109d8",
				"error": "0",
				"message": "SUCCESS",
				"token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJicmFuZElkIjo5MDgsImV4cCI6MTcyMDE4MTk2NCwicGxheWVySWQiOiI0dWNtcXBseGIwMjkifQ.ltXkd98X1twzJVYXGRAOLZkjjkgfZxp_3tW46HbLEuY"
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

		var actual = await apiClient.PlayerGetTokenAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			cancellationToken: default)
			.ConfigureAwait(true);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJicmFuZElkIjo5MDgsImV4cCI6MTcyMDE4MTk2NCwicGxheWVySWQiOiI0dWNtcXBseGIwMjkifQ.ltXkd98X1twzJVYXGRAOLZkjjkgfZxp_3tW46HbLEuY", actual.Token);
	}

	[Fact]
	public async Task Player_not_found()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerId = "4ucmqplxb029";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/getToken").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "4ucmqplxb029",
				"error": "P_04",
				"message": "Player not found"
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

		var actual = await apiClient.PlayerGetTokenAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			cancellationToken: default)
			.ConfigureAwait(true);
		Assert.False(actual.IsPlayerExist);
		Assert.Empty(actual.Token);
	}
}
