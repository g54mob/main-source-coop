using UnityEngine;

namespace Zorro.UI.Effects
{
	[ExecuteInEditMode]
	public class FadeInEffect : EffectBase
	{
		[Range(0f, 1f)]
		public float Time;

		private static readonly int FadeTime = Shader.PropertyToID("_FadeTime");

		protected override void Update()
		{
			base.Update();
			m_material.SetFloat(FadeTime, Time * 1.4f);
			base.Update();
		}
	}
}
