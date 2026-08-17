using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(GUIText))]
	public sealed class RawInputJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool wWscDfNTHZBeHiXeZnGMsfjBaakY;

		public void Awake()
		{
			if (aAartVHPHvnuMVCKHpMYWrmDkOdL())
			{
				if (base.transform.position != Vector3.zero)
				{
					base.transform.position = Vector3.zero;
				}
				wWscDfNTHZBeHiXeZnGMsfjBaakY = oHJeRBTQaTBdrfBrOnpadPHlLcKE.fvDUdFpPcZbUobJWmHveyztzrwzpA("Rewired_Windows", "RawInput") as IElementIdentifierTool;
				if (wWscDfNTHZBeHiXeZnGMsfjBaakY == null)
				{
					Logger.LogError("RawInput Tool could not be initialized! Make sure the correct platform mode is chosen in Unity's Build Settings.");
				}
				else
				{
					wWscDfNTHZBeHiXeZnGMsfjBaakY.Initialize(GUIText.CreateLogger(base.gameObject));
				}
			}
		}

		public void Start()
		{
			if (wWscDfNTHZBeHiXeZnGMsfjBaakY != null)
			{
				wWscDfNTHZBeHiXeZnGMsfjBaakY.Start();
			}
		}

		public void Update()
		{
			if (wWscDfNTHZBeHiXeZnGMsfjBaakY != null)
			{
				wWscDfNTHZBeHiXeZnGMsfjBaakY.Update();
			}
		}

		public void OnDestroy()
		{
			if (wWscDfNTHZBeHiXeZnGMsfjBaakY != null)
			{
				wWscDfNTHZBeHiXeZnGMsfjBaakY.OnDestroy();
			}
			wWscDfNTHZBeHiXeZnGMsfjBaakY = null;
		}

		private bool aAartVHPHvnuMVCKHpMYWrmDkOdL()
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
