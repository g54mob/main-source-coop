using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.UIAnimationsModule.Scripts
{
	public interface IUIAnimationService
	{
		UniTask PlayAsync(RectTransform target, UIAnimationKey key, CancellationToken ct = default(CancellationToken));

		void Play(RectTransform target, UIAnimationKey key);

		void Stop(RectTransform target);

		bool IsPlaying(RectTransform target);
	}
}
