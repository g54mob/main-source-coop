using System;
using Features.Movement.Scripts;
using Features.RagdollModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class CopyRotationFromObject : NetworkBehaviour
	{
		[SerializeField]
		private bool _ignoreYRotation;

		[SerializeField]
		private float _lerpSpeed = 5f;

		private bool _isInitialized;

		private PlayersRagdollModel _playersRagdollModel;

		private PlayerMovableModel _playerMovableModel;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("DisableRotation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _DisableRotation;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe bool DisableRotation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CopyRotationFromObject.DisableRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing CopyRotationFromObject.DisableRotation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public Transform ObjectToCopy { get; internal set; }

		[Inject]
		private void InjectDependencies(PlayersRagdollModel playersRagdollModel, PlayerMovableModel playerMovableModel)
		{
			_playersRagdollModel = playersRagdollModel;
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			_isInitialized = true;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isInitialized = false;
		}

		private void LateUpdate()
		{
			if (_isInitialized && !DisableRotation && !(ObjectToCopy == null) && (!_playersRagdollModel.PlayersRagdoll.TryGetValue(base.Object.InputAuthority.PlayerId, out var value) || !value.IsSimulated))
			{
				CopyRotation(ObjectToCopy);
			}
		}

		private void CopyRotation(Transform objectToCopy)
		{
			Vector3 forward = objectToCopy.forward;
			if (_ignoreYRotation)
			{
				forward.y = 0f;
				forward.Normalize();
			}
			base.transform.forward = Vector3.Lerp(base.transform.forward, forward, Time.deltaTime * _lerpSpeed);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			DisableRotation = _DisableRotation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_DisableRotation = DisableRotation;
		}
	}
}
