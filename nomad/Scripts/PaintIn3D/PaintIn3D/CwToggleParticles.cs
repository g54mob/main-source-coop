using CW.Common;
using PaintCore;
using UnityEngine;

namespace PaintIn3D
{
	[HelpURL("https://carloswilkes.com/Documentation/PaintIn3D#CwToggleParticles")]
	[AddComponentMenu("CW/Paint in 3D/CW Toggle Particles")]
	public class CwToggleParticles : MonoBehaviour
	{
		[SerializeField]
		private LayerMask guiLayers = 32;

		[SerializeField]
		private KeyCode key = KeyCode.Mouse0;

		[SerializeField]
		private ParticleSystem target;

		[SerializeField]
		protected bool storeStates = true;

		public LayerMask GuiLayers
		{
			get
			{
				return guiLayers;
			}
			set
			{
				guiLayers = value;
			}
		}

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

		public ParticleSystem Target
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

		protected virtual void LateUpdate()
		{
			if (!(target != null))
			{
				return;
			}
			if (key < KeyCode.Mouse0)
			{
				_ = key;
				_ = 329;
			}
			if (CwInput.GetKeyIsHeld(key))
			{
				if (storeStates && !target.isPlaying)
				{
					CwStateManager.PotentiallyStoreAllStates();
				}
				target.Play();
			}
			else
			{
				target.Stop();
			}
		}
	}
}
