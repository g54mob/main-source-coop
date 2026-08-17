using System;
using EvilCore.DI.Core;
using EvilCore.Networking;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.VoiceChat.DI
{
	[Serializable]
	public class VoiceChatInstaller : MonoInstaller
	{
		[SerializeField]
		private VoiceChatManager voiceChatManagerReference;

		public override void Install(IContainerBuilder builder)
		{
			RegisterIfNotNull(builder, voiceChatManagerReference, delegate(RegistrationBuilder c)
			{
				c.As<IVoiceChatManager>().As<IVoiceDeviceController>();
			});
		}
	}
}
