using System.Net;

using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest;

public class GameApiClient_PaymentPlayerWithdrawalTest
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
		var extTransId = "36B92C00-23E4-4209-B71E-2F7CB752759A";
		var fakerHash = Guid.NewGuid().ToString("N");
		var handler = new MockHttpMessageHandler();
		handler.Expect(HttpMethod.Post, new Uri(apiUrl, "/payment/player/withdrawal").ToString())
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
				"requestId": "97d0d7dc196340469ee2df169225e56f",
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
				"bonusBalance": 0,
				"brandBalance": 1000000
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

		var actual = await apiClient.PaymentPlayerWithdrawalAsync(
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
		Assert.Equal("1810189791626473474", actual.TransId);
		Assert.NotEmpty(actual.Resp);
	}
}
