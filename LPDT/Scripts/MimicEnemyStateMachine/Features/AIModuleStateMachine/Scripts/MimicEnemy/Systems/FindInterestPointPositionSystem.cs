using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.MimicEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class FindInterestPointPositionSystem : MonoSystem
	{
		[SerializeField]
		private float _searchRadius;

		[SerializeField]
		private int _historySize = 3;

		[SerializeField]
		private float _minSelectionDistance = 5f;

		[SerializeField]
		private float _distanceWeightPower = 1f;

		[SerializeField]
		private float _behindDirectionWeight = 0.2f;

		private readonly Queue<MimicInterestPoint> _recentPoints = new Queue<MimicInterestPoint>();

		private INavigationService _navigationService;

		private MimicEnemyContext _context;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		private void InjectDependencies(MimicEnemyContext context, INavigationService navigationService)
		{
			_context = context;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_enabled = true;
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_context.NeedToFindTargetPosition = false;
			_recentPoints.Clear();
		}

		private void Update()
		{
			if (!base.Initialized || !_enabled || !_context.NeedToFindTargetPosition)
			{
				return;
			}
			if (_context.MimicInterestPoints == null || _context.MimicInterestPoints.Count == 0)
			{
				FindRandomPos();
				return;
			}
			MimicInterestPoint mimicInterestPoint = SelectInterestPoint();
			if (mimicInterestPoint == null)
			{
				FindRandomPos();
				return;
			}
			if (!_navigationService.TryResolveReachablePoint(_context.NavMeshAgent, mimicInterestPoint.transform.position, out var reachable, out var _))
			{
				FindRandomPos();
				return;
			}
			RememberPoint(mimicInterestPoint);
			_context.CurrentTargetInterestPriority = mimicInterestPoint.PointPriority;
			_context.SetTargetPosition(reachable);
			_context.NeedToFindTargetPosition = false;
			_context.SetTargetPositionCompleted(isCompleted: false);
		}

		private MimicInterestPoint SelectInterestPoint()
		{
			Vector3 origin = base.transform.position;
			int maxPriority = _context.MimicInterestPoints.Where((MimicInterestPoint point) => point != null).Max((MimicInterestPoint point) => point.PointPriority);
			List<MimicInterestPoint> list = _context.MimicInterestPoints.Where((MimicInterestPoint point) => point != null && point.PointPriority == maxPriority).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			List<MimicInterestPoint> list2 = list.Where((MimicInterestPoint point) => !_recentPoints.Contains(point)).ToList();
			if (list2.Count > 0)
			{
				list = list2;
			}
			List<MimicInterestPoint> list3 = list.Where((MimicInterestPoint point) => Vector3.Distance(origin, point.transform.position) >= _minSelectionDistance).ToList();
			if (list3.Count > 0)
			{
				list = list3;
			}
			return PickWeighted(list, origin);
		}

		private MimicInterestPoint PickWeighted(List<MimicInterestPoint> candidates, Vector3 origin)
		{
			if (candidates.Count == 1)
			{
				return candidates[0];
			}
			Vector3 vector = ((_context.NavMeshAgent.velocity.sqrMagnitude > 0.01f) ? _context.NavMeshAgent.velocity.normalized : base.transform.forward);
			float[] array = new float[candidates.Count];
			float num = 0f;
			for (int i = 0; i < candidates.Count; i++)
			{
				Vector3 vector2 = candidates[i].transform.position - origin;
				float magnitude = vector2.magnitude;
				float num2 = Mathf.Pow(Mathf.Max(magnitude, 0.01f), _distanceWeightPower);
				float t = (Vector3.Dot((magnitude > 0.01f) ? (vector2 / magnitude) : vector, vector) + 1f) * 0.5f;
				float num3 = Mathf.Lerp(_behindDirectionWeight, 1f, t);
				array[i] = num2 * num3;
				num += array[i];
			}
			if (num <= 0f)
			{
				return candidates[Random.Range(0, candidates.Count)];
			}
			float num4 = Random.value * num;
			for (int j = 0; j < candidates.Count; j++)
			{
				num4 -= array[j];
				if (num4 <= 0f)
				{
					return candidates[j];
				}
			}
			return candidates[candidates.Count - 1];
		}

		private void RememberPoint(MimicInterestPoint point)
		{
			_recentPoints.Enqueue(point);
			while (_recentPoints.Count > Mathf.Max(1, _historySize))
			{
				_recentPoints.Dequeue();
			}
		}

		private void FindRandomPos()
		{
			if (_navigationService.TryGetRandomNavmeshPosition(base.transform.position, _searchRadius, out var position) && _navigationService.TryResolveReachablePoint(_context.NavMeshAgent, position, out var reachable, out var _))
			{
				_context.SetTargetPosition(reachable);
				_context.NeedToFindTargetPosition = false;
				_context.SetTargetPositionCompleted(isCompleted: false);
			}
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
