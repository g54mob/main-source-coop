namespace Features.PlayerSkinModule.Scripts
{
	public class PlayerStateSkinSwitchService : IPlayerStateSkinSwitchService
	{
		private readonly SkinModel _skinModel;

		public PlayerStateSkinSwitchService(SkinModel skinModel)
		{
			_skinModel = skinModel;
		}

		public void ApplyPlayerSkinState(int playerId, bool isDead, bool hasBottomPart)
		{
			if (TryGetSkinHandlers(playerId, out var skinSpawner))
			{
				if (isDead)
				{
					ApplyDeadAppearance(playerId, skinSpawner);
				}
				else
				{
					ApplyAliveAppearance(playerId, skinSpawner, hasBottomPart);
				}
			}
		}

		public void SwitchSkin(int playerId, bool isDead, bool isBodyVisible)
		{
			HandleLocalVisibility(playerId, isDead, isBodyVisible);
		}

		private void ApplyDeadAppearance(int playerId, SkinSpawner skinSpawner)
		{
			skinSpawner.ApplyDeadSkinLocal();
			if (_skinModel.BaseCharacterVisibility[playerId].IsRenderObjectEnabled)
			{
				_skinModel.BaseCharacterVisibility[playerId].DisableRenderObject();
			}
			if (_skinModel.BottomCharacterVisibility[playerId].IsRenderObjectEnabled)
			{
				_skinModel.BottomCharacterVisibility[playerId].DisableRenderObject();
			}
			if (!_skinModel.HatCharacterVisibility[playerId].IsRenderObjectEnabled)
			{
				_skinModel.HatCharacterVisibility[playerId].EnableRenderObject();
			}
			if (!_skinModel.TorsoCharacterVisibility[playerId].IsRenderObjectEnabled)
			{
				_skinModel.TorsoCharacterVisibility[playerId].EnableRenderObject();
			}
		}

		private void ApplyAliveAppearance(int playerId, SkinSpawner skinSpawner, bool hasBottomPart)
		{
			skinSpawner.ApplyAliveSkinLocal();
			if (hasBottomPart)
			{
				if (!_skinModel.BottomCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.BottomCharacterVisibility[playerId].EnableRenderObject();
				}
				if (!_skinModel.HatCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.HatCharacterVisibility[playerId].EnableRenderObject();
				}
				if (!_skinModel.TorsoCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.TorsoCharacterVisibility[playerId].EnableRenderObject();
				}
				if (_skinModel.BaseCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.BaseCharacterVisibility[playerId].DisableRenderObject();
				}
			}
			else
			{
				if (_skinModel.BottomCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.BottomCharacterVisibility[playerId].DisableRenderObject();
				}
				if (_skinModel.HatCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.HatCharacterVisibility[playerId].DisableRenderObject();
				}
				if (_skinModel.TorsoCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.TorsoCharacterVisibility[playerId].DisableRenderObject();
				}
				if (!_skinModel.BaseCharacterVisibility[playerId].IsRenderObjectEnabled)
				{
					_skinModel.BaseCharacterVisibility[playerId].EnableRenderObject();
				}
			}
		}

		private void HandleLocalVisibility(int playerId, bool isDead, bool isBodyVisible)
		{
			if (TryGetSkinHandlers(playerId, out var skinSpawner))
			{
				if (isDead)
				{
					_skinModel.AllCharacterVisibility[playerId].EnableVisibility();
					skinSpawner.EnableDeadVisibility();
				}
				else if (!isBodyVisible)
				{
					_skinModel.AllCharacterVisibility[playerId].DisableVisibility();
					skinSpawner.DisableVisibility();
				}
				else
				{
					_skinModel.AllCharacterVisibility[playerId].EnableVisibility();
					skinSpawner.EnableVisibility();
				}
			}
		}

		private bool TryGetSkinHandlers(int playerId, out SkinSpawner skinSpawner)
		{
			skinSpawner = null;
			if (!_skinModel.SkinVisibility.TryGetValue(playerId, out skinSpawner))
			{
				return false;
			}
			if (_skinModel.AllCharacterVisibility.ContainsKey(playerId) && _skinModel.BaseCharacterVisibility.ContainsKey(playerId) && _skinModel.HatCharacterVisibility.ContainsKey(playerId) && _skinModel.TorsoCharacterVisibility.ContainsKey(playerId))
			{
				return _skinModel.BottomCharacterVisibility.ContainsKey(playerId);
			}
			return false;
		}
	}
}
