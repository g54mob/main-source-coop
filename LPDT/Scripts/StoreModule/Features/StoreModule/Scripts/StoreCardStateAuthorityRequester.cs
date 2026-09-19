using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.StoreModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class StoreCardStateAuthorityRequester : NetworkBehaviour
	{
		private const int NoRequestedTargetPlayerId = -1;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private LayerMask _collisionLayerMask = -1;

		[SerializeField]
		private Vector3 _overlapCenterOffset;

		[SerializeField]
		private Vector3 _overlapHalfExtents = Vector3.one;

		[SerializeField]
		private QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.Collide;

		private readonly Dictionary<SimplePointGrabable, int> _requestedAuthorityTargetsByCard = new Dictionary<SimplePointGrabable, int>();

		private readonly Collider[] _overlapResults = new Collider[32];

		private CardsOnTableModel _cardsOnTableModel;

		[Inject]
		private void InjectDependencies(CardsOnTableModel cardsOnTableModel)
		{
			_cardsOnTableModel = cardsOnTableModel;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_simplePointGrabable.LocalOnGrab += RequestCachedCardsStateAuthority;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_simplePointGrabable.LocalOnGrab -= RequestCachedCardsStateAuthority;
			_requestedAuthorityTargetsByCard.Clear();
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_requestedAuthorityTargetsByCard.Clear();
		}

		private void RequestCachedCardsStateAuthority(int i1)
		{
			if (!TryGetTargetPlayerId(out var targetPlayerId) || base.Runner == null || base.Runner.LocalPlayer.PlayerId != targetPlayerId)
			{
				return;
			}
			int num = Physics.OverlapBoxNonAlloc(base.transform.TransformPoint(_overlapCenterOffset), _overlapHalfExtents, _overlapResults, base.transform.rotation, _collisionLayerMask, _queryTriggerInteraction);
			for (int j = 0; j < num; j++)
			{
				if (TryGetSimplePointGrabable(_overlapResults[j].gameObject, out var simplePointGrabable) && CanRequestStateAuthority(simplePointGrabable))
				{
					int value;
					if (simplePointGrabable.NetworkObject.StateAuthority.PlayerId == targetPlayerId)
					{
						_requestedAuthorityTargetsByCard.Remove(simplePointGrabable);
					}
					else if (!_requestedAuthorityTargetsByCard.TryGetValue(simplePointGrabable, out value) || value != targetPlayerId)
					{
						_requestedAuthorityTargetsByCard[simplePointGrabable] = targetPlayerId;
						simplePointGrabable.RequestStateAuthorityRPC(targetPlayerId);
					}
				}
			}
		}

		private bool CanRequestStateAuthority(SimplePointGrabable simplePointGrabable)
		{
			if (simplePointGrabable == null || simplePointGrabable == _simplePointGrabable)
			{
				return false;
			}
			if (!simplePointGrabable.Initialized || simplePointGrabable.NetworkObject == null)
			{
				return false;
			}
			if (simplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				return false;
			}
			return true;
		}

		private bool TryGetTargetPlayerId(out int targetPlayerId)
		{
			if (_simplePointGrabable.GrabbedByPlayers.Count > 0)
			{
				targetPlayerId = _simplePointGrabable.GrabbedByPlayers[0];
				return true;
			}
			if (base.Object != null)
			{
				targetPlayerId = base.Object.StateAuthority.PlayerId;
				return true;
			}
			targetPlayerId = -1;
			return false;
		}

		private bool TryGetSimplePointGrabable(GameObject grabbable, out SimplePointGrabable simplePointGrabable)
		{
			simplePointGrabable = null;
			if (_cardsOnTableModel == null)
			{
				return false;
			}
			StoreCardBehaviour storeCardBehaviour = grabbable.GetComponentInParent<StoreCardBehaviour>();
			if (storeCardBehaviour == null)
			{
				storeCardBehaviour = grabbable.GetComponentInChildren<StoreCardBehaviour>();
			}
			if (storeCardBehaviour == null || storeCardBehaviour.SimplePointGrabable == _simplePointGrabable)
			{
				return false;
			}
			foreach (StoreCardBehaviour item in _cardsOnTableModel.CardsOnTable)
			{
				if (!(item != storeCardBehaviour))
				{
					simplePointGrabable = item.SimplePointGrabable;
					break;
				}
			}
			return simplePointGrabable != null;
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
