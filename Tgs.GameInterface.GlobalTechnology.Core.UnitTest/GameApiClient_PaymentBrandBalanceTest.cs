using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PaymentBrandBalanceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/brand/balance").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "dabdffbdd5394cd99261c00038cdb450",
				"error": "0",
				"message": "SUCCESS",
				"data": [
					{
						"updatedAt": "2024-07-04 03:33:59.745",
						"brandId": 908,
						"currency": "IDR",
						"balance": 1000000,
						"lastTransId": "1808705792580202498"
					},
					{
						"updatedAt": "2024-07-04 03:40:56.580",
						"brandId": 908,
						"currency": "JPY",
						"balance": 1000000,
						"lastTransId": "1808707540904521731"
					},
					{
						"updatedAt": "2024-07-04 03:36:38.607",
						"brandId": 908,
						"currency": "VND",
						"balance": 1000000,
						"lastTransId": "1808706458895724546"
					},
					{
						"updatedAt": "2024-07-08 05:50:52.681",
						"brandId": 908,
						"currency": "CNY",
						"balance": 1000000,
						"lastTransId": "1810189791626473473"
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

		var actual = await apiClient.PaymentBrandBalanceAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			cancellationToken: default)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var brandBalance = actual.Where(x => x.Currency.Equals("CNY", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		Assert.NotNull(brandBalance);
		Assert.Equal(908, brandBalance.BrandId);
		Assert.Equal(1000000.0m, brandBalance.Balance);
	}
}
