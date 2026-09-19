using Fusion;
using UnityEngine;

namespace Features.WeaponModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class PlayParticleOnSpawned : NetworkBehaviour
	{
		[SerializeField]
		private ParticleSystem _particleSystem;

		private bool _isSpawned;

		public override void Spawned()
		{
			_isSpawned = true;
			Play();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_isSpawned = false;
			StopAndClear();
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			if (_isSpawned)
			{
				Play();
			}
		}

		private void Play()
		{
			if (!(_particleSystem == null))
			{
				_particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
				_particleSystem.Play(withChildren: true);
			}
		}

		private void StopAndClear()
		{
			if (!(_particleSystem == null))
			{
				_particleSystem.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
