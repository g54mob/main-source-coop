using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.ObjectPlacement;
using UnityEngine;

namespace NomadDrive.Features.Cooking
{
	public class CoolSnappingPlane : SnappingPlane
	{
		private readonly HashSet<uint> _refrigeratedFoodNetIds = new HashSet<uint>();

		public override void OnStartServer()
		{
			base.OnStartServer();
			SyncList<uint> placedEntityNetworkIDs = base.PlacedEntityNetworkIDs;
			placedEntityNetworkIDs.OnChange = (Action<SyncList<uint>.Operation, int, uint>)Delegate.Combine(placedEntityNetworkIDs.OnChange, new Action<SyncList<uint>.Operation, int, uint>(OnServerPlacedEntitiesChanged));
			ReconcileExistingFoodsAsync().Forget();
		}

		public override void OnStopServer()
		{
			base.OnStopServer();
			SyncList<uint> placedEntityNetworkIDs = base.PlacedEntityNetworkIDs;
			placedEntityNetworkIDs.OnChange = (Action<SyncList<uint>.Operation, int, uint>)Delegate.Remove(placedEntityNetworkIDs.OnChange, new Action<SyncList<uint>.Operation, int, uint>(OnServerPlacedEntitiesChanged));
		}

		private void OnServerPlacedEntitiesChanged(SyncList<uint>.Operation op, int index, uint netId)
		{
			switch (op)
			{
			case SyncList<uint>.Operation.OP_ADD:
			case SyncList<uint>.Operation.OP_INSERT:
				PauseFoodAsync(netId).Forget();
				break;
			case SyncList<uint>.Operation.OP_SET:
			case SyncList<uint>.Operation.OP_REMOVEAT:
				ResumeFood(netId);
				break;
			case SyncList<uint>.Operation.OP_CLEAR:
				ResumeAllRefrigeratedFoods();
				break;
			}
		}

		private async UniTaskVoid ReconcileExistingFoodsAsync()
		{
			foreach (uint placedEntityNetworkID in base.PlacedEntityNetworkIDs)
			{
				await PauseFoodAsync(placedEntityNetworkID);
			}
		}

		private async UniTask PauseFoodAsync(uint netId)
		{
			(bool, GameObject) tuple = await networkManager.TryGetNetworkObjectByIdAsync(netId, 30, 100, this.GetCancellationTokenOnDestroy());
			if (tuple.Item1 && !(tuple.Item2 == null) && base.PlacedEntityNetworkIDs.Contains(netId) && tuple.Item2.TryGetComponent<Food>(out var component))
			{
				component.PauseSpoilage();
				_refrigeratedFoodNetIds.Add(netId);
			}
		}

		private void ResumeFood(uint netId)
		{
			if (_refrigeratedFoodNetIds.Remove(netId) && networkManager.TryGetNetworkObjectById(netId, out var networkObject) && networkObject != null && networkObject.TryGetComponent<Food>(out var component))
			{
				component.ResumeSpoilage();
			}
		}

		private void ResumeAllRefrigeratedFoods()
		{
			foreach (uint refrigeratedFoodNetId in _refrigeratedFoodNetIds)
			{
				if (networkManager.TryGetNetworkObjectById(refrigeratedFoodNetId, out var networkObject) && networkObject != null && networkObject.TryGetComponent<Food>(out var component))
				{
					component.ResumeSpoilage();
				}
			}
			_refrigeratedFoodNetIds.Clear();
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
