using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.GrabModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class CrabGrabbedByPlayerTrackSystem : MonoSystem
	{
		[SerializeField]
		private List<SimplePointGrabable> _grabables;

		private CrabEnemyContext _context;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		private void InjectDependencies(CrabEnemyContext crabEnemyContext)
		{
			_context = crabEnemyContext;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		public override void Clear()
		{
			_context.IsGrabbedByPlayer = false;
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled)
			{
				_context.IsGrabbedByPlayer = IsAnyGrabableHeldByPlayer();
			}
		}

		private bool IsAnyGrabableHeldByPlayer()
		{
			foreach (SimplePointGrabable grabable in _grabables)
			{
				if (grabable.GrabbedByPlayersCount > 0)
				{
					return true;
				}
			}
			return false;
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
