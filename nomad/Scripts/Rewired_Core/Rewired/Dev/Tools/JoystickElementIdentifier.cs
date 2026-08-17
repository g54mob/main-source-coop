using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(GUIText))]
	public sealed class JoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool NLtIBSXoVrUQWNJRgpmnDwXVuKMX;

		public void Awake()
		{
			if (!pXBECsFxtoRmYWpuFnBUfqQPBGzZA())
			{
				return;
			}
			if (base.transform.position != Vector3.zero)
			{
				base.transform.position = Vector3.zero;
			}
			if (ReInput.UserData.ConfigVars.alwaysUseUnityInput || ReInput.usingUnityInput)
			{
				NLtIBSXoVrUQWNJRgpmnDwXVuKMX = new pBmaXVMBljaEYtpvxkWtsgYGDkCA();
			}
			else
			{
				Platform platform = UnityTools.platform;
				if (UnityTools.isEditor)
				{
					switch (UnityTools.editorPlatform)
					{
					case EditorPlatform.Windows:
						platform = Platform.Windows;
						break;
					case EditorPlatform.OSX:
						platform = Platform.OSX;
						break;
					case EditorPlatform.Linux:
						platform = Platform.Linux;
						break;
					}
				}
				InputSource inputSourceType = ReInput.primaryInputManager.inputSourceType;
				if (inputSourceType == InputSource.Fallback || inputSourceType == InputSource.Fallback_PreConfigured)
				{
					NLtIBSXoVrUQWNJRgpmnDwXVuKMX = new pBmaXVMBljaEYtpvxkWtsgYGDkCA();
				}
				if (NLtIBSXoVrUQWNJRgpmnDwXVuKMX == null)
				{
					switch (platform)
					{
					case Platform.Windows:
						switch (ReInput.primaryInputManager.inputSourceType)
						{
						case InputSource.DirectInput:
							NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_Windows", "DirectInput") as IElementIdentifierTool;
							break;
						case InputSource.RawInput:
							NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_Windows", "RawInput") as IElementIdentifierTool;
							break;
						}
						break;
					case Platform.WindowsAppStore:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = new pBmaXVMBljaEYtpvxkWtsgYGDkCA();
						break;
					case Platform.WindowsUWP:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("", "WindowsUWP") as IElementIdentifierTool;
						break;
					case Platform.OSX:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_OSX", "OSX") as IElementIdentifierTool;
						break;
					case Platform.Linux:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_Linux", "Linux") as IElementIdentifierTool;
						break;
					case Platform.WebGL:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_WebGL", "WebGL") as IElementIdentifierTool;
						break;
					case Platform.GameCoreXboxOne:
					case Platform.GameCoreScarlett:
						NLtIBSXoVrUQWNJRgpmnDwXVuKMX = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_GameCore", "GameCore") as IElementIdentifierTool;
						break;
					}
				}
			}
			if (NLtIBSXoVrUQWNJRgpmnDwXVuKMX == null)
			{
				Logger.LogWarning("There was an error initializing the platform tool for the current platform and input source. Unity input will be shown instead.");
				NLtIBSXoVrUQWNJRgpmnDwXVuKMX = new pBmaXVMBljaEYtpvxkWtsgYGDkCA();
			}
			NLtIBSXoVrUQWNJRgpmnDwXVuKMX.Initialize(GUIText.CreateLogger(base.gameObject));
		}

		public void Start()
		{
			if (NLtIBSXoVrUQWNJRgpmnDwXVuKMX != null)
			{
				NLtIBSXoVrUQWNJRgpmnDwXVuKMX.Start();
			}
		}

		public void Update()
		{
			if (NLtIBSXoVrUQWNJRgpmnDwXVuKMX != null)
			{
				NLtIBSXoVrUQWNJRgpmnDwXVuKMX.Update();
			}
		}

		public void OnDestroy()
		{
			if (NLtIBSXoVrUQWNJRgpmnDwXVuKMX != null)
			{
				NLtIBSXoVrUQWNJRgpmnDwXVuKMX.OnDestroy();
			}
			NLtIBSXoVrUQWNJRgpmnDwXVuKMX = null;
		}

		private bool pXBECsFxtoRmYWpuFnBUfqQPBGzZA()
		{
			if (!ReInput.isReady)
			{
				Logger.LogError("No active Rewired Input Manager was found in the scene! You must create a Rewired Input Manager for the tool to function.");
				return false;
			}
			return true;
		}
	}
}
