using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwToggleScript")]
	[AddComponentMenu("CW/Paint in 3D/CW Toggle Script")]
	public class CwToggleScript : MonoBehaviour
	{
		[SerializeField]
		private KeyCode key = KeyCode.Mouse0;

		[SerializeField]
		private MonoBehaviour target;

		[SerializeField]
		protected bool storeStates;

		public KeyCode Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		public MonoBehaviour Target
		{
			get
			{
				return target;
			}
			set
			{
				target = value;
			}
		}

		public bool StoreStates
		{
			get
			{
				return storeStates;
			}
			set
			{
				storeStates = value;
			}
		}

		protected virtual void Update()
		{
			if (!(target != null))
			{
				return;
			}
			if (CwInput.GetKeyIsHeld(key))
			{
				if (storeStates && !target.enabled)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				target.enabled = true;
			}
			else
			{
				target.enabled = false;
			}
		}
	}
}
