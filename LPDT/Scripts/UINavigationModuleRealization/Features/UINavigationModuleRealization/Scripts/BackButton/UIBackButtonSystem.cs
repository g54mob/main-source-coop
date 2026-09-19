using System;
using System.Collections.Generic;
using Features.InputModule.Scripts.Generated;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using Zenject;

namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	public class UIBackButtonSystem : IInitializable, IDisposable
	{
		private readonly IInputService _inputService;

		private readonly UIBackButtonModel _model;

		private readonly UIBackButtonPriorityConfiguration _priorityConfiguration;

		private IReadOnlyList<BackButtonProcessorType> _typesByDescendingPriority;

		public UIBackButtonSystem(IInputService inputService, UIBackButtonModel model, UIBackButtonPriorityConfiguration priorityConfiguration)
		{
			_inputService = inputService;
			_model = model;
			_priorityConfiguration = priorityConfiguration;
		}

		public void Initialize()
		{
			_typesByDescendingPriority = _priorityConfiguration.GetTypesByDescendingPriority();
			InputDefaultActions uIBack = _inputService.UIBack;
			uIBack.Performed = (Action)Delegate.Combine(uIBack.Performed, new Action(HandleBack));
		}

		public void Dispose()
		{
			InputDefaultActions uIBack = _inputService.UIBack;
			uIBack.Performed = (Action)Delegate.Remove(uIBack.Performed, new Action(HandleBack));
		}

		private void HandleBack()
		{
			foreach (BackButtonProcessorType item in _typesByDescendingPriority)
			{
				if (!_model.TryGetProcessors(item, out var processors))
				{
					continue;
				}
				for (int num = processors.Count - 1; num >= 0; num--)
				{
					IBackButtonProcessor backButtonProcessor = processors[num];
					if (backButtonProcessor.CanHandleBack())
					{
						backButtonProcessor.OnBack();
						return;
					}
				}
			}
		}
	}
}
