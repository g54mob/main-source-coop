using System;
using FMODUnity;
using Features.AudioServiceModule.Scripts;
using Features.ScreenShakeModule.Scripts;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

namespace Features.ItemsModule.Scripts
{
	public interface IItem
	{
		ItemType Type { get; }

		bool IsCollectable { get; }

		bool AvailableForEnemy { get; set; }

		bool IsSpawned { get; }

		bool IsDespawned { get; }

		bool IsConsumed { get; }

		bool AreDespawnEffectsSuppressed { get; }

		ushort MaxCurrencyValue { get; }

		ushort CurrencyValue { get; }

		int LastGrabTikRaw { get; }

		NetworkObject NetworkObject { get; }

		bool IsReducible { get; }

		float DamageOnCollide { get; }

		EventReference CollisionSound { get; }

		CinemachineImpulseSource CinemachineImpulseSource { get; }

		ScreenShakeData ScreenShakeData { get; }

		ScreenShakeData ScreenShakeDataOnDestroy { get; }

		ParticleSystem CollisionParticle { get; }

		bool IsSoundOnAnyCollision { get; }

		ITransformBasedSoundSource SoundSource { get; }

		event Action<IItem> OnSpawn;

		event Action<IItem> OnDespawn;

		void SetIsCollectable(bool isTakeable);

		void SetCurrencyValue(int newValue);

		void SetLastGrabTime(Tick grabTime);

		void Consume();

		Vector3 GetPricePosition();
	}
}
