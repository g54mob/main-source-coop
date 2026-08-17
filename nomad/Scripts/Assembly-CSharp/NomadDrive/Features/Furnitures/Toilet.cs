using Cysharp.Threading.Tasks;
using EvilCore;
using NomadDrive.Features.Interaction;
using UnityEngine.Events;

namespace NomadDrive.Features.Furnitures
{
	public class Toilet : Interactable, IInitialize
	{
		private BasicInteraction _basicInteraction;

		private bool _isFlushing;

		public UnityEvent OnToiletTryFlush { get; } = new UnityEvent();

		public bool IsInitialized { get; set; }

		public void Init()
		{
			_basicInteraction = GetComponent<BasicInteraction>();
			_isFlushing = false;
			IsInitialized = true;
		}

		private async void TryFlush()
		{
			if (!_isFlushing)
			{
				OnToiletTryFlush.Invoke();
				await TryFlushAsync();
			}
		}

		public async UniTask TryFlushAsync()
		{
			StartFlush();
			await UniTask.Delay(3000);
			EndFlush();
			await UniTask.CompletedTask;
		}

		private void StartFlush()
		{
			SetInteractionAvailability(newValue: false);
			_isFlushing = true;
		}

		private void EndFlush()
		{
			_isFlushing = false;
			SetBasicInteraction(TryFlush, "@interaction.flush");
			SetInteractionAvailability(newValue: true);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
