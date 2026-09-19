using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayerRenderModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerRenderersInitializer : NetworkBehaviour
	{
		[SerializeField]
		private List<Renderer> _basePlayerRenderers;

		[SerializeField]
		private List<Renderer> _customPlayerRenderers;

		private PlayerRenderModel _playerRenderModel;

		private bool _registered;

		[Inject]
		public void InjectDependencies(PlayerRenderModel playerRenderModel)
		{
			_playerRenderModel = playerRenderModel;
		}

		public override void Spawned()
		{
			if (base.HasInputAuthority)
			{
				_registered = true;
				_playerRenderModel.InitializePlayerRenderers(_basePlayerRenderers, _customPlayerRenderers);
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (_registered)
			{
				_playerRenderModel.ClearPlayerRenderers();
			}
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
