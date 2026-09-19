using System;
using Fusion;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(1)]
	public class IdlePoseRegistrar : NetworkBehaviour
	{
		[WeaverGenerated]
		[DefaultForProperty("ArmOrientation", 0, 1)]
		[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
		private Arm _ArmOrientation;

		private IdlePoseForArmModel _idlePoseForArmModel;

		[Networked]
		[NetworkedWeaved(0, 1)]
		private unsafe Arm ArmOrientation
		{
			get
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing IdlePoseRegistrar.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				return *(Arm*)((byte*)Ptr + 0);
			}
			set
			{
				if (Ptr == null)
				{
					throw new InvalidOperationException("Error when accessing IdlePoseRegistrar.ArmOrientation. Networked properties can only be accessed when Spawned() has been called.");
				}
				*(Arm*)((byte*)Ptr + 0) = value;
			}
		}

		[Inject]
		public void InjectDependencies(IdlePoseForArmModel idlePoseForArmModel)
		{
			_idlePoseForArmModel = idlePoseForArmModel;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			_idlePoseForArmModel.UnregisterIdlePose(ArmOrientation, base.Object.InputAuthority);
		}

		public void SetArmOrientation(Arm armOrientation)
		{
			ArmOrientation = armOrientation;
		}

		public void RegisterPose()
		{
			_idlePoseForArmModel.RegisterIdlePose(ArmOrientation, base.transform, base.Object.StateAuthority);
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
