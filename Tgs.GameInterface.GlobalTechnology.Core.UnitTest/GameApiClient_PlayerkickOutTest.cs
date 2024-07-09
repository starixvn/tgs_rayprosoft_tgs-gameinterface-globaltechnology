using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;
public class GameApiClient_PlayerkickOutTest
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
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/kickOut").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "b9192e878c43477e9b4b9bb4434e951c",
				"error": "0",
				"message": "SUCCESS"
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

		await apiClient.PlayerkickOutAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			cancellationToken: default)
			.ConfigureAwait(true);
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
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/kickOut").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "1fcac4419dec4bffbcd93b6aad22fd0a",
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

		await apiClient.PlayerkickOutAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			cancellationToken: default)
			.ConfigureAwait(true);
	}
}
