using Ami.BroAudio;
using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Battery
{
	[RequireComponent(typeof(ConditionComponent))]
	public class Battery : AttachableObject
	{
		[SerializeField]
		public BatteryConfig batteryConfig;

		[Header("Audio")]
		[SerializeField]
		private SoundID repairSound;

		protected override void OnRepair()
		{
			base.OnRepair();
			if (repairSound.IsValid())
			{
				NetworkAudioRelay?.PlayOneShot(repairSound, base.transform.position);
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
