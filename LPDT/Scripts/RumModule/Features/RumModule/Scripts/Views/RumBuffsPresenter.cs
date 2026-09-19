using System.Collections.Generic;
using Features.ViewSystemModule.Scripts.Windows;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.RumModule.Scripts.Views
{
	[PublicAPI]
	public class RumBuffsPresenter : PresenterBehaviour<RumBuffsViewBase>
	{
		private readonly RumStatsRewardModel _model;

		private readonly RumBuffsViewModel _viewModel;

		private readonly DiContainer _container;

		private readonly SessionWindow _sessionWindow;

		private readonly Dictionary<RumType, TemporalRumBuffItemPresenter> _temporalItems = new Dictionary<RumType, TemporalRumBuffItemPresenter>();

		private readonly Dictionary<RumType, PermanentRumBuffItemPresenter> _permanentItems = new Dictionary<RumType, PermanentRumBuffItemPresenter>();

		private readonly List<RumType> _temporalOrder = new List<RumType>();

		private readonly List<RumType> _permanentOrder = new List<RumType>();

		private readonly List<RumType> _scratchRumTypes = new List<RumType>();

		private readonly Dictionary<RumType, List<TemporalRumData>> _scratchTemporal = new Dictionary<RumType, List<TemporalRumData>>();

		private readonly Dictionary<RumType, int> _scratchPermanent = new Dictionary<RumType, int>();

		public RumBuffsPresenter(RumStatsRewardModel model, RumBuffsViewModel viewModel, DiContainer container, SessionWindow sessionWindow)
		{
			_model = model;
			_viewModel = viewModel;
			_container = container;
			_sessionWindow = sessionWindow;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			_model.OnTemporalRumDataChanged += RefreshTemporal;
			_model.OnRumDataChanged += RefreshPermanent;
			_viewModel.OnBuffsVisibilityChanged += ChangeVisibility;
			RefreshTemporal();
			RefreshPermanent();
		}

		protected override void OnViewDisabled()
		{
			base.OnViewDisabled();
			_model.OnTemporalRumDataChanged -= RefreshTemporal;
			_model.OnRumDataChanged -= RefreshPermanent;
			_viewModel.OnBuffsVisibilityChanged -= ChangeVisibility;
			ClearAll();
		}

		private void ChangeVisibility(bool isVisible)
		{
			base.View.SetVisible(isVisible);
		}

		private void RefreshTemporal()
		{
			_scratchRumTypes.Clear();
			_scratchTemporal.Clear();
			IReadOnlyList<TemporalRumData> activeTemporalRumsData = _model.ActiveTemporalRumsData;
			for (int i = 0; i < activeTemporalRumsData.Count; i++)
			{
				TemporalRumData temporalRumData = activeTemporalRumsData[i];
				if (temporalRumData != null && temporalRumData.RumData != null && !temporalRumData.RumData.HideFromBuffUi)
				{
					RumType rumType = temporalRumData.RumData.RumType;
					if (!_scratchTemporal.TryGetValue(rumType, out var value))
					{
						value = new List<TemporalRumData>();
						_scratchTemporal[rumType] = value;
						_scratchRumTypes.Add(rumType);
					}
					value.Add(temporalRumData);
				}
			}
			RemoveStaleTemporal(_scratchTemporal);
			_temporalOrder.Clear();
			for (int j = 0; j < _scratchRumTypes.Count; j++)
			{
				RumType rumType2 = _scratchRumTypes[j];
				if (!_temporalItems.TryGetValue(rumType2, out var value2))
				{
					value2 = SpawnTemporalItem(rumType2);
				}
				value2.SetSources(rumType2, _scratchTemporal[rumType2]);
				_temporalOrder.Add(rumType2);
			}
			ApplySiblingOrder();
		}

		private void RefreshPermanent()
		{
			_scratchRumTypes.Clear();
			_scratchPermanent.Clear();
			IReadOnlyList<RumData> activeRumsData = _model.ActiveRumsData;
			for (int i = 0; i < activeRumsData.Count; i++)
			{
				RumData rumData = activeRumsData[i];
				if (rumData != null && !rumData.HideFromBuffUi)
				{
					RumType rumType = rumData.RumType;
					if (!_scratchPermanent.ContainsKey(rumType))
					{
						_scratchPermanent[rumType] = 0;
						_scratchRumTypes.Add(rumType);
					}
					_scratchPermanent[rumType]++;
				}
			}
			RemoveStalePermanent(_scratchPermanent);
			_permanentOrder.Clear();
			for (int j = 0; j < _scratchRumTypes.Count; j++)
			{
				RumType rumType2 = _scratchRumTypes[j];
				if (!_permanentItems.TryGetValue(rumType2, out var value))
				{
					value = SpawnPermanentItem(rumType2);
				}
				value.SetData(rumType2, _scratchPermanent[rumType2]);
				_permanentOrder.Add(rumType2);
			}
			ApplySiblingOrder();
		}

		private TemporalRumBuffItemPresenter SpawnTemporalItem(RumType type)
		{
			TemporalRumBuffItemViewBase temporalRumBuffItemViewBase = _container.InstantiatePrefabForComponent<TemporalRumBuffItemViewBase>(base.View.TemporalItemPrefab);
			_sessionWindow.AddView(temporalRumBuffItemViewBase.transform, worldPositionStays: false);
			temporalRumBuffItemViewBase.transform.SetParent(base.View.ItemsContainer, worldPositionStays: false);
			TemporalRumBuffItemPresenter presenterForView = _sessionWindow.GetPresenterForView<TemporalRumBuffItemPresenter>(temporalRumBuffItemViewBase);
			_temporalItems[type] = presenterForView;
			return presenterForView;
		}

		private PermanentRumBuffItemPresenter SpawnPermanentItem(RumType type)
		{
			PermanentRumBuffItemViewBase permanentRumBuffItemViewBase = _container.InstantiatePrefabForComponent<PermanentRumBuffItemViewBase>(base.View.PermanentItemPrefab);
			_sessionWindow.AddView(permanentRumBuffItemViewBase.transform, worldPositionStays: false);
			permanentRumBuffItemViewBase.transform.SetParent(base.View.ItemsContainer, worldPositionStays: false);
			PermanentRumBuffItemPresenter presenterForView = _sessionWindow.GetPresenterForView<PermanentRumBuffItemPresenter>(permanentRumBuffItemViewBase);
			_permanentItems[type] = presenterForView;
			return presenterForView;
		}

		private void RemoveStaleTemporal(Dictionary<RumType, List<TemporalRumData>> stillPresent)
		{
			List<RumType> list = null;
			foreach (RumType key2 in _temporalItems.Keys)
			{
				if (!stillPresent.ContainsKey(key2))
				{
					if (list == null)
					{
						list = new List<RumType>();
					}
					list.Add(key2);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					RumType key = list[i];
					_temporalItems[key].DestroyView();
					_temporalItems.Remove(key);
				}
			}
		}

		private void RemoveStalePermanent(Dictionary<RumType, int> stillPresent)
		{
			List<RumType> list = null;
			foreach (RumType key2 in _permanentItems.Keys)
			{
				if (!stillPresent.ContainsKey(key2))
				{
					if (list == null)
					{
						list = new List<RumType>();
					}
					list.Add(key2);
				}
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					RumType key = list[i];
					_permanentItems[key].DestroyView();
					_permanentItems.Remove(key);
				}
			}
		}

		private void ApplySiblingOrder()
		{
			int num = 0;
			for (int i = 0; i < _temporalOrder.Count; i++)
			{
				if (_temporalItems.TryGetValue(_temporalOrder[i], out var value))
				{
					value.SetSiblingIndex(num++);
				}
			}
			for (int j = 0; j < _permanentOrder.Count; j++)
			{
				if (_permanentItems.TryGetValue(_permanentOrder[j], out var value2))
				{
					value2.SetSiblingIndex(num++);
				}
			}
		}

		private void ClearAll()
		{
			foreach (KeyValuePair<RumType, TemporalRumBuffItemPresenter> temporalItem in _temporalItems)
			{
				temporalItem.Value.DestroyView();
			}
			_temporalItems.Clear();
			_temporalOrder.Clear();
			foreach (KeyValuePair<RumType, PermanentRumBuffItemPresenter> permanentItem in _permanentItems)
			{
				permanentItem.Value.DestroyView();
			}
			_permanentItems.Clear();
			_permanentOrder.Clear();
		}
	}
}
