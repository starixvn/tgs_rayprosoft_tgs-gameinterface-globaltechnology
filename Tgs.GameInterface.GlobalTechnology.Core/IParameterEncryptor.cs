using System.Text.Json.Nodes;

namespace Tgs.GameInterface.GlobalTechnology.Core;

public interface IParameterEncryptor
{
	string Encrypt(JsonObject values, string secretKey);
}
