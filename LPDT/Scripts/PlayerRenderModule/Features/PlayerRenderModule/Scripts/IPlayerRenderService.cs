namespace Features.PlayerRenderModule.Scripts
{
	public interface IPlayerRenderService
	{
		void SetPlayerAlpha(float alpha);

		void SetCustomPlayerAlpha(float alpha);

		void SetBasePlayerAlpha(float alpha);

		void SetPlayerSurfaceType(PlayerRendererSurfaceType surfaceType);

		void SetCustomPlayerSurfaceType(PlayerRendererSurfaceType surfaceType);

		void SetBasePlayerSurfaceType(PlayerRendererSurfaceType surfaceType);

		void SetPlayerSurfaceRenderQueue(int renderQueue);
	}
}
