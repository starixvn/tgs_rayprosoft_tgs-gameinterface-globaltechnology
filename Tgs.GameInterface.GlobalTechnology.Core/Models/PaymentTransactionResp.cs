namespace Tgs.GameInterface.GlobalTechnology.Core.Models;

public record PaymentTransactionResp(
	string TransId,
	string ExtTransId,
	decimal Balance,
	string Resp);
