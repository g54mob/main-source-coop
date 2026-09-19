using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Features.SnakeModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SnakeEnemy.Systems
{
	[NetworkBehaviourWeaved(0)]
	public class FindSnakeRandomPositionSystem : MonoSystem
	{
		private const float MIN_SEGMENT_LENGTH_SQR = 1E-08f;

		[SerializeField]
		private SnakeController _snakeController;

		[SerializeField]
		private float _searchRadius = 15f;

		[SerializeField]
		private int _attempts = 8;

		[SerializeField]
		private float _bodyClearance = 0.75f;

		[SerializeField]
		private int _ignoredHeadBodySegments = 2;

		private IMovementContext _movementContext;

		private INavigationService _navigationService;

		private NavMeshPath _path;

		private bool _enabled;

		public override bool IsEnabled => _enabled;

		[Inject]
		public void InjectDependencies(IMovementContext movementContext, INavigationService navigationService)
		{
			_movementContext = movementContext;
			_navigationService = navigationService;
		}

		public override void Enable()
		{
			_enabled = true;
			TryFindPosition();
		}

		public override void Disable()
		{
			_enabled = false;
			Clear();
		}

		public override void Clear()
		{
			_movementContext.NeedToFindTargetPosition = false;
		}

		private void Update()
		{
			if (base.Initialized && _enabled)
			{
				TryFindPosition();
			}
		}

		private void TryFindPosition()
		{
			if (!base.Initialized || !_enabled || !_movementContext.NeedToFindTargetPosition || !_movementContext.NavMeshAgent.isActiveAndEnabled || !_movementContext.NavMeshAgent.isOnNavMesh)
			{
				return;
			}
			int num = Mathf.Max(1, _attempts);
			bool flag = false;
			Vector3 position = Vector3.zero;
			float clearanceSqr = _bodyClearance * _bodyClearance;
			if (_path == null)
			{
				_path = new NavMeshPath();
			}
			for (int i = 0; i < num; i++)
			{
				if (_navigationService.TryGetRandomNavmeshPosition(base.transform.position, _searchRadius, out var position2) && _movementContext.NavMeshAgent.CalculatePath(position2, _path) && _path.status == NavMeshPathStatus.PathComplete)
				{
					if (!flag)
					{
						position = position2;
						flag = true;
					}
					if (!DoesPathCrossBody(_path.corners, clearanceSqr))
					{
						AcceptTarget(position2);
						return;
					}
				}
			}
			if (flag)
			{
				AcceptTarget(position);
			}
		}

		private void AcceptTarget(Vector3 position)
		{
			_movementContext.SetTargetPosition(position);
			_movementContext.NeedToFindTargetPosition = false;
			_movementContext.SetTargetPositionCompleted(isCompleted: false);
		}

		private bool DoesPathCrossBody(Vector3[] corners, float clearanceSqr)
		{
			if (_snakeController == null || corners == null || corners.Length < 2)
			{
				return false;
			}
			IReadOnlyList<Transform> bodyParts = _snakeController.BodyParts;
			if (bodyParts == null || bodyParts.Count < 2)
			{
				return false;
			}
			int num = Mathf.Clamp(_ignoredHeadBodySegments, 0, bodyParts.Count - 1);
			for (int i = 0; i < corners.Length - 1; i++)
			{
				Vector3 a = corners[i];
				Vector3 a2 = corners[i + 1];
				for (int j = num; j < bodyParts.Count - 1; j++)
				{
					Transform transform = bodyParts[j];
					Transform transform2 = bodyParts[j + 1];
					if (!(transform == null) && !(transform2 == null) && SqrDistanceSegmentToSegmentXZ(a, a2, transform.position, transform2.position) <= clearanceSqr)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static float SqrDistanceSegmentToSegmentXZ(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
		{
			float x = a0.x;
			float z = a0.z;
			float num = a1.x - a0.x;
			float num2 = a1.z - a0.z;
			float x2 = b0.x;
			float z2 = b0.z;
			float num3 = b1.x - b0.x;
			float num4 = b1.z - b0.z;
			float num5 = num * num + num2 * num2;
			float num6 = num3 * num3 + num4 * num4;
			if (num5 < 1E-08f && num6 < 1E-08f)
			{
				return SqrDistanceXZ(a0, b0);
			}
			if (num5 < 1E-08f)
			{
				return SqrDistancePointToSegmentXZ(a0, b0, b1);
			}
			if (num6 < 1E-08f)
			{
				return SqrDistancePointToSegmentXZ(b0, a0, a1);
			}
			float num7 = x - x2;
			float num8 = z - z2;
			float num9 = num5;
			float num10 = num6;
			float num11 = num * num3 + num2 * num4;
			float num12 = num * num7 + num2 * num8;
			float num13 = num3 * num7 + num4 * num8;
			float num14 = num9 * num10 - num11 * num11;
			float num15;
			float num16;
			if (num14 < 1E-08f)
			{
				num15 = 0f;
				num16 = Mathf.Clamp01(num13 / num10);
			}
			else
			{
				num15 = Mathf.Clamp01((num11 * num13 - num10 * num12) / num14);
				num16 = (num11 * num15 + num13) / num10;
				if (num16 < 0f)
				{
					num16 = 0f;
					num15 = Mathf.Clamp01((0f - num12) / num9);
				}
				else if (num16 > 1f)
				{
					num16 = 1f;
					num15 = Mathf.Clamp01((num11 - num12) / num9);
				}
			}
			float num17 = x + num * num15 - (x2 + num3 * num16);
			float num18 = z + num2 * num15 - (z2 + num4 * num16);
			return num17 * num17 + num18 * num18;
		}

		private static float SqrDistancePointToSegmentXZ(Vector3 point, Vector3 a, Vector3 b)
		{
			float num = b.x - a.x;
			float num2 = b.z - a.z;
			float num3 = num * num + num2 * num2;
			if (num3 < 1E-08f)
			{
				return SqrDistanceXZ(point, a);
			}
			float num4 = point.x - a.x;
			float num5 = point.z - a.z;
			float num6 = Mathf.Clamp01((num4 * num + num5 * num2) / num3);
			float num7 = a.x + num * num6 - point.x;
			float num8 = a.z + num2 * num6 - point.z;
			return num7 * num7 + num8 * num8;
		}

		private static float SqrDistanceXZ(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return num * num + num2 * num2;
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
