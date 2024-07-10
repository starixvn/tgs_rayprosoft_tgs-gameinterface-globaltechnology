namespace Tgs.GameInterface.GlobalTechnology.Core.Models;
public record HistoryGameResp(
	string PlayerCode,
	string ExternalBetId,
	string ExternalGameId,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	string RoundType,
	decimal Bet,
	decimal Cancels,
	decimal Settle,
	decimal JackpotContribution,
	decimal JackpotWin,
	string AdditionalData);
