using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
using Tgs.GameInterface.GlobalTechnology.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.GrpcServices;

public class GameInterfaceGlobalTechnologyGrpcService(
	ILogger<GameInterfaceGlobalTechnologyGrpcService> logger,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
	IGetService<GameUrlQuery, GameUrlResult> gameUrlService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	ICommandService<PlayerCreateCommand> playerCreateService,
	ICommandService<PlayerLogoutCommand> playerLogoutService
	) : GameInterfaceGlobalTechnology.GameInterfaceGlobalTechnologyBase
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName,
		});
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode,
			Currency = request.Currency,
			OrderId = request.OrderId,
			Amount = request.Amount
		};

		var result = await fundInService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			OrderId = result.OrderId,
			ExternalOrderId = result.ExternalOrderId,
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode,
			Currency = request.Currency,
			OrderId = request.OrderId,
			Amount = request.Amount
		};

		var result = await fundOutService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			OrderId = result.OrderId,
			ExternalOrderId = result.ExternalOrderId,
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			OrderId = request.OrderId
		};

		var result = await fundStatusService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			Status = result
		};
	}

	public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
	{
		var query = new GameRecordQuery
		{
			ApiUrl = new Uri(request.DataApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			StartTime = request.StartTime.ToDateTimeOffset(),
			EndTime = request.EndTime.ToDateTimeOffset(),
			Page = request.Page,
			Size = request.PageSize
		};

		var results = gameRecordService.QueryAsync(
			query: query,
			cancellationToken: context.CancellationToken);

		var reply = new GameRecordReply();

		await foreach (var result in results)
		{
			reply.Items.Add(new GameRecordInfo
			{
				PlayerCode = result.PlayerCode,
				ExternalRoundId = result.ExternalRoundId,
				ExternalBetId = result.ExternalBetId,
				ExternalGameId = result.ExternalGameId,
				ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
				ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
				IsFreeGame = result.IsFreeGame,
				IsBonusGame = result.IsBonusGame,
				Bet = result.Bet,
				Settle = result.Settle,
				WinLoss = result.WinLoss,
				JackpotContribution = result.JackpotContribution,
				JackpotWin = result.JackpotWin,
				AdditionalData = result.AdditionalData
			});
		}

		return reply;
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			GameLauncherUrl = new Uri(request.LauncherApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode,
			Currency = request.Currency,
			Language = request.Language,
			IsMobile = request.IsMobile,
			GameCode = request.GameCode
		};

		var result = await gameUrlService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply
		{
			IsPlayerExist = result.IsPlayerExist,
			GameUrl = result.GameUrl
		};
	}

	public override async Task<GameUrlReply> GameUrlPlaytechByLive(GameUrlPlaytechByLiveRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			GameLauncherUrl = new Uri(request.LauncherApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode,
			Currency = request.Currency,
			Language = request.Language,
			IsMobile = request.IsMobile,
			GameCode = "bal"
		};

		var result = await gameUrlService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply
		{
			IsPlayerExist = result.IsPlayerExist,
			GameUrl = result.GameUrl
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode
		};

		var result = await playerBalanceService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply
		{
			IsPlayerExist = result.IsPlayerExist,
			Balance = result.Balance,
		};
	}

	public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
	{
		var command = new PlayerCreateCommand
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode,
			Currency = request.Currency,
			Language = request.Language
		};

		await playerCreateService.ExecuteAsync(
			command: command,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			Status = true
		};
	}

	public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
	{
		var command = new PlayerLogoutCommand
		{
			ApiUrl = new Uri(request.ApiUrl),
			BrandId = request.BrandId,
			SecretKey = request.SecretKey,
			PlayerCode = request.PlayerCode
		};

		await playerLogoutService.ExecuteAsync(
			command: command,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			Status = true
		};
	}
}
