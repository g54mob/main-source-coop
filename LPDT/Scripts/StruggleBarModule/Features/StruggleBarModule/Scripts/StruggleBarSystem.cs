using System;
using Features.InputModule.Scripts.Generated;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using UnityEngine;
using Zenject;

namespace Features.StruggleBarModule.Scripts
{
	public class StruggleBarSystem : IInitializable, ITickable, IDisposable
	{
		private readonly StruggleBarService _struggleBarService;

		private readonly StruggleBarModel _struggleBarModel;

		private readonly IInputService _inputService;

		private bool _isInputSubscribed;

		public StruggleBarSystem(StruggleBarService struggleBarService, StruggleBarModel struggleBarModel, IInputService inputService)
		{
			_struggleBarService = struggleBarService;
			_struggleBarModel = struggleBarModel;
			_inputService = inputService;
		}

		public void Initialize()
		{
			_struggleBarModel.OnActiveChanged += OnActiveChanged;
		}

		public void Tick()
		{
			if (_struggleBarModel.IsActive)
			{
				_struggleBarService.TickDrain(Time.deltaTime);
			}
		}

		public void Dispose()
		{
			_struggleBarModel.OnActiveChanged -= OnActiveChanged;
			UnsubscribeInput();
		}

		private void OnActiveChanged(bool isActive)
		{
			if (isActive)
			{
				SubscribeInput();
			}
			else
			{
				UnsubscribeInput();
			}
		}

		private void OnJumpPerformed()
		{
			_struggleBarService.ApplyBoost();
		}

		private void SubscribeInput()
		{
			if (!_isInputSubscribed)
			{
				InputDefaultActions gameplayApply = _inputService.GameplayApply;
				gameplayApply.Performed = (Action)Delegate.Combine(gameplayApply.Performed, new Action(OnJumpPerformed));
				_isInputSubscribed = true;
			}
		}

		private void UnsubscribeInput()
		{
			if (_isInputSubscribed)
			{
				InputDefaultActions gameplayApply = _inputService.GameplayApply;
				gameplayApply.Performed = (Action)Delegate.Remove(gameplayApply.Performed, new Action(OnJumpPerformed));
				_isInputSubscribed = false;
			}
		}
	}
}
