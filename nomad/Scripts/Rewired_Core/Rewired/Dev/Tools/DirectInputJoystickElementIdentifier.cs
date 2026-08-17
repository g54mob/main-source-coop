using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(GUIText))]
	public sealed class DirectInputJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool mNUiGKfBdnNuTAamiNgkrcsWhdYT;

		public void Awake()
		{
			if (ahKBCWetVQYLLigDtFBTLvmCBFge())
			{
				if (base.transform.position != Vector3.zero)
				{
					base.transform.position = Vector3.zero;
				}
				mNUiGKfBdnNuTAamiNgkrcsWhdYT = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_Windows", "DirectInput") as IElementIdentifierTool;
				if (mNUiGKfBdnNuTAamiNgkrcsWhdYT == null)
				{
					Logger.LogError("DirectInput Tool could not be initialized! Make sure the correct platform mode is chosen in Unity's Build Settings.");
				}
				else
				{
					mNUiGKfBdnNuTAamiNgkrcsWhdYT.Initialize(GUIText.CreateLogger(base.gameObject));
				}
			}
		}

		public void Start()
		{
			if (mNUiGKfBdnNuTAamiNgkrcsWhdYT != null)
			{
				mNUiGKfBdnNuTAamiNgkrcsWhdYT.Start();
			}
		}

		public void Update()
		{
			if (mNUiGKfBdnNuTAamiNgkrcsWhdYT != null)
			{
				mNUiGKfBdnNuTAamiNgkrcsWhdYT.Update();
			}
		}

		public void OnDestroy()
		{
			if (mNUiGKfBdnNuTAamiNgkrcsWhdYT != null)
			{
				mNUiGKfBdnNuTAamiNgkrcsWhdYT.OnDestroy();
			}
			mNUiGKfBdnNuTAamiNgkrcsWhdYT = null;
		}

		private bool ahKBCWetVQYLLigDtFBTLvmCBFge()
		{
			InputManager_Base[] array = (InputManager_Base[])Object.FindObjectsOfType(typeof(InputManager_Base));
			if (array == null || array.Length == 0)
			{
				Logger.LogError("No active Rewired Input Manager was found in the scene! You must create a Rewired Input Manager for the tool to function.");
				return false;
			}
			return true;
		}
	}
}
