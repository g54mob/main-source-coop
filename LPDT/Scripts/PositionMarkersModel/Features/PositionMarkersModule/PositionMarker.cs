using UnityEngine;
using Zenject;

namespace Features.PositionMarkersModule
{
	public class PositionMarker : MonoBehaviour
	{
		public PositionName positionName;

		private PositionMarkersModel _positionMarkersModel;

		[Inject]
		public void InjectDependencies(PositionMarkersModel positionMarkersModel)
		{
			_positionMarkersModel = positionMarkersModel;
		}

		private void OnEnable()
		{
			_positionMarkersModel.RegisterPositionMarker(this);
		}
	}
}
