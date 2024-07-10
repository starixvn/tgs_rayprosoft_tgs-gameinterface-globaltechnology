namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;
public record GameRecordResult(
	string PlayerCode,
	string ExternalBetId,
	string ExternalGameId,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	bool IsFreeGame,
	bool IsBonusGame,
	decimal Bet,
	decimal Settle,
	decimal WinLoss,
	decimal JackpotContribution,
	decimal JackpotWin,
	string AdditionalData);
