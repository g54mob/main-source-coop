using System;
using System.Collections.Generic;
using Features.GrabModule.Scripts;
using Features.GrabModule.Scripts.PhysGrab;
using Features.InteractModule.Scripts;
using Features.QuotaModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.MagnetModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class MagnetInteractable : InteractableBase, IToggleableInteractable
	{
		[SerializeField]
		private LayerMask _nothingLayerMask;

		[SerializeField]
		private LayerMask _collectablesExcludeLayerMask;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private float _maxCapacity = 10f;

		[SerializeField]
		private SimplePointGrabable _simplePointGrabable;

		[SerializeField]
		private PhysGrabber _physGrabber;

		[SerializeField]
		private MagnetCollisionHandler _magnetCollisionHandler;

		[SerializeField]
		private List<GrabDistanceType> _allowedGrabDistanceTypes;

		[SerializeField]
		private LayerMask _obstacleMask;

		private List<SimplePointGrabable> _grabbedItems = new List<SimplePointGrabable>();

		private List<SimplePointGrabable> _itemsInRange = new List<SimplePointGrabable>();

		private bool _isInitialized;

		private QuotaContainerModel _quotaContainerModel;

		[WeaverGenerated]
		[DefaultForProperty("IsInteracted", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private bool _IsInteracted;

		private bool _isPendingInteraction;

		private bool _lastToggleState;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe bool IsInteracted
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MagnetInteractable.IsInteracted. Networked properties can only be accessed when Spawned() has been called.");
				}
				return ReadWriteUtilsForWeaver.ReadBoolean((int*)((byte*)Ptr + 0));
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing MagnetInteractable.IsInteracted. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(NetworkBool*)((byte*)Ptr + 0) = new NetworkBool(value);
			}
		}

		public List<SimplePointGrabable> GrabbedItems => _grabbedItems;

		public bool IsToggledOn => IsInteracted;

		public event Action OnToggleChanged;

		[Inject]
		private void InjectDependencies(QuotaContainerModel quotaContainerModel)
		{
			_quotaContainerModel = quotaContainerModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			_isInitialized = true;
		}

		private void OnEnable()
		{
			NetworkBehaviourUtils.InternalOnEnable(this);
			_magnetCollisionHandler.OnItemInRange += ProcessItemsAdd;
			_magnetCollisionHandler.OnItemInRangeExit += ProcessItemsExit;
		}

		private void OnDisable()
		{
			NetworkBehaviourUtils.InternalOnDisable(this);
			_magnetCollisionHandler.OnItemInRange -= ProcessItemsAdd;
			_magnetCollisionHandler.OnItemInRangeExit -= ProcessItemsExit;
		}

		private void ProcessItemsExit(SimplePointGrabable simplePointGrabable)
		{
			if (_itemsInRange.Contains(simplePointGrabable))
			{
				_itemsInRange.Remove(simplePointGrabable);
			}
		}

		private void ProcessItemsAdd(SimplePointGrabable simplePointGrabable)
		{
			if (!_itemsInRange.Contains(simplePointGrabable))
			{
				_itemsInRange.Add(simplePointGrabable);
			}
		}

		public override void Interact()
		{
			if (_isInitialized && IsInteractable)
			{
				if (base.Object.HasStateAuthority)
				{
					ExecuteInteractionLogic();
					return;
				}
				_isPendingInteraction = true;
				base.Object.RequestStateAuthority();
			}
		}

		public override void StateAuthorityChanged()
		{
			ClearLocalPendingInteractionUnlessAuthority(ref _isPendingInteraction);
			base.StateAuthorityChanged();
			if (base.HasStateAuthority && _isPendingInteraction)
			{
				_isPendingInteraction = false;
				ExecuteInteractionLogic();
			}
		}

		private void ExecuteInteractionLogic()
		{
			IsInteracted = !IsInteracted;
		}

		public override void OnInteractEnd()
		{
			IsInteracted = false;
		}

		private void Update()
		{
			if (_isInitialized && _lastToggleState != IsInteracted)
			{
				_lastToggleState = IsInteracted;
				this.OnToggleChanged?.Invoke();
			}
		}

		private void FixedUpdate()
		{
			if (!_isInitialized || !base.HasStateAuthority)
			{
				return;
			}
			if (IsInteracted)
			{
				foreach (SimplePointGrabable item in _itemsInRange)
				{
					if (!(GetGrabbedItemsWeight() + item.Weight > _maxCapacity) && !_grabbedItems.Contains(item) && _allowedGrabDistanceTypes.Contains(item.GrabDistanceType) && item.GrabbedByPlayersCount <= 0 && !item.InCart && !IsItemInQuotaContainer(item) && IsItemAvailable(item))
					{
						_physGrabber.physGrabPoints.Add(item.GrabObject, _simplePointGrabable.GetNearestHandle(base.transform.position));
						item.GrabObject.Grabbers.Add(_physGrabber);
						_grabbedItems.Add(item);
						item.AddIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
						if (item.NetworkObject.StateAuthority.PlayerId != base.Object.StateAuthority.PlayerId)
						{
							item.RequestStateAuthorityRPC(base.Object.StateAuthority.PlayerId);
						}
					}
				}
				_collider.excludeLayers = _collectablesExcludeLayerMask;
				List<SimplePointGrabable> list = new List<SimplePointGrabable>();
				foreach (SimplePointGrabable grabbedItem in _grabbedItems)
				{
					if (grabbedItem.GrabbedByPlayersCount > 0 || grabbedItem.InCart || !_itemsInRange.Contains(grabbedItem))
					{
						list.Add(grabbedItem);
					}
				}
				{
					foreach (SimplePointGrabable item2 in list)
					{
						Unjoin(item2);
					}
					return;
				}
			}
			UnjoinAll();
			_collider.excludeLayers = _nothingLayerMask;
		}

		private bool IsItemInQuotaContainer(SimplePointGrabable simplePointGrabable)
		{
			foreach (QuotaContainerItemData freeContainerItem in _quotaContainerModel.FreeContainerItems)
			{
				if (freeContainerItem.PointGrabable == simplePointGrabable)
				{
					return true;
				}
			}
			return false;
		}

		private void Unjoin(SimplePointGrabable simplePointGrabable)
		{
			if (_grabbedItems.Contains(simplePointGrabable))
			{
				simplePointGrabable.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
				simplePointGrabable.GrabObject.Grabbers.Remove(_physGrabber);
				_physGrabber.physGrabPoints.Remove(simplePointGrabable.GrabObject);
				_grabbedItems.Remove(simplePointGrabable);
				simplePointGrabable.Rigidbody.linearVelocity = Vector3.zero;
			}
		}

		private void UnjoinAll()
		{
			foreach (SimplePointGrabable grabbedItem in _grabbedItems)
			{
				grabbedItem.GrabObject.Grabbers.Remove(_physGrabber);
				_physGrabber.physGrabPoints.Remove(grabbedItem.GrabObject);
				grabbedItem.RemoveIgnoreItemsCollisionRequest(base.gameObject.GetHashCode());
			}
			_grabbedItems.Clear();
		}

		public float GetGrabbedItemsWeight()
		{
			float num = 0f;
			foreach (SimplePointGrabable grabbedItem in _grabbedItems)
			{
				num += grabbedItem.Weight;
			}
			return num;
		}

		public bool IsItemAvailable(SimplePointGrabable simplePointGrabable)
		{
			Vector3 position = base.transform.position;
			Vector3 direction = simplePointGrabable.transform.position - position;
			float magnitude = direction.magnitude;
			if (magnitude <= 0.001f)
			{
				return true;
			}
			direction /= magnitude;
			RaycastHit hitInfo;
			bool flag = Physics.Raycast(position, direction, out hitInfo, magnitude, _obstacleMask, QueryTriggerInteraction.Ignore);
			if (flag && hitInfo.transform.gameObject == simplePointGrabable.gameObject)
			{
				return true;
			}
			return !flag;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
			IsInteracted = _IsInteracted;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
			_IsInteracted = IsInteracted;
		}
	}
}
