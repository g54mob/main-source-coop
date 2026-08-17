using EvilCore.EvilPack.EvilLogger;
using EvilCore.Networking;
using UnityEngine;

namespace NomadDrive.Features.Inputs
{
	public class VoiceChatInputHandler : MonoBehaviour
	{
		private IVoiceChatManager _voiceChatManager;

		private void Awake()
		{
			_voiceChatManager = GetComponent<IVoiceChatManager>();
		}

		private void Update()
		{
			if (_voiceChatManager == null)
			{
				EvilLogger.LogError("Voice Chat Manager is null!", "Update", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Inputs\\VoiceChatInputHandler.cs", 22);
			}
			if (BaseInputs.IsVoiceChatButtonDown())
			{
				_voiceChatManager.ToggleMute();
			}
		}
	}
}
