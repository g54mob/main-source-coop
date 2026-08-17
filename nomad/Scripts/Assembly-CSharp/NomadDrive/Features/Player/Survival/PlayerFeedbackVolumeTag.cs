using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.Player.Survival
{
	[RequireComponent(typeof(Volume))]
	public class PlayerFeedbackVolumeTag : MonoBehaviour
	{
		private Volume _volume;

		public Volume Volume
		{
			get
			{
				if (_volume == null)
				{
					_volume = GetComponent<Volume>();
				}
				return _volume;
			}
		}
	}
}
