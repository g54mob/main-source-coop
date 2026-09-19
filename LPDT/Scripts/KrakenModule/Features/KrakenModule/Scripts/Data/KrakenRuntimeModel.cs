using System;

namespace Features.KrakenModule.Scripts.Data
{
	public class KrakenRuntimeModel
	{
		public KrakenController Controller { get; private set; }

		public event Action<KrakenController> OnControllerChanged;

		public bool TryGetController(out KrakenController controller)
		{
			controller = Controller;
			return controller != null;
		}

		public void SetController(KrakenController controller)
		{
			if (!(Controller == controller))
			{
				Controller = controller;
				this.OnControllerChanged?.Invoke(Controller);
			}
		}

		public void ClearController(KrakenController controller)
		{
			if (!(Controller != controller))
			{
				Controller = null;
				this.OnControllerChanged?.Invoke(null);
			}
		}
	}
}
