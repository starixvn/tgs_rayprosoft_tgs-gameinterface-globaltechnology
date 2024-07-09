namespace Tgs.GameInterface.GlobalTechnology.Core;

public class ErrorCodeException(string errorCode, string message) : Exception($"{errorCode}: {message}")
{
	public string ErrorCode => errorCode;
}
