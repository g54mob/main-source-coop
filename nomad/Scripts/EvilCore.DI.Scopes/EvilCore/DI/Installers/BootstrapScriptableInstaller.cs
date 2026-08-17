using EvilCore.Audio;
using EvilCore.Inputs;
using EvilCore.Particles;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EvilCore.DI.Installers
{
	[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "EvilCore/DI/Scriptable Installer")]
	public class BootstrapScriptableInstaller : ScriptableObject, IInstaller
	{
		[Header("Input Actions")]
		[SerializeField]
		private InputActionPromptsDatabase inputActionsDatabase;

		[Header("Particles")]
		[SerializeField]
		private ParticleDatabase particleDatabase;

		[Header("Attachables")]
		[SerializeField]
		private AttachableAudioFallbackConfig attachableAudioFallback;

		public void Install(IContainerBuilder builder)
		{
			if (inputActionsDatabase != null)
			{
				builder.RegisterInstance(inputActionsDatabase);
			}
			if (particleDatabase != null)
			{
				builder.RegisterInstance(particleDatabase);
			}
			if (attachableAudioFallback != null)
			{
				builder.RegisterInstance(attachableAudioFallback);
			}
		}
	}
}
