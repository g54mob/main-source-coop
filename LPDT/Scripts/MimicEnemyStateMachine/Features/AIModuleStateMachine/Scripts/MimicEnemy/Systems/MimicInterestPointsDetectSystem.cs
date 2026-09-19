using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class MimicInterestPointsDetectSystem : MonoSystem
	{
		[SerializeField]
		private float _detectRadius = 15f;

		[SerializeField]
		private float _detectIntervalSeconds = 1f;

		private MimicInterestPointsModel _mimicInterestPointsModel;

		private MimicEnemyContext _context;

		private float _timer;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, MimicInterestPointsModel mimicInterestPointsModel)
		{
			_context = context;
			_mimicInterestPointsModel = mimicInterestPointsModel;
		}

		public override void Enable()
		{
			_enabled = true;
			_timer = 0f;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_context.SetMimicInterestPoints(new List<MimicInterestPoint>());
			_timer = 0f;
		}

		private void Update()
		{
			if (base.Initialized && _enabled)
			{
				_timer += Time.deltaTime;
				if (!(_timer < _detectIntervalSeconds))
				{
					_timer = 0f;
					DetectInterestPoints();
				}
			}
		}

		private void DetectInterestPoints()
		{
			Vector3 origin = base.transform.position;
			List<MimicInterestPoint> mimicInterestPoints = (from point in _mimicInterestPointsModel.NavigableInterestPoints
				where point != null
				where Vector3.Distance(origin, point.transform.position) <= _detectRadius
				orderby Vector3.Distance(origin, point.transform.position), point.PointPriority descending
				select point).ToList();
			_context.SetMimicInterestPoints(mimicInterestPoints);
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
