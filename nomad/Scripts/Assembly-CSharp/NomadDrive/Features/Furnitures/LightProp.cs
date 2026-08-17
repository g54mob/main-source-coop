using UnityEngine;

namespace NomadDrive.Features.Furnitures
{
	public class LightProp : MonoBehaviour
	{
		private Light _light;

		private void Awake()
		{
			_light = GetComponent<Light>();
		}

		public void On()
		{
			_light.enabled = true;
		}

		public void Off()
		{
			_light.enabled = false;
		}
	}
}
