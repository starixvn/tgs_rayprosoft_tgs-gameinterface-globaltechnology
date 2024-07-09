using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			PlayerCode = "4ucmqplxb029"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new PlayerBalanceResp(
			IsPlayerExist: true,
			PlayerId: "4ucmqplxb029",
			Currency: "CNY",
			Balance: 10.0m,
			BonusBalance: 1.0m,
			BrandId: 908);

		_ = fakeClient.PlayerBalanceAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			playerId: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var sut = new PlayerBalanceGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(resp.PlayerId, actual.PlayerCode);
		Assert.Equal(resp.Currency, actual.Currency);
		Assert.Equal(resp.Balance, actual.Balance);
		Assert.Equal(resp.BonusBalance, actual.BonusBalance);
		Assert.Equal(resp.BrandId, actual.BrandId);
	}
}
