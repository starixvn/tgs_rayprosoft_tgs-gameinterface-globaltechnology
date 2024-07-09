using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Tgs.GameInterface.GlobalTechnology.Core;

internal class ParameterEncryptor : IParameterEncryptor
{
	/// <summary>
	/// 參考作文件: https://globaltllc.atlassian.net/wiki/spaces/GAG1/pages/1243546890/6.#C%23
	/// </summary>

	public string Encrypt(JsonObject values, string secretKey)
		=> EncryptString(ExceptHashAndSort(values) + secretKey);

	private static string ExceptHashAndSort(IDictionary<string, JsonNode?> requestDict)
	{
		requestDict.Remove("hash");
		var plain = new StringBuilder();
		foreach (KeyValuePair<string, JsonNode?> pair in requestDict.OrderBy(o => o.Key))
		{
			plain.Append(pair.Key).Append('=');
			if (pair.Value is { } value && value.GetValueKind() == JsonValueKind.Array)
			{
				plain.Append(value.AsArray().ToJsonString());
			}
			else
			{
				plain.Append(pair.Value);
			}

			plain.Append('&');
		}

		plain.Length -= 1;

		return plain.ToString();
	}

	private static string EncryptString(string str)
	{
		byte[] byteOld = Encoding.UTF8.GetBytes(str);
		byte[] byteNew = MD5.HashData(byteOld);
		var sb = new StringBuilder();
		foreach (byte b in byteNew)
		{
			sb.Append(b.ToString("x2"));
		}

		return sb.ToString();
	}
}
