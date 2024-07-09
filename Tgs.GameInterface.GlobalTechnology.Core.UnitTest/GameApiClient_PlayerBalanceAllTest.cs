using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PlayerBalanceAllTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var page = 1;
		var size = 5000;
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/player/balance/all").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["page"]!.GetValue<int>() == page &&
				content["size"]!.GetValue<int>() == size)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"error": "0",
				"message": "SUCCESS",
				"total": 2,
				"current": 1,
				"size": 5000,
				"pages": 1,
				"records": [
					{
						"brandId": 908,
						"playerId": "4ucmqplxb028",
						"currency": "IDR",
						"balance": 0,
						"bonusBalance": 0
					},
					{
						"brandId": 908,
						"playerId": "4ucmqplxb029",
						"currency": "CNY",
						"balance": 0,
						"bonusBalance": 0
					}
				]
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

		var actual = await apiClient.PlayerBalanceAllAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			page: page,
			size: size,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var brandBalance = actual.Where(x => x.PlayerId.Equals("4ucmqplxb029", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		Assert.NotNull(brandBalance);
		Assert.Equal(908, brandBalance.BrandId);
		Assert.Equal(0.0m, brandBalance.Balance);
	}
}
