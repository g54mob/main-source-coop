using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(Rewired.Internal.GUIText))]
	public sealed class JoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool oJtDqAekwAkQhTEtjUoPKgSVmvNk;

		public void Awake()
		{
			if (!ajhXJbgnqLHNDTtpxLqCuAgGrnVb())
			{
				return;
			}
			if (base.transform.position != Vector3.zero)
			{
				base.transform.position = Vector3.zero;
			}
			if (ReInput.UserData.ConfigVars.alwaysUseUnityInput || ReInput.usingUnityInput)
			{
				oJtDqAekwAkQhTEtjUoPKgSVmvNk = new BGvdFRgcyHDHHtVOFGqCyMzedLldb();
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
					oJtDqAekwAkQhTEtjUoPKgSVmvNk = new BGvdFRgcyHDHHtVOFGqCyMzedLldb();
				}
				if (oJtDqAekwAkQhTEtjUoPKgSVmvNk == null)
				{
					switch (platform)
					{
					case Platform.Windows:
						switch (ReInput.primaryInputManager.inputSourceType)
						{
						case InputSource.DirectInput:
							oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_Windows", "DirectInput") as IElementIdentifierTool;
							break;
						case InputSource.RawInput:
							oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_Windows", "RawInput") as IElementIdentifierTool;
							break;
						}
						break;
					case Platform.WindowsAppStore:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = new BGvdFRgcyHDHHtVOFGqCyMzedLldb();
						break;
					case Platform.WindowsUWP:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("", "WindowsUWP") as IElementIdentifierTool;
						break;
					case Platform.OSX:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_OSX", "OSX") as IElementIdentifierTool;
						break;
					case Platform.Linux:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_Linux", "Linux") as IElementIdentifierTool;
						break;
					case Platform.WebGL:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_WebGL", "WebGL") as IElementIdentifierTool;
						break;
					case Platform.Stadia:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_Stadia", "Stadia") as IElementIdentifierTool;
						break;
					case Platform.GameCoreXboxOne:
					case Platform.GameCoreScarlett:
						oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_GameCore", "GameCore") as IElementIdentifierTool;
						break;
					}
				}
			}
			if (oJtDqAekwAkQhTEtjUoPKgSVmvNk == null)
			{
				Logger.LogWarning("There was an error initializing the platform tool for the current platform and input source. Unity input will be shown instead.");
				oJtDqAekwAkQhTEtjUoPKgSVmvNk = new BGvdFRgcyHDHHtVOFGqCyMzedLldb();
			}
			oJtDqAekwAkQhTEtjUoPKgSVmvNk.Initialize(Rewired.Internal.GUIText.CreateLogger(base.gameObject));
		}

		public void Start()
		{
			if (oJtDqAekwAkQhTEtjUoPKgSVmvNk != null)
			{
				oJtDqAekwAkQhTEtjUoPKgSVmvNk.Start();
			}
		}

		public void Update()
		{
			if (oJtDqAekwAkQhTEtjUoPKgSVmvNk != null)
			{
				oJtDqAekwAkQhTEtjUoPKgSVmvNk.Update();
			}
		}

		public void OnDestroy()
		{
			if (oJtDqAekwAkQhTEtjUoPKgSVmvNk != null)
			{
				oJtDqAekwAkQhTEtjUoPKgSVmvNk.OnDestroy();
			}
			oJtDqAekwAkQhTEtjUoPKgSVmvNk = null;
		}

		private bool ajhXJbgnqLHNDTtpxLqCuAgGrnVb()
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
