using System;
using System.Collections.Generic;
using Features.GameUpdaterModule;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.RumModule.Scripts.Views
{
	[PublicAPI]
	public class TemporalRumBuffItemPresenter : PresenterBehaviour<TemporalRumBuffItemViewBase>
	{
		private readonly IGameUpdater _gameUpdater;

		private readonly RumIconsConfiguration _rumIconsConfiguration;

		private readonly List<TemporalRumData> _sources = new List<TemporalRumData>();

		private int _lastSeconds = int.MinValue;

		private bool _isTicking;

		public TemporalRumBuffItemPresenter(IGameUpdater gameUpdater, RumIconsConfiguration rumIconsConfiguration)
		{
			_gameUpdater = gameUpdater;
			_rumIconsConfiguration = rumIconsConfiguration;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			SubscribeTick();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			UnsubscribeTick();
		}

		public void SetSources(RumType rumType, IReadOnlyList<TemporalRumData> sources)
		{
			_sources.Clear();
			for (int i = 0; i < sources.Count; i++)
			{
				_sources.Add(sources[i]);
			}
			base.View.SetRumIcon(_rumIconsConfiguration.GetIcon(rumType));
			base.View.SetStackCount(_sources.Count);
			_lastSeconds = int.MinValue;
			Tick();
		}

		public void SetSiblingIndex(int index)
		{
			base.View.transform.SetSiblingIndex(index);
		}

		public void DestroyView()
		{
			UnsubscribeTick();
			UnityEngine.Object.Destroy(base.View.gameObject);
		}

		private void SubscribeTick()
		{
			if (!_isTicking)
			{
				_gameUpdater.OnUpdate += Tick;
				_isTicking = true;
			}
		}

		private void UnsubscribeTick()
		{
			if (_isTicking)
			{
				_gameUpdater.OnUpdate -= Tick;
				_isTicking = false;
			}
		}

		private void Tick()
		{
			if (_sources.Count == 0)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < _sources.Count; i++)
			{
				TemporalRumData temporalRumData = _sources[i];
				if (temporalRumData != null && temporalRumData.RumData != null)
				{
					float num2 = temporalRumData.RumData.Duration - temporalRumData.Duration;
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
			int num3 = Math.Max(0, Mathf.CeilToInt(num));
			if (num3 != _lastSeconds)
			{
				_lastSeconds = num3;
				base.View.SetRemainingSeconds(num3);
			}
		}
	}
}
