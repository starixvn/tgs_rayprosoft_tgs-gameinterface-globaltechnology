using NSubstitute;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Core.Models;

namespace Tgs.GameInterface.GlobalTechnology.Core.UnitTest.ApplicationServices;

public class GameRecordDetailGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameRecordDetailQuery
		{
			ApiUrl = new Uri("https://api-stg.gmgiantgold.com"),
			BrandId = 908,
			SecretKey = "SMT1ZCVRI3uqtOXNV93q",
			ExternalProviderCode = "py",
			ExternalBetId = "gpas_vA0nc4ZybDj8eXLBVdKf",
			ExternalEndTime = DateTimeOffset.Parse("2024-07-10T17:18:05+08:00"),
			Language = "CN"
		};

		var fakeClient = Substitute.For<IGameApiClient>();

		var resp = new HistoryRoundDetailResp(
			IsSupport: true,
			DetailUrl: "https://extstg3-cashier01.ptstaging.eu");

		_ = fakeClient.HistoryRoundDetailAsync(
			apiBaseUrl: Arg.Is(query.ApiUrl),
			brandId: Arg.Is(query.BrandId),
			secretKey: Arg.Is(query.SecretKey),
			providerCode: Arg.Is(query.ExternalProviderCode),
			roundId: Arg.Is(query.ExternalBetId),
			endTime: Arg.Is(query.ExternalEndTime),
			language: Arg.Is(query.Language),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var sut = new GameRecordDetailGetService(
			fakeClient);

		var actual = await sut.GetAsync(query);

		Assert.NotNull(actual);
		Assert.True(actual.IsSupport);
		Assert.NotEmpty(actual.DetailUrl);
	}
}
