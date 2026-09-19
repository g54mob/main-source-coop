using UnityEngine;

namespace Features.TutorialModule.Scripts.UIGlow
{
	internal class GlowableUIView : GlowableUIViewBase
	{
		[SerializeField]
		private GlowableObject _glowableObject;

		public override IGlowableObject GlowableObject => _glowableObject;
	}
}
