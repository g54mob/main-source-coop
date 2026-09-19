using Features.Movement.Scripts;
using Fusion;
using Obi;
using UnityEngine;
using Zenject;

namespace Features.StrechArmsModule.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class ObiArmController : NetworkBehaviour
	{
		[SerializeField]
		private ObiSolver _obiSolver;

		private PlayerCharacterMovableBase _playerCharacterMovableBase;

		private PlayerMovableModel _playerMovableModel;

		[Inject]
		public void InjectDependencies(PlayerMovableModel playerMovableModel)
		{
			_playerMovableModel = playerMovableModel;
		}

		public override void Spawned()
		{
			if (!base.HasStateAuthority)
			{
				_obiSolver.substeps = 5;
			}
			if (base.HasStateAuthority)
			{
				_playerCharacterMovableBase = _playerMovableModel.AllCharacterMovables[base.Object.StateAuthority];
				if (_playerCharacterMovableBase != null)
				{
					_playerCharacterMovableBase.OnChangePosition += _obiSolver.PushSolverParameters;
				}
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			if (base.HasStateAuthority && _playerCharacterMovableBase != null && _obiSolver != null)
			{
				_playerCharacterMovableBase.OnChangePosition -= _obiSolver.PushSolverParameters;
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
