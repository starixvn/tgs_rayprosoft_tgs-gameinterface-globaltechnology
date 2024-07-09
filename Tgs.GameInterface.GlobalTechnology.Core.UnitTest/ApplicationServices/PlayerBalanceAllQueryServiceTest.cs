using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class PlayerBalanceAllQueryServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceAllQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			Page = 1,
			Size = 5000
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new PlayerBalanceAllResp(
			PlayerId: "4ucmqplxb029",
			Currency: "CNY",
			Balance: 10.0m,
			BonusBalance: 1.0m,
			BrandId: 908);

		_ = fakeClient.PlayerBalanceAllAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			page: Arg.Is(query.Page),
			size: Arg.Is(query.Size),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new PlayerBalanceAllQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var brandBalance = actual.FirstOrDefault();
		Assert.NotNull(brandBalance);
		Assert.Equal(resp.PlayerId, brandBalance.PlayerCode);
		Assert.Equal(resp.Currency, brandBalance.Currency);
		Assert.Equal(resp.Balance, brandBalance.Balance);
		Assert.Equal(resp.BonusBalance, brandBalance.BonusBalance);
		Assert.Equal(resp.BrandId, brandBalance.BrandId);
	}
}
