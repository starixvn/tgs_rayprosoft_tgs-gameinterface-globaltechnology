using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient apiClient) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> apiClient.PlayerkickOutAsync(
			apiBaseUrl: command.ApiUrl,
			brandId: command.BrandId,
			secretKey: command.SecretKey,
			playerId: command.PlayerCode,
			cancellationToken: cancellationToken);
}
