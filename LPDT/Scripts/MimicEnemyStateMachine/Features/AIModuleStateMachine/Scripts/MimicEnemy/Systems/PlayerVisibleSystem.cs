using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.Movement.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerVisibleSystem : MonoSystem
	{
		[SerializeField]
		private Transform _lookTargetTransform;

		private MimicEnemyContext _context;

		[SerializeField]
		private float _hardDetectionDistance;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(MimicEnemyContext context, SpawnedPlayersModel spawnedPlayersModel)
		{
			_context = context;
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled)
			{
				DetectPlayersInArea();
			}
		}

		private void DetectPlayersInArea()
		{
			if (_spawnedPlayersModel.Players.Count == 0)
			{
				return;
			}
			List<PlayerDataHolder> list = new List<PlayerDataHolder>();
			foreach (PlayerDataHolder item in _spawnedPlayersModel.Players.Select((KeyValuePair<PlayerRef, PlayerDataHolder> kv) => kv.Value))
			{
				if (!(item.NetworkObject == null) && !(item.NetworkObject.InputAuthority == PlayerRef.None) && _spawnedPlayersModel.Players.TryGetValue(item.NetworkObject.InputAuthority, out var value) && value.NetworkObject.GetComponent<PlayerLookDetection>().IsLookingAtObject(_lookTargetTransform, angleCullEnabled: false, _hardDetectionDistance))
				{
					list.Add(item);
				}
			}
			_context.SetVisiblePlayers(list);
		}

		public override void Clear()
		{
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
