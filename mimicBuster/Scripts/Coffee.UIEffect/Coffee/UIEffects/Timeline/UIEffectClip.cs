using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Coffee.UIEffects.Timeline
{
	public abstract class UIEffectClip : PlayableAsset, ITimelineClipAsset
	{
		public TimelineClip timelineClip { get; set; }

		public ClipCaps clipCaps => ClipCaps.Extrapolation | ClipCaps.Blending;

		private void OnValidate()
		{
		}
	}
	public abstract class UIEffectClip<T> : UIEffectClip where T : UIEffectBehaviour, new()
	{
		[NotKeyable]
		[SerializeField]
		public T m_Data = new T();

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			ScriptPlayable<T> scriptPlayable = ScriptPlayable<T>.Create(graph, m_Data);
			scriptPlayable.GetBehaviour().clip = this;
			return scriptPlayable;
		}
	}
}
