using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PaymentPlayerDepositTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://api-stg.gmgiantgold.com");
		var brandId = 908;
		var secretKey = "SMT1ZCVRI3uqtOXNV93q";
		var playerId = "4ucmqplxb029";
		var currency = "CNY";
		var amount = 10.0m;
		var extTransId = "E0900B54-F913-452D-9A5D-BC5711D80E09";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/deposit").ToString())
			.WithExactQueryString(new Dictionary<string, string>
			{
				["hash"] = fakerHash,
			})
			.WithJsonContent<JsonObject>(content =>
				content["brandId"]!.GetValue<int>() == brandId &&
				content["playerId"]!.GetValue<string>() == playerId &&
				content["currency"]!.GetValue<string>() == currency &&
				content["amount"]!.GetValue<decimal>() == amount &&
				content["extTransId"]!.GetValue<string>() == extTransId)
			.Respond(HttpStatusCode.OK, JsonContent.Create(JsonNode.Parse("""
			{
				"requestId": "4d5d38beae7242818e5841f00ddc4b69",
				"error": "0",
				"message": "SUCCESS",
				"currency": "CNY",
				"transId": "1810187480510443523",
				"extTransId": "E0900B54-F913-452D-9A5D-BC5711D80E09",
				"status": "approved",
				"amount": 10,
				"balance": 10,
				"transType": "deposit",
				"transTime": "2024-07-08 05:41:41.679",
				"playerId": "4ucmqplxb029",
				"bonusBalance": 0,
				"brandBalance": 999990
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

		var actual = await apiClient.PaymentPlayerDepositAsync(
			apiBaseUrl: apiUrl,
			brandId: brandId,
			secretKey: secretKey,
			playerId: playerId,
			currency: currency,
			amount: amount,
			extTransId: extTransId,
			cancellationToken: default)
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal(extTransId, actual.ExtTransId);
		Assert.Equal("1810187480510443523", actual.TransId);
		Assert.NotEmpty(actual.Resp);
	}
}
