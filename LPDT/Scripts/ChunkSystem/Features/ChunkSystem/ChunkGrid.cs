using UnityEngine;
using Zenject;

namespace Features.ChunkSystem
{
	public class ChunkGrid : MonoBehaviour
	{
		[SerializeField]
		private string _layer;

		[SerializeField]
		private float _cellSize;

		[SerializeField]
		private Vector3 _originOffset;

		[SerializeField]
		private Vector3Int _gizmoSize = new Vector3Int(10, 1, 10);

		private ChunkSystemModel _chunkSystemModel;

		public string Layer => _layer;

		public float CellSize => _cellSize;

		public Vector3 OriginOffset => _originOffset;

		[Inject]
		public void InjectDependencies(ChunkSystemModel chunkSystemModel)
		{
			_chunkSystemModel = chunkSystemModel;
		}

		private void OnEnable()
		{
			_chunkSystemModel.RegisterGrid(_layer, this);
		}

		private void OnDisable()
		{
			_chunkSystemModel.UnregisterGrid(_layer);
		}

		public Vector3Int WorldToGrid(Vector3 worldPosition)
		{
			Vector3 vector = worldPosition - base.transform.position - _originOffset;
			return new Vector3Int(Mathf.FloorToInt(vector.x / _cellSize), Mathf.FloorToInt(vector.y / _cellSize), Mathf.FloorToInt(vector.z / _cellSize));
		}

		public Vector3 GridToWorld(Vector3Int gridPosition)
		{
			return base.transform.position + _originOffset + new Vector3(((float)gridPosition.x + 0.5f) * _cellSize, ((float)gridPosition.y + 0.5f) * _cellSize, ((float)gridPosition.z + 0.5f) * _cellSize);
		}

		public Vector3 SnapToGrid(Vector3 worldPosition)
		{
			return GridToWorld(WorldToGrid(worldPosition));
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.white;
			Vector3Int vector3Int = new Vector3Int(_gizmoSize.x / 2, _gizmoSize.y / 2, _gizmoSize.z / 2);
			for (int i = -vector3Int.x; i < _gizmoSize.x - vector3Int.x; i++)
			{
				for (int j = -vector3Int.y; j < _gizmoSize.y - vector3Int.y; j++)
				{
					for (int k = -vector3Int.z; k < _gizmoSize.z - vector3Int.z; k++)
					{
						Vector3Int gridPosition = new Vector3Int(i, j, k);
						Gizmos.DrawWireCube(GridToWorld(gridPosition), Vector3.one * _cellSize);
					}
				}
			}
		}
	}
}
