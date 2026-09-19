using System;
using System.Collections.Generic;

namespace Features.UINavigationModuleRealization.Scripts.BackButton
{
	public class UIBackButtonModel
	{
		private readonly Dictionary<BackButtonProcessorType, List<IBackButtonProcessor>> _processorsByType = new Dictionary<BackButtonProcessorType, List<IBackButtonProcessor>>();

		public event Action OnProcessorsChanged;

		public bool TryGetProcessors(BackButtonProcessorType type, out IReadOnlyList<IBackButtonProcessor> processors)
		{
			if (_processorsByType.TryGetValue(type, out var value))
			{
				processors = value;
				return true;
			}
			processors = Array.Empty<IBackButtonProcessor>();
			return false;
		}

		public void Register(IBackButtonProcessor processor)
		{
			if (processor != null)
			{
				if (!_processorsByType.TryGetValue(processor.Type, out var value))
				{
					value = new List<IBackButtonProcessor>();
					_processorsByType[processor.Type] = value;
				}
				if (!value.Contains(processor))
				{
					value.Add(processor);
					this.OnProcessorsChanged?.Invoke();
				}
			}
		}

		public void Unregister(IBackButtonProcessor processor)
		{
			if (processor != null && _processorsByType.TryGetValue(processor.Type, out var value) && value.Remove(processor))
			{
				if (value.Count == 0)
				{
					_processorsByType.Remove(processor.Type);
				}
				this.OnProcessorsChanged?.Invoke();
			}
		}
	}
}
