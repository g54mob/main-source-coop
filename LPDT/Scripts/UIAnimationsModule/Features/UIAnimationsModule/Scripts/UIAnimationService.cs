using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Features.UIAnimationsModule.Scripts
{
	public class UIAnimationService : IUIAnimationService
	{
		private readonly IInstantiator _instantiator;

		private UIAnimationPlaybackController _controller;

		private UIAnimationPlaybackController Controller => _controller ?? (_controller = _instantiator.Instantiate<UIAnimationPlaybackController>());

		public UIAnimationService(IInstantiator instantiator)
		{
			_instantiator = instantiator;
		}

		public UniTask PlayAsync(RectTransform target, UIAnimationKey key, CancellationToken ct = default(CancellationToken))
		{
			return Controller.PlayAsync(target, key, ct);
		}

		public void Play(RectTransform target, UIAnimationKey key)
		{
			Controller.Play(target, key);
		}

		public void Stop(RectTransform target)
		{
			Controller.Stop(target);
		}

		public bool IsPlaying(RectTransform target)
		{
			return Controller.IsPlaying(target);
		}
	}
}
