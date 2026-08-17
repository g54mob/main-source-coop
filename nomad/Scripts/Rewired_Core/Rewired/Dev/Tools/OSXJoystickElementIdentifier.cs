using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(GUIText))]
	public sealed class OSXJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool sbXoXWPFVQQnOybdVJcmArZseFBA;

		public void Awake()
		{
			if (cuiawfmQbceDVoVztHQnXRrXacuj())
			{
				if (base.transform.position != Vector3.zero)
				{
					base.transform.position = Vector3.zero;
				}
				sbXoXWPFVQQnOybdVJcmArZseFBA = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_OSX", "OSX") as IElementIdentifierTool;
				if (sbXoXWPFVQQnOybdVJcmArZseFBA == null)
				{
					Logger.LogError("OSX Tool could not be initialized! Make sure the correct platform mode is chosen in Unity's Build Settings.");
				}
				else
				{
					sbXoXWPFVQQnOybdVJcmArZseFBA.Initialize(GUIText.CreateLogger(base.gameObject));
				}
			}
		}

		public void Start()
		{
			if (sbXoXWPFVQQnOybdVJcmArZseFBA != null)
			{
				sbXoXWPFVQQnOybdVJcmArZseFBA.Start();
			}
		}

		public void Update()
		{
			if (sbXoXWPFVQQnOybdVJcmArZseFBA != null)
			{
				sbXoXWPFVQQnOybdVJcmArZseFBA.Update();
			}
		}

		public void OnDestroy()
		{
			if (sbXoXWPFVQQnOybdVJcmArZseFBA != null)
			{
				sbXoXWPFVQQnOybdVJcmArZseFBA.OnDestroy();
			}
			sbXoXWPFVQQnOybdVJcmArZseFBA = null;
		}

		private bool cuiawfmQbceDVoVztHQnXRrXacuj()
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
