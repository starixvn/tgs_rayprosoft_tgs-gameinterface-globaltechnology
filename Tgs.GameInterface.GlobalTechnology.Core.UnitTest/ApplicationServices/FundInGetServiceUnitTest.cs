using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundInQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029",
			Currency = "CNY",
			OrderId = "36B92C00-23E4-4209-B71E-2F7CB752759A",
			Amount = 10.0m,
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new PaymentTransactionResp(
			TransId: "1810189791626473474",
			ExtTransId: "36B92C00-23E4-4209-B71E-2F7CB752759A",
			Balance: 0.0m,
			Resp: new JsonObject
			{
				["requestId"] = "97d0d7dc196340469ee2df169225e56f",
				["transId"] = "1810189791626473474",
				["extTransId"] = "36B92C00-23E4-4209-B71E-2F7CB752759A",
			}.ToJsonString());

		_ = fakeClient.PaymentPlayerWithdrawalAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			amount: Arg.Is(query.Amount),
			extTransId: Arg.Is(query.OrderId),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var sut = new FundInGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal("36B92C00-23E4-4209-B71E-2F7CB752759A", actual.OrderId);
		Assert.Equal("1810189791626473474", actual.ExternalOrderId);
		Assert.NotEmpty(resp.Resp);
	}
}
