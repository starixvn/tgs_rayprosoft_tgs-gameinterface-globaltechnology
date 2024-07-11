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
public class GameApiClient_HistoryRoundDetailTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var endTime = DateTime.UtcNow;
		var roundId = Guid.NewGuid().ToString();
		var providerCode = "pt";
		var language = "CN";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/roundDetail").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
			    "requestId": "requestId1234",
			    "error": "0",
			    "message": "SUCCESS",
			    "data": "https://detailurl.example"
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

		var actual = await apiClient.HistoryRoundDetailAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			providerCode: providerCode,
			roundId: roundId,
			endTime: endTime,
			language: language,
			cancellationToken: default);

		Assert.NotNull(actual);
		Assert.True(actual.IsSupport);
		Assert.NotEmpty(actual.DetailUrl);
	}

	[Fact]
	public async Task No_data()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var endTime = DateTime.UtcNow;
		var roundId = Guid.NewGuid().ToString();
		var providerCode = "pt";
		var language = "CN";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/roundDetail").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
			    "requestId": "request2868",
			    "error": "50001",
			    "message": "No data"
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

		var actual = await apiClient.HistoryRoundDetailAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			providerCode: providerCode,
			roundId: roundId,
			endTime: endTime,
			language: language,
			cancellationToken: default);

		Assert.NotNull(actual);
		Assert.False(actual.IsSupport);
		Assert.Empty(actual.DetailUrl);
	}

	[Fact]
	public async Task ErrorCodeException()
	{
		var apiUrl = new Uri("http://gm-stage-data.gmgiantgold.com/");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var endTime = DateTime.UtcNow;
		var roundId = Guid.NewGuid().ToString();
		var providerCode = "pt";
		var language = "CN";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/history/roundDetail").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
			   "requestId": "requestId1234",
			   "error": "P_02",
			   "message": "Invalid hash"
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

		_ = await Assert.ThrowsAsync<ErrorCodeException>(
			() => apiClient.HistoryRoundDetailAsync(
				apiBaseUrl: apiUrl,
				brandId: brandId,
				secretKey: secretKey,
				providerCode: providerCode,
				roundId: roundId,
				endTime: endTime,
				language: language,
				cancellationToken: default)
			.AsTask());
	}
}
