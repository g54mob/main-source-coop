using System;

namespace Features.NetworkServices.Scripts.InterestManagement
{
	public class ActiveAOILayerModel
	{
		private AOILayer _activeAOILayer;

		public AOILayer ActiveAOILayer
		{
			get
			{
				return _activeAOILayer;
			}
			internal set
			{
				_activeAOILayer = value;
				this.OnActiveAOILayerUpdated?.Invoke(value);
			}
		}

		public event Action<AOILayer> OnActiveAOILayerUpdated;
	}
}
