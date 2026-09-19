using System.Collections.Generic;
using UnityEngine;

namespace Features.GrabModule.Scripts
{
	public class SmallObjectPositioner : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> _positions;

		private List<Transform> _freePositions = new List<Transform>();

		private void OnEnable()
		{
			_freePositions.AddRange(_positions);
		}

		public Transform GetFreePosition()
		{
			if (_freePositions.Count == 0)
			{
				return null;
			}
			Transform result = _freePositions[0];
			_freePositions.RemoveAt(0);
			return result;
		}

		public void ReturnPosition(Transform position)
		{
			if (!_freePositions.Contains(position))
			{
				_freePositions.Add(position);
			}
		}
	}
}
