using System;

namespace NomadDrive.Features.Objectives
{
	public abstract class ObjectiveTrigger
	{
		[NonSerialized]
		protected ObjectiveTriggerContext Context;

		[NonSerialized]
		protected Action Fire;

		[NonSerialized]
		protected Action<float> Report;

		[NonSerialized]
		private bool _isActive;

		[NonSerialized]
		private string _lastSourceId;

		public bool IsActive => _isActive;

		public string LastSourceId => _lastSourceId;

		public void Activate(ObjectiveTriggerContext context, Action onFire)
		{
			Activate(context, onFire, null);
		}

		public void Activate(ObjectiveTriggerContext context, Action onFire, Action<float> onRatio)
		{
			if (_isActive)
			{
				Deactivate();
			}
			Context = context;
			Fire = onFire;
			Report = onRatio;
			_lastSourceId = null;
			_isActive = true;
			OnActivate();
		}

		public void Deactivate()
		{
			if (_isActive)
			{
				OnDeactivate();
				_isActive = false;
				Context = null;
				Fire = null;
				Report = null;
				_lastSourceId = null;
			}
		}

		protected void FireWithSource(string sourceId)
		{
			_lastSourceId = sourceId;
			Fire?.Invoke();
		}

		protected abstract void OnActivate();

		protected abstract void OnDeactivate();
	}
}
