using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class FundBrandBalanceQueryServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundBrandBalanceQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new BrandBalanceResp(
			BrandId: 908,
			Currency: "CNY",
			Balance: 1000000.0m,
			LastTransId: Guid.NewGuid().ToString(),
			UpdateAt: DateTimeOffset.UtcNow);

		_ = fakeClient.PaymentBrandBalanceAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { resp }.ToAsyncEnumerable());

		var sut = new FundBrandBalanceQueryService(
			fakeClient);

		var actual = await sut.QueryAsync(query)
			.ToArrayAsync();

		Assert.NotNull(actual);
		var brandBalance = actual.FirstOrDefault();
		Assert.NotNull(brandBalance);
		Assert.Equal(resp.BrandId, brandBalance.BrandId);
		Assert.Equal(resp.Balance, brandBalance.Balance);
	}
}
