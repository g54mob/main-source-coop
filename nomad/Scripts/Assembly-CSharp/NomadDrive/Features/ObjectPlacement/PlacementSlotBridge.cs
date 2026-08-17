using EvilCore.DynamicCasting;
using NomadDrive.Features.Attachables;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Player;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.ObjectPlacement
{
	public class PlacementSlotBridge : MonoBehaviour, IPlayerComponent
	{
		[SerializeField]
		private float snapLerpSpeed = 15f;

		[Inject]
		private IPlayerService _playerService;

		[Inject]
		private ICastingManager _castingManager;

		[SerializeField]
		private SlotHighlightSettings slotHighlightSettings;

		private ObjectPlacementManager _placementManager;

		private AttachableObject _placementAttachable;

		private GameObject _placementObject;

		private IObjectSlot _activeSnapSlot;

		private IRaycastHandle _rayHandle;

		private bool _isActive;

		public int SetupPriority => 11;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (!isLocalPlayer)
			{
				base.enabled = false;
				return;
			}
			_placementManager = GetComponent<ObjectPlacementManager>();
			_placementManager.OnTrySlotAttach = TrySlotAttach;
			_placementManager.OnPlacementEnter.AddListener(OnPlacementEnter);
		}

		private void FixedUpdate()
		{
			if (_isActive && !_placementManager.IsPlacementModeActive)
			{
				Deactivate();
			}
		}

		private void OnPlacementEnter(GameObject obj)
		{
			if (obj.TryGetComponent<AttachableObject>(out var component) && component.targetSlots != null && component.targetSlots.Count != 0)
			{
				_placementAttachable = component;
				_placementObject = obj;
				_isActive = true;
				RegisterSlotRay();
			}
		}

		private void Deactivate()
		{
			ExitSlotSnap();
			UnregisterSlotRay();
			_playerService.EquipmentManager?.DisableObjectSlotDetectionMode();
			_placementAttachable = null;
			_placementObject = null;
			_isActive = false;
		}

		private void RegisterSlotRay()
		{
			if (_rayHandle == null && _playerService.TryGetCameraTransform(out var cameraTransform))
			{
				InteractionManager component = GetComponent<InteractionManager>();
				float distance = ((component != null) ? component.interactionDistance : 2f);
				CastRequest request = CastRequest.Ray(cameraTransform, distance, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Collide);
				request.UpdateMode = UpdateMode.LateUpdate;
				request.MaxHits = 16;
				_rayHandle = _castingManager.Register(request, OnSlotRayResult);
			}
		}

		private void UnregisterSlotRay()
		{
			if (_rayHandle != null)
			{
				_castingManager?.Unregister(_rayHandle);
				_rayHandle = null;
			}
		}

		private void OnSlotRayResult(CastResult result)
		{
			if (!_isActive || _placementAttachable == null)
			{
				return;
			}
			IObjectSlot objectSlot = ResolveCompatibleSlot(result);
			if (objectSlot != null)
			{
				if (_activeSnapSlot != objectSlot)
				{
					ExitSlotSnap();
					EnterSlotSnap(objectSlot);
				}
				ApplySnapTransform(objectSlot);
			}
			else if (_activeSnapSlot != null)
			{
				ExitSlotSnap();
			}
		}

		private IObjectSlot ResolveCompatibleSlot(CastResult result)
		{
			if (!result.DidHit)
			{
				return null;
			}
			for (int i = 0; i < result.HitCount; i++)
			{
				RaycastHit raycastHit = result.Hits[i];
				if (!(raycastHit.collider == null))
				{
					IObjectSlot objectSlot = ResolveSlotFromCollider(raycastHit.collider);
					if (objectSlot != null && _placementAttachable.targetSlots.Contains(objectSlot.SlotType) && !IsSlotOccupied(objectSlot))
					{
						return objectSlot;
					}
				}
			}
			return null;
		}

		private static IObjectSlot ResolveSlotFromCollider(Collider col)
		{
			if (col.TryGetComponent<IObjectSlot>(out var component))
			{
				return component;
			}
			if (col.TryGetComponent<InteractableRouter>(out var component2) && component2.RouteInteractable != null)
			{
				return component2.RouteInteractable.GetComponent<IObjectSlot>();
			}
			if (col.TryGetComponent<StaticInteractableRouter>(out var component3) && component3.RouteInteractable is IObjectSlot result)
			{
				return result;
			}
			return null;
		}

		private void EnterSlotSnap(IObjectSlot slot)
		{
			_activeSnapSlot = slot;
			_placementManager.IsExternalSnapActive = true;
			if (_placementAttachable.HighlightSource != null)
			{
				Material overrideMaterial = ((slotHighlightSettings != null) ? slotHighlightSettings.sweepHighlightMaterial : null);
				slot.SetupHighlightModel(_placementAttachable.HighlightSource, Vector3.zero, Quaternion.identity, overrideMaterial);
			}
			slot.SetHighlight(ObjectHighlightType.CorrectPlacement);
			slot.UnignoreHovering();
			slot.ApplyAttachPreview(_placementAttachable);
		}

		private void ExitSlotSnap(bool resetPreview = true)
		{
			if (_activeSnapSlot != null)
			{
				if (resetPreview)
				{
					_activeSnapSlot.ClearAttachPreview(_placementAttachable);
					if (_placementObject != null && _placementAttachable != null)
					{
						_placementObject.transform.localScale = _placementAttachable.OriginalLocalScale;
					}
				}
				_activeSnapSlot.ClearHighlightModel();
				_activeSnapSlot.SetHighlight(ObjectHighlightType.None);
				_activeSnapSlot.IgnoreHovering();
			}
			_activeSnapSlot = null;
			_placementManager.IsExternalSnapActive = false;
		}

		private void ApplySnapTransform(IObjectSlot slot)
		{
			Transform slotTransform = GetSlotTransform(slot);
			if (!(slotTransform == null) && !(_placementAttachable == null))
			{
				float t = Time.deltaTime * snapLerpSpeed;
				_placementObject.transform.position = Vector3.Lerp(_placementObject.transform.position, slotTransform.position, t);
				_placementObject.transform.rotation = Quaternion.Slerp(_placementObject.transform.rotation, slotTransform.rotation, t);
				_placementObject.transform.localScale = Vector3.Scale(slotTransform.lossyScale, _placementAttachable.OriginalLocalScale);
			}
		}

		private bool TrySlotAttach()
		{
			if (_activeSnapSlot == null)
			{
				return false;
			}
			if (_placementAttachable == null)
			{
				return false;
			}
			_activeSnapSlot.AttachFromPlacement(_placementAttachable.netId);
			ExitSlotSnap(resetPreview: false);
			UnregisterSlotRay();
			return true;
		}

		private static bool IsSlotOccupied(IObjectSlot slot)
		{
			if (slot is ObjectSlot objectSlot)
			{
				return objectSlot.CurrentState == ObjectSlotState.Occupied;
			}
			if (slot is VehicleSlot vehicleSlot)
			{
				return vehicleSlot.IsOccupied;
			}
			return false;
		}

		private static Transform GetSlotTransform(IObjectSlot slot)
		{
			if (slot is MonoBehaviour monoBehaviour)
			{
				return monoBehaviour.transform;
			}
			return null;
		}
	}
}
