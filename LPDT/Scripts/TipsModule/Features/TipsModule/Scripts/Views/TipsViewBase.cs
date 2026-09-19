using System;
using System.Collections.Generic;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.TipsModule.Scripts.Views
{
	public abstract class TipsViewBase : ViewBehaviour, ITipViewHost
	{
		public CanvasGroup ContainerCanvasGroup;

		public RectTransform ContainerRect;

		public List<TipViewBase> Tips = new List<TipViewBase>();

		public CombineTipViewBase CombineTipPrefab;

		public CombineTipViewBase SplitTipPrefab;

		public AnimationCurve ShowCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public AnimationCurve HideCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		public float AnimationDuration = 0.25f;

		private readonly Queue<TipViewBase> _availableSimpleTips = new Queue<TipViewBase>();

		private readonly Queue<CombineTipViewBase> _availableCombineTips = new Queue<CombineTipViewBase>();

		private readonly Queue<CombineTipViewBase> _availableSplitTips = new Queue<CombineTipViewBase>();

		private readonly List<TipSlotViewBase> _activeSlots = new List<TipSlotViewBase>();

		private bool _poolsInitialized;

		public IReadOnlyList<TipSlotViewBase> ActiveSlots => _activeSlots;

		public TipViewBase RentSimple()
		{
			EnsurePoolsInitialized();
			TipViewBase tipViewBase = ((_availableSimpleTips.Count > 0) ? _availableSimpleTips.Dequeue() : CreateSimpleTip());
			PrepareSlot(tipViewBase);
			_activeSlots.Add(tipViewBase);
			return tipViewBase;
		}

		public CombineTipViewBase RentCombine()
		{
			EnsurePoolsInitialized();
			CombineTipViewBase combineTipViewBase = ((_availableCombineTips.Count > 0) ? _availableCombineTips.Dequeue() : CreateCombineTip());
			PrepareSlot(combineTipViewBase);
			_activeSlots.Add(combineTipViewBase);
			return combineTipViewBase;
		}

		public CombineTipViewBase RentSplit()
		{
			EnsurePoolsInitialized();
			CombineTipViewBase combineTipViewBase = ((_availableSplitTips.Count > 0) ? _availableSplitTips.Dequeue() : CreateSplitTip());
			PrepareSlot(combineTipViewBase);
			_activeSlots.Add(combineTipViewBase);
			return combineTipViewBase;
		}

		public void ReleaseAll()
		{
			EnsurePoolsInitialized();
			for (int i = 0; i < _activeSlots.Count; i++)
			{
				TipSlotViewBase tipSlotViewBase = _activeSlots[i];
				if (!(tipSlotViewBase == null))
				{
					tipSlotViewBase.HideView();
					if (tipSlotViewBase is TipViewBase item)
					{
						_availableSimpleTips.Enqueue(item);
					}
					else if (tipSlotViewBase is SplitTipView item2)
					{
						_availableSplitTips.Enqueue(item2);
					}
					else if (tipSlotViewBase is CombineTipViewBase item3)
					{
						_availableCombineTips.Enqueue(item3);
					}
				}
			}
			_activeSlots.Clear();
		}

		private void EnsurePoolsInitialized()
		{
			if (_poolsInitialized)
			{
				return;
			}
			for (int i = 0; i < Tips.Count; i++)
			{
				TipViewBase tipViewBase = Tips[i];
				if (!(tipViewBase == null))
				{
					tipViewBase.HideView();
					_availableSimpleTips.Enqueue(tipViewBase);
				}
			}
			_poolsInitialized = true;
		}

		private TipViewBase CreateSimpleTip()
		{
			if (Tips.Count == 0 || Tips[0] == null)
			{
				throw new InvalidOperationException("TipsViewBase has no TipViewBase template to instantiate.");
			}
			TipViewBase tipViewBase = UnityEngine.Object.Instantiate(Tips[0], ContainerRect);
			tipViewBase.HideView();
			return tipViewBase;
		}

		private CombineTipViewBase CreateCombineTip()
		{
			if (CombineTipPrefab == null)
			{
				throw new InvalidOperationException("TipsViewBase.CombineTipPrefab is not assigned.");
			}
			CombineTipViewBase combineTipViewBase = UnityEngine.Object.Instantiate(CombineTipPrefab, ContainerRect);
			combineTipViewBase.HideView();
			return combineTipViewBase;
		}

		private CombineTipViewBase CreateSplitTip()
		{
			if (SplitTipPrefab == null)
			{
				throw new InvalidOperationException("TipsViewBase.SplitTipPrefab is not assigned.");
			}
			CombineTipViewBase combineTipViewBase = UnityEngine.Object.Instantiate(SplitTipPrefab, ContainerRect);
			combineTipViewBase.HideView();
			return combineTipViewBase;
		}

		private void PrepareSlot(TipSlotViewBase tipView)
		{
			RectTransform rectTransform = tipView.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.SetParent(ContainerRect, worldPositionStays: false);
				rectTransform.SetAsLastSibling();
			}
			tipView.gameObject.SetActive(value: true);
		}
	}
}
