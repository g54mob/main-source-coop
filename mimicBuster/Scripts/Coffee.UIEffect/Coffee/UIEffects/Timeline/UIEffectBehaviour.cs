using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Coffee.UIEffects.Timeline
{
	[Serializable]
	public abstract class UIEffectBehaviour : PlayableBehaviour
	{
		public bool m_Tween;

		public AnimationCurve m_Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public UIEffectClip clip { get; set; }
	}
}
