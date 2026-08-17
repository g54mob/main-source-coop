using Ami.BroAudio;
using NomadDrive.Features.Vehicle.Modules.Slots;
using UnityEngine;
using UnityEngine.Events;

namespace NomadDrive.Features.Vehicle.Parts.Engine
{
	public class EngineSlot : VehicleSlot
	{
		public UnityEvent<Engine> onEngineInstalled = new UnityEvent<Engine>();

		public UnityEvent onEngineRemoved = new UnityEvent();

		[Header("Audio")]
		[SerializeField]
		private SoundID attachSound;

		[SerializeField]
		private SoundID detachSound;

		public Engine InstalledEngine { get; set; }

		protected override void OnEnable()
		{
			base.OnEnable();
			base.OnObjectAttached.AddListener(InstallEngine);
			base.OnObjectDetached.AddListener(RemoveEngine);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			base.OnObjectAttached.RemoveListener(InstallEngine);
			base.OnObjectDetached.RemoveListener(RemoveEngine);
		}

		private void InstallEngine()
		{
			if (attachSound.IsValid())
			{
				AudioManager?.PlayOneShot(attachSound, base.transform.position);
			}
			Engine arg = (InstalledEngine = attachedObject.GetComponent<Engine>());
			onEngineInstalled.Invoke(arg);
		}

		private void RemoveEngine()
		{
			if (detachSound.IsValid())
			{
				AudioManager?.PlayOneShot(detachSound, base.transform.position);
			}
			InstalledEngine = null;
			onEngineRemoved.Invoke();
		}
	}
}
