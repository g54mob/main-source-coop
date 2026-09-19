using UnityEngine;
using Zenject;

namespace Features.LevelGatesModule.Scripts
{
	public class RoomTerritoryFootprint : MonoBehaviour
	{
		private const int CORNER_COUNT = 4;

		[SerializeField]
		private Transform[] _corners = new Transform[4];

		[SerializeField]
		private RoomTerritoryKind _kind;

		[SerializeField]
		private bool _overrideHeight;

		[SerializeField]
		private float _minY;

		[SerializeField]
		private float _maxY;

		[SerializeField]
		private float _fallbackHeight = 4f;

		private RoomTerritoryModel _roomTerritoryModel;

		public RoomTerritoryKind Kind => _kind;

		[Inject]
		public void InjectDependencies(RoomTerritoryModel roomTerritoryModel)
		{
			_roomTerritoryModel = roomTerritoryModel;
			if (base.isActiveAndEnabled)
			{
				_roomTerritoryModel.Register(this);
			}
		}

		private void OnEnable()
		{
			if (_roomTerritoryModel != null)
			{
				_roomTerritoryModel.Register(this);
			}
		}

		private void OnDisable()
		{
			if (_roomTerritoryModel != null)
			{
				_roomTerritoryModel.Unregister(this);
			}
		}

		public bool Contains(Vector3 worldPoint)
		{
			if (!TryGetCorners(out var c, out var c2, out var c3, out var c4))
			{
				return false;
			}
			GetHeightRange(c, c2, c3, c4, out var minY, out var maxY);
			if (worldPoint.y < minY || worldPoint.y > maxY)
			{
				return false;
			}
			return IsPointInQuadXZ(worldPoint, c, c2, c3, c4);
		}

		private bool TryGetCorners(out Vector3 c0, out Vector3 c1, out Vector3 c2, out Vector3 c3)
		{
			c0 = (c1 = (c2 = (c3 = default(Vector3))));
			if (_corners == null || _corners.Length < 4)
			{
				return false;
			}
			if (_corners[0] == null || _corners[1] == null || _corners[2] == null || _corners[3] == null)
			{
				return false;
			}
			c0 = _corners[0].position;
			c1 = _corners[1].position;
			c2 = _corners[2].position;
			c3 = _corners[3].position;
			return true;
		}

		private void GetHeightRange(Vector3 c0, Vector3 c1, Vector3 c2, Vector3 c3, out float minY, out float maxY)
		{
			if (_overrideHeight)
			{
				minY = Mathf.Min(_minY, _maxY);
				maxY = Mathf.Max(_minY, _maxY);
				return;
			}
			minY = Mathf.Min(Mathf.Min(c0.y, c1.y), Mathf.Min(c2.y, c3.y));
			maxY = Mathf.Max(Mathf.Max(c0.y, c1.y), Mathf.Max(c2.y, c3.y));
			if (maxY - minY < 0.01f)
			{
				maxY = minY + Mathf.Max(0.01f, _fallbackHeight);
			}
		}

		private static bool IsPointInQuadXZ(Vector3 point, Vector3 c0, Vector3 c1, Vector3 c2, Vector3 c3)
		{
			if (!IsPointInTriangleXZ(point, c0, c1, c2))
			{
				return IsPointInTriangleXZ(point, c0, c2, c3);
			}
			return true;
		}

		private static bool IsPointInTriangleXZ(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
		{
			float num = SignXZ(point, a, b);
			float num2 = SignXZ(point, b, c);
			float num3 = SignXZ(point, c, a);
			bool flag = num < 0f || num2 < 0f || num3 < 0f;
			bool flag2 = num > 0f || num2 > 0f || num3 > 0f;
			return !(flag && flag2);
		}

		private static float SignXZ(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
		}
	}
}
