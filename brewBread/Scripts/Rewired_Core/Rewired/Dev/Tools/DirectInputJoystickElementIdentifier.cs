using Rewired.Interfaces;
using Rewired.Internal;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	[RequireComponent(typeof(Rewired.Internal.GUIText))]
	[AddComponentMenu("")]
	public sealed class DirectInputJoystickElementIdentifier : MonoBehaviour
	{
		private IElementIdentifierTool oJtDqAekwAkQhTEtjUoPKgSVmvNk;

		public void Awake()
		{
			if (sCILfbGEfOHidUNVEWwWkeBCprAL())
			{
				if (base.transform.position != Vector3.zero)
				{
					base.transform.position = Vector3.zero;
				}
				oJtDqAekwAkQhTEtjUoPKgSVmvNk = YRnaBTcvnvJguXFoejldCIMIRFTrb.lgqGPLYsThRluihoOcGxFVBbaQcDA("Rewired_Windows", "DirectInput") as IElementIdentifierTool;
				if (oJtDqAekwAkQhTEtjUoPKgSVmvNk == null)
				{
					Logger.LogError("DirectInput Tool could not be initialized! Make sure the correct platform mode is chosen in Unity's Build Settings.");
				}
				else
				{
					oJtDqAekwAkQhTEtjUoPKgSVmvNk.Initialize(Rewired.Internal.GUIText.CreateLogger(base.gameObject));
				}
			}
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

		private bool sCILfbGEfOHidUNVEWwWkeBCprAL()
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
