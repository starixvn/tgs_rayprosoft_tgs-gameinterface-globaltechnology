using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundOutQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029",
			Currency = "CNY",
			OrderId = "E0900B54-F913-452D-9A5D-BC5711D80E09",
			Amount = 10.0m,
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new PaymentTransactionResp(
			TransId: "1810187480510443523",
			ExtTransId: "E0900B54-F913-452D-9A5D-BC5711D80E09",
			Balance: 10.0m,
			Resp: new JsonObject
			{
				["requestId"] = "4d5d38beae7242818e5841f00ddc4b69",
				["transId"] = "1810187480510443523",
				["extTransId"] = "E0900B54-F913-452D-9A5D-BC5711D80E09",
			}.ToJsonString());

		_ = fakeClient.PaymentPlayerDepositAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			currency: Arg.Is(query.Currency),
			amount: Arg.Is(query.Amount),
			extTransId: Arg.Is(query.OrderId),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var sut = new FundOutGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal("E0900B54-F913-452D-9A5D-BC5711D80E09", actual.OrderId);
		Assert.Equal("1810187480510443523", actual.ExternalOrderId);
		Assert.NotEmpty(resp.Resp);
	}
}
