using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InteractModule.Scripts;
using Features.PhysicsUtilsModule.Scripts;
using Features.TipsModule.Scripts.Data;
using Fusion;
using QuickOutline.Scripts;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public interface IPointGrabable
	{
		bool IsReadyToQuota { get; set; }

		bool IsMainRagdollGrabable { get; }

		bool ResetPhysicsOnCart { get; }

		bool WeightResetOnCart { get; }

		bool GrabBlocked { get; set; }

		bool CanGrabInStun { get; }

		bool IgnoredByCart { get; }

		float CartMass { get; }

		float OriginalMass { get; }

		bool Initialized { get; }

		NetworkObject NetworkObject { get; }

		Rigidbody Rigidbody { get; }

		GameObject GameObject { get; }

		ArmConfiguration ArmConfiguration { get; }

		GrabObjectBase GrabObject { get; }

		InteractableBase Interactable { get; }

		GrabDistanceType GrabDistanceType { get; }

		bool IsAuthorityRequested { get; set; }

		bool InCart { get; set; }

		List<GameObject> Carts { get; set; }

		bool LocalGrabBlocked { get; set; }

		bool FreezeRotationOnGrab { get; }

		List<int> GrabbedByPlayers { get; }

		List<int> GrabbedByExternals { get; }

		List<Transform> Handles { get; }

		float Weight { get; }

		bool IsReturnsAuthorityToHost { get; }

		bool IgnoreItemsCollision { get; }

		int GrabbedByPlayersCount { get; }

		int GrabbedByExternalsCount { get; }

		int GrabbedBySomethingCount { get; }

		Outline Outline { get; set; }

		GrabbableStaticType GrabbableStaticType { get; }

		bool IsHeavyItem { get; }

		bool IsAutoGrabEnabled { get; }

		PhysicsResolutionController PhysicsResolutionController { get; }

		float HeavyItemMultiplier { get; set; }

		bool DisableGrabWhileHoldingOther { get; set; }

		bool IsGrabPriority { get; set; }

		event Action OnGrab;

		event Action OnUnGrab;

		event Action OnCleanup;

		event Action OnGrabbedPlayersChanged;

		event Action OnGrabbedExternalsChanged;

		event Action<bool> OnTipsChanged;

		void EnableOutline(bool enable);

		void LockOutline(bool locked);

		void ForceOutline(bool enable);

		void InvokeOnGrab();

		void InvokeOnUnGrab();

		void GrabbedByPlayer(int playerId);

		void UnGrabbedByPlayer(int playerId);

		void GrabbedByExternal(int holderId, int authorityPlayerId);

		void UnGrabbedByExternal(int holderId);

		void RequestStateAuthorityRPC(int playerId);

		void AddIgnoreItemsCollisionRequest(int requestOwnerID);

		void RemoveIgnoreItemsCollisionRequest(int requestOwnerID);

		void SuppressCollisionDamageFor(float seconds);

		void Cleanup();

		Transform GetNearestHandle(Vector3 position);

		List<TipType> GetTips();

		List<TipType> GetRaycastTips();

		void SetPhysicsMaterialToColliders(PhysicsMaterial physicMaterialOnGrab);

		void BlockGrabRPC();

		void EnableGrabRPC();
	}
}
