using Ami.Extension;

namespace Ami.BroAudio.Runtime
{
	public struct FadeData
	{
		private Ease _baseEase;

		private Ease _nextEase;

		public const float UseClipSetting = -1f;

		public const float Immediate = 0f;

		public float Base { get; set; }

		public float Next { get; set; }

		public FadeData(Ease ease, Ease nextEase)
		{
			Base = -1f;
			_baseEase = ease;
			Next = -1f;
			_nextEase = nextEase;
		}

		public void SetEase(Ease ease)
		{
			_nextEase = ease;
			_baseEase = ease;
		}

		public bool TryGetOrConsumeOverride(out float fade, out Ease ease)
		{
			if (Next >= 0f)
			{
				fade = Next;
				Next = -1f;
				ease = _nextEase;
				return true;
			}
			fade = Base;
			ease = _baseEase;
			return fade >= 0f;
		}
	}
}
