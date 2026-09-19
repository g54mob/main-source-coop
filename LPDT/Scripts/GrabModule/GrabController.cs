using System;
using System.Collections.Generic;
using System.Linq;
using Features.GrabModule.Scripts;
using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[NetworkBehaviourWeaved(0)]
public class GrabController : NetworkBehaviour
{
	[SerializeField]
	private NetworkRigidbody _networkRigidbody3D;

	[SerializeField]
	private Rigidbody _rigidbody;

	[SerializeField]
	private float _grabRadius = 0.5f;

	[SerializeField]
	private LayerMask _grabMask = -1;

	public GrabState CurrentGrabState;

	private CurrentGrabAbility _currentGrabAbilityState = CurrentGrabAbility.CannotGrab;

	public IGrabableBase SingleGrabbed;

	public readonly List<IGrabableBase> MultipleGrabbed = new List<IGrabableBase>();

	private bool _isThrown;

	public event Action OnGrabStateChanged;

	public void NotifyUnjoin(IGrabableBase grabable)
	{
		if (base.HasStateAuthority)
		{
			Release(grabable);
		}
	}

	public void TryGrab()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, _grabRadius, _grabMask);
		List<IGrabableBase> list = new List<IGrabableBase>();
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			IGrabableBase component2;
			if (collider.TryGetComponent<GrabableCollider>(out var component))
			{
				list.Add(component.Grabable);
			}
			else if (collider.TryGetComponent<IGrabableBase>(out component2))
			{
				list.Add(component2);
			}
		}
		list = list.OrderBy((IGrabableBase g) => g.GrabableType).ToList();
		foreach (IGrabableBase item in list)
		{
			if (item.IsEnableToGrab)
			{
				ProcessGrabable(item);
			}
		}
		if (CurrentGrabState == GrabState.Grabbed)
		{
			_currentGrabAbilityState = CurrentGrabAbility.CannotGrab;
		}
		else
		{
			_currentGrabAbilityState = CurrentGrabAbility.CanGrab;
		}
	}

	public void UnGrab()
	{
		ReleaseAll();
		_currentGrabAbilityState = CurrentGrabAbility.CannotGrab;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (base.HasStateAuthority && CanGrab() && other.gameObject.TryGetComponent<IGrabableBase>(out var component))
		{
			_ = component.IsEnableToGrab;
		}
	}

	public bool CanGrab()
	{
		if (_currentGrabAbilityState == CurrentGrabAbility.CannotGrab)
		{
			return false;
		}
		if (CurrentGrabState != GrabState.Thrown)
		{
			return CurrentGrabState == GrabState.GrabbedMultiple;
		}
		return true;
	}

	public void SetThrown(bool isThrown)
	{
		_isThrown = isThrown;
		GrabState currentGrabState = CurrentGrabState;
		if (currentGrabState != GrabState.Grabbed && currentGrabState != GrabState.GrabbedMultiple && currentGrabState != GrabState.GrabStatic)
		{
			if (isThrown)
			{
				CurrentGrabState = GrabState.Thrown;
				this.OnGrabStateChanged?.Invoke();
			}
			else
			{
				CurrentGrabState = GrabState.Idle;
				this.OnGrabStateChanged?.Invoke();
			}
		}
	}

	public void Release(IGrabableBase grabable)
	{
		if (grabable != null)
		{
			if (SingleGrabbed == grabable)
			{
				SingleGrabbed = null;
			}
			if (MultipleGrabbed.Contains(grabable))
			{
				MultipleGrabbed.Remove(grabable);
			}
			UpdateStateAfterRelease();
		}
	}

	public void ReleaseAll()
	{
		if (SingleGrabbed != null)
		{
			SingleGrabbed.Unjoin(_rigidbody);
			SingleGrabbed = null;
		}
		IGrabableBase[] array = MultipleGrabbed.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Unjoin(_rigidbody);
		}
		MultipleGrabbed.Clear();
		CurrentGrabState = (_isThrown ? GrabState.Thrown : GrabState.Idle);
		this.OnGrabStateChanged?.Invoke();
	}

	private void UpdateStateAfterRelease()
	{
		Debug.LogError("UpdateStateAfterRelease");
		if (MultipleGrabbed.Count > 0)
		{
			foreach (IGrabableBase item in MultipleGrabbed)
			{
				Debug.LogError(item.GameObject.name, item.GameObject);
			}
			Debug.LogError("GrabbedMultiple");
			CurrentGrabState = GrabState.GrabbedMultiple;
			_currentGrabAbilityState = CurrentGrabAbility.CanGrab;
			this.OnGrabStateChanged?.Invoke();
		}
		else if (SingleGrabbed != null)
		{
			Debug.LogError(SingleGrabbed.GameObject.name, SingleGrabbed.GameObject);
			Debug.LogError("Grabbed");
			CurrentGrabState = GrabState.Grabbed;
			_currentGrabAbilityState = CurrentGrabAbility.CannotGrab;
			this.OnGrabStateChanged?.Invoke();
		}
		else
		{
			Debug.LogError("Thrown or idle");
			CurrentGrabState = (_isThrown ? GrabState.Thrown : GrabState.Idle);
			_currentGrabAbilityState = CurrentGrabAbility.CanGrab;
			this.OnGrabStateChanged?.Invoke();
		}
	}

	private void OnDisable()
	{
		NetworkBehaviourUtils.InternalOnDisable(this);
		ReleaseAll();
	}

	private bool ProcessGrabable(IGrabableBase grabable)
	{
		if (grabable == null)
		{
			return false;
		}
		switch (grabable.GrabableType)
		{
		case GrabableType.SmallRigidBody:
			if (SingleGrabbed != null)
			{
				return false;
			}
			if (CurrentGrabState != GrabState.Thrown && CurrentGrabState != GrabState.GrabbedMultiple)
			{
				return false;
			}
			grabable.Join(_rigidbody);
			if (!MultipleGrabbed.Contains(grabable))
			{
				MultipleGrabbed.Add(grabable);
			}
			CurrentGrabState = GrabState.GrabbedMultiple;
			return true;
		case GrabableType.Static:
			if (CurrentGrabState == GrabState.GrabbedMultiple || MultipleGrabbed.Any())
			{
				return false;
			}
			if (SingleGrabbed != null)
			{
				return false;
			}
			grabable.Join(_rigidbody);
			SingleGrabbed = grabable;
			CurrentGrabState = GrabState.GrabStatic;
			return true;
		case GrabableType.BigRigidBody:
			if (CurrentGrabState == GrabState.GrabbedMultiple || MultipleGrabbed.Any())
			{
				return false;
			}
			if (SingleGrabbed != null)
			{
				return false;
			}
			grabable.Join(_rigidbody);
			SingleGrabbed = grabable;
			CurrentGrabState = GrabState.Grabbed;
			return true;
		default:
			return false;
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
