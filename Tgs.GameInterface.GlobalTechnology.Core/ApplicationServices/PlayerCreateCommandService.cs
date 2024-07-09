using Tgs.ServiceUtility;

namespace Tgs.GameInterface.GlobalTechnology.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient apiClient) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> apiClient.PlayerCreateAsync(
			apiBaseUrl: command.ApiUrl,
			brandId: command.BrandId,
			secretKey: command.SecretKey,
			playerId: command.PlayerCode,
			currency: command.Currency,
			language: command.Language,
			cancellationToken: cancellationToken);
}
