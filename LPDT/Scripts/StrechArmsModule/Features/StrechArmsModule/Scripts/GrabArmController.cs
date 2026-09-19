using System;
using System.Collections;
using Features.GrabModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Fusion;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class GrabArmController : NetworkBehaviour, IArmController
	{
		private const float HOLD_TIME = 0.2f;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		public GrabController GrabController;

		[SerializeField]
		private Rigidbody _endRigidbody;

		[SerializeField]
		private StaticColorChangerByPlayerRef _colorChanger;

		[SerializeField]
		private VisualController _visualController;

		private ArmStartsModel _armStartsModel;

		private bool _isArmRegistered;

		private bool _isThrowed;

		private IInputService _inputService;

		public bool _isThrowing;

		[WeaverGenerated]
		[SerializeField]
		[DefaultForProperty("ArmOrientation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Arm _ArmOrientation;

		public bool EnemyGrabbed;

		public Transform EnemyTransform;

		public bool _isGrabbing;

		public bool _canGrab = true;

		private ArmStateModel _armStateModel;

		private Coroutine _ButtonHoldingCoroutine;

		private bool _isArmHolding;

		private bool _isButtonHolding;

		[Networked]
		[NetworkedWeaved(0, 1)]
		public unsafe Arm ArmOrientation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GrabArmController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Arm*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing GrabArmController.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Arm*)((byte*)Ptr + 0) = value;
			}
		}

		public VisualController VisualController => _visualController;

		public event Action OnHited;

		[Inject]
		private void InjectDependencies(ArmStartsModel armStartsModel, IInputService inputService, ArmStateModel armStateModel)
		{
			_armStateModel = armStateModel;
			_inputService = inputService;
			_armStartsModel = armStartsModel;
		}

		public void Start()
		{
			GrabController.OnGrabStateChanged += GrabChanged;
			switch (ArmOrientation)
			{
			case Arm.Left:
			{
				InputDefaultActions grabItem2 = _inputService.GrabItem;
				grabItem2.Started = (Action)Delegate.Combine(grabItem2.Started, new Action(StartCheckingForRightArmHold));
				break;
			}
			case Arm.Right:
			{
				InputDefaultActions grabItem = _inputService.GrabItem;
				grabItem.Started = (Action)Delegate.Combine(grabItem.Started, new Action(StartCheckingForRightArmHold));
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			case Arm.None:
			case Arm.Mid:
				break;
			}
			InputDefaultActions dropAllFromArms = _inputService.DropAllFromArms;
			dropAllFromArms.Performed = (Action)Delegate.Combine(dropAllFromArms.Performed, new Action(OnDropAll));
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			InputDefaultActions grabItem = _inputService.GrabItem;
			grabItem.Started = (Action)Delegate.Remove(grabItem.Started, new Action(StartCheckingForRightArmHold));
			InputDefaultActions grabItem2 = _inputService.GrabItem;
			grabItem2.Started = (Action)Delegate.Remove(grabItem2.Started, new Action(StartCheckingForRightArmHold));
			InputDefaultActions grabItem3 = _inputService.GrabItem;
			grabItem3.Canceled = (Action)Delegate.Remove(grabItem3.Canceled, new Action(OnRightArmHoldCanceled));
			InputDefaultActions grabItem4 = _inputService.GrabItem;
			grabItem4.Canceled = (Action)Delegate.Remove(grabItem4.Canceled, new Action(OnRightArmHoldCanceled));
			InputDefaultActions dropAllFromArms = _inputService.DropAllFromArms;
			dropAllFromArms.Performed = (Action)Delegate.Remove(dropAllFromArms.Performed, new Action(OnDropAll));
			if (_ButtonHoldingCoroutine != null)
			{
				StopCoroutine(_ButtonHoldingCoroutine);
			}
		}

		private void GrabChanged()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			GrabState currentGrabState = GrabController.CurrentGrabState;
			if (currentGrabState == GrabState.Grabbed || currentGrabState == GrabState.GrabbedMultiple || currentGrabState == GrabState.GrabStatic)
			{
				_isGrabbing = true;
				if (GrabController.SingleGrabbed != null)
				{
					Debug.LogError("Grabbed " + GrabController.SingleGrabbed.GameObject.name, GrabController.SingleGrabbed.GameObject);
				}
				else
				{
					foreach (IGrabableBase item in GrabController.MultipleGrabbed)
					{
						Debug.LogError("Grabbed " + item.GameObject.name, item.GameObject);
					}
				}
				_armStateModel.SetArmState(ArmOrientation, ArmState.Grabbing);
			}
			else if (GrabController.CurrentGrabState == GrabState.Thrown)
			{
				_isGrabbing = false;
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
			}
			else if (GrabController.CurrentGrabState == GrabState.Idle)
			{
				_isGrabbing = false;
				_armStateModel.SetArmState(ArmOrientation, ArmState.ReadyToThrow);
			}
		}

		public void SetEnemyGrabbed(bool value, Transform staticTransform)
		{
			EnemyTransform = staticTransform;
			EnemyGrabbed = value;
			_armStateModel.OnEnemyGrabbedChangedInvoke(ArmOrientation, value);
		}

		private void OnArmDrag()
		{
			if (base.HasStateAuthority && !_isArmHolding && _canGrab && _isThrowed)
			{
				OnArmDragSetState();
				Debug.LogError("OnArmGrab end " + _isGrabbing);
			}
		}

		private void OnDropAll()
		{
			if (base.HasStateAuthority && _isGrabbing)
			{
				_isGrabbing = false;
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
				GrabController.ReleaseAll();
				if (_ButtonHoldingCoroutine != null)
				{
					StopCoroutine(_ButtonHoldingCoroutine);
				}
			}
		}

		private void OnArmDragSetState()
		{
			if (_isGrabbing)
			{
				_isGrabbing = false;
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
				GrabController.ReleaseAll();
				return;
			}
			_isGrabbing = true;
			GrabController.TryGrab();
			GrabState currentGrabState = GrabController.CurrentGrabState;
			if (currentGrabState == GrabState.Grabbed || currentGrabState == GrabState.GrabbedMultiple || currentGrabState == GrabState.GrabStatic)
			{
				if (GrabController.SingleGrabbed != null)
				{
					Debug.LogError("Grabbed " + GrabController.SingleGrabbed.GameObject.name, GrabController.SingleGrabbed.GameObject);
				}
				else
				{
					foreach (IGrabableBase item in GrabController.MultipleGrabbed)
					{
						Debug.LogError("Grabbed " + item.GameObject.name, item.GameObject);
					}
				}
				_armStateModel.SetArmState(ArmOrientation, ArmState.Grabbing);
			}
			else if (GrabController.CurrentGrabState == GrabState.Thrown)
			{
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
			}
		}

		public void SetArmOrientation(Arm armOrientation)
		{
			ArmOrientation = armOrientation;
		}

		private void Update()
		{
			if (ArmOrientation != Arm.None && !(base.Object == null) && !_isArmRegistered)
			{
				_isArmRegistered = true;
				_armStartsModel.AddArmEnd(base.Object.StateAuthority, new ArmEndEntity(base.transform, _colorChanger), ArmOrientation);
			}
		}

		public void SetIsThrow(bool isThrow)
		{
			_isThrowed = isThrow;
			_canGrab = isThrow;
			GrabController.SetThrown(isThrow);
		}

		public void TryGrab()
		{
			if (!base.HasStateAuthority)
			{
				return;
			}
			GrabController.TryGrab();
			GrabState currentGrabState = GrabController.CurrentGrabState;
			if (currentGrabState == GrabState.Grabbed || currentGrabState == GrabState.GrabbedMultiple || currentGrabState == GrabState.GrabStatic)
			{
				_isGrabbing = true;
				if (GrabController.SingleGrabbed != null)
				{
					Debug.LogError("Grabbed " + GrabController.SingleGrabbed.GameObject.name, GrabController.SingleGrabbed.GameObject);
				}
				else
				{
					foreach (IGrabableBase item in GrabController.MultipleGrabbed)
					{
						Debug.LogError("Grabbed " + item.GameObject.name, item.GameObject);
					}
				}
				_armStateModel.SetArmState(ArmOrientation, ArmState.Grabbing);
			}
			else if (GrabController.CurrentGrabState == GrabState.Thrown)
			{
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
			}
		}

		public void UngrabAll()
		{
			if (base.HasStateAuthority)
			{
				GrabController.ReleaseAll();
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
			}
		}

		private void OnCollisionEnter(Collision other)
		{
			if (!base.HasStateAuthority || (_layerMask.value & (1 << other.gameObject.layer)) != 0 || !_isThrowing)
			{
				return;
			}
			this.OnHited?.Invoke();
			if (GrabController.CurrentGrabState == GrabState.Grabbed)
			{
				return;
			}
			_isGrabbing = true;
			GrabController.TryGrab();
			GrabState currentGrabState = GrabController.CurrentGrabState;
			if (currentGrabState == GrabState.Grabbed || currentGrabState == GrabState.GrabbedMultiple || currentGrabState == GrabState.GrabStatic)
			{
				if (GrabController.SingleGrabbed != null)
				{
					Debug.LogError("Grabbed " + GrabController.SingleGrabbed.GameObject.name, GrabController.SingleGrabbed.GameObject);
				}
				else
				{
					foreach (IGrabableBase item in GrabController.MultipleGrabbed)
					{
						Debug.LogError("Grabbed " + item.GameObject.name, item.GameObject);
					}
				}
				_armStateModel.SetArmState(ArmOrientation, ArmState.Grabbing);
			}
			else if (GrabController.CurrentGrabState == GrabState.Thrown)
			{
				_armStateModel.SetArmState(ArmOrientation, ArmState.CanGrabbing);
			}
		}

		public void StartCheckingForRightArmHold()
		{
			if (!EnemyGrabbed && !_isArmHolding && _canGrab && _isThrowed)
			{
				_isButtonHolding = true;
				_isArmHolding = false;
				if (ArmOrientation == Arm.Right)
				{
					InputDefaultActions grabItem = _inputService.GrabItem;
					grabItem.Canceled = (Action)Delegate.Combine(grabItem.Canceled, new Action(OnRightArmHoldCanceled));
				}
				else if (ArmOrientation == Arm.Left)
				{
					InputDefaultActions grabItem2 = _inputService.GrabItem;
					grabItem2.Canceled = (Action)Delegate.Combine(grabItem2.Canceled, new Action(OnRightArmHoldCanceled));
				}
				_ButtonHoldingCoroutine = StartCoroutine(WaitForRightButtonHold());
			}
		}

		private void OnRightArmHoldCanceled()
		{
			if (ArmOrientation == Arm.Right)
			{
				InputDefaultActions grabItem = _inputService.GrabItem;
				grabItem.Canceled = (Action)Delegate.Remove(grabItem.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem2 = _inputService.GrabItem;
				grabItem2.Performed = (Action)Delegate.Remove(grabItem2.Performed, new Action(OnRightArmHoldCanceled));
			}
			else if (ArmOrientation == Arm.Left)
			{
				InputDefaultActions grabItem3 = _inputService.GrabItem;
				grabItem3.Canceled = (Action)Delegate.Remove(grabItem3.Canceled, new Action(OnRightArmHoldCanceled));
				InputDefaultActions grabItem4 = _inputService.GrabItem;
				grabItem4.Performed = (Action)Delegate.Remove(grabItem4.Performed, new Action(OnRightArmHoldCanceled));
			}
			_isButtonHolding = false;
			_isArmHolding = false;
		}

		public IEnumerator WaitForRightButtonHold()
		{
			float elapsedTime = 0f;
			while (elapsedTime < 0.2f)
			{
				if (!_isButtonHolding)
				{
					_isArmHolding = false;
					OnArmDrag();
					yield break;
				}
				elapsedTime += Time.deltaTime;
				yield return null;
			}
			_isArmHolding = true;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			ArmOrientation = _ArmOrientation;
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			_ArmOrientation = ArmOrientation;
		}
	}
}
