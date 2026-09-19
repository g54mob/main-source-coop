using UnityEngine;

namespace Features.LineArmModule.Scripts.Data
{
	public class TableCenterModel
	{
		private Transform _center;

		public bool TryGetCenter(out Transform center)
		{
			center = _center;
			return center != null;
		}

		public void RegisterCenter(Transform center)
		{
			_center = center;
		}

		public void UnregisterCenter(Transform center)
		{
			if (_center == center)
			{
				_center = null;
			}
		}
	}
}
