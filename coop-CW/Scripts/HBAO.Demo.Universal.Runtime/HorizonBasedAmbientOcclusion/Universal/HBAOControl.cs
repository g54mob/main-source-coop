using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace HorizonBasedAmbientOcclusion.Universal
{
	public class HBAOControl : MonoBehaviour
	{
		public VolumeProfile postProcessProfile;

		public Slider aoRadiusSlider;

		private bool m_HbaoDisplayed = true;

		public void Start()
		{
			postProcessProfile.TryGet<HBAO>(out var component);
			if (component != null)
			{
				component.EnableHBAO(enable: true);
				component.SetDebugMode(HBAO.DebugMode.Disabled);
				component.SetAoRadius(aoRadiusSlider.value);
			}
		}

		public void ToggleHBAO()
		{
			postProcessProfile.TryGet<HBAO>(out var component);
			if (component != null)
			{
				m_HbaoDisplayed = !m_HbaoDisplayed;
				component.EnableHBAO(m_HbaoDisplayed);
			}
		}

		public void ToggleShowAO()
		{
			postProcessProfile.TryGet<HBAO>(out var component);
			if (component != null)
			{
				if (component.GetDebugMode() != HBAO.DebugMode.Disabled)
				{
					component.SetDebugMode(HBAO.DebugMode.Disabled);
				}
				else
				{
					component.SetDebugMode(HBAO.DebugMode.AOOnly);
				}
			}
		}

		public void UpdateAoRadius()
		{
			postProcessProfile.TryGet<HBAO>(out var component);
			if (component != null)
			{
				component.SetAoRadius(aoRadiusSlider.value);
			}
		}
	}
}
