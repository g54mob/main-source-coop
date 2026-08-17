using UnityEngine;

namespace Ami.BroAudio
{
	public class Volume : PropertyAttribute
	{
		public bool CanBoost;

		public Volume()
		{
			CanBoost = true;
		}

		public Volume(bool canBoost)
		{
			CanBoost = canBoost;
		}
	}
}
