using System.Collections.Generic;
using Features.InteractModule.Scripts;
using Features.TipsModule.Scripts.Data;
using UnityEngine;

namespace Features.GrabModule.Scripts.Tips
{
	public class DrinkingGrabbableTipsReactor : GrabbableTipsReactorBase
	{
		[SerializeField]
		private List<TipType> _baseTips = new List<TipType>
		{
			TipType.ThrowTip,
			TipType.ZoomInOutTip
		};

		[SerializeField]
		private List<TipType> _raycastTips = new List<TipType> { TipType.PickUpTip };

		[SerializeField]
		private TipType _startDrinkingTip = TipType.StartDrinkingTip;

		[SerializeField]
		private TipType _drinkTip = TipType.DrinkTip;

		[SerializeField]
		private DrinkingInteractableBase _drinkingSource;

		private bool _subscribed;

		private readonly List<TipType> _tipsBuffer = new List<TipType>();

		private void OnEnable()
		{
			Subscribe();
		}

		private void OnDisable()
		{
			Unsubscribe();
		}

		public override IReadOnlyList<TipType> GetTips()
		{
			Subscribe();
			_tipsBuffer.Clear();
			_tipsBuffer.AddRange(_baseTips);
			if (_drinkingSource == null || _drinkingSource.IsConsumed)
			{
				return _tipsBuffer;
			}
			bool isToggledOn = _drinkingSource.IsToggledOn;
			_tipsBuffer.Add(isToggledOn ? _drinkTip : _startDrinkingTip);
			return _tipsBuffer;
		}

		public override IReadOnlyList<TipType> GetRaycastTips()
		{
			return _raycastTips;
		}

		private void Subscribe()
		{
			if (!_subscribed && !(_drinkingSource == null))
			{
				_drinkingSource.OnToggleChanged += base.RaiseTipsChanged;
				_drinkingSource.OnConsumed += base.RaiseTipsChanged;
				_subscribed = true;
			}
		}

		private void Unsubscribe()
		{
			if (_subscribed && !(_drinkingSource == null))
			{
				_drinkingSource.OnToggleChanged -= base.RaiseTipsChanged;
				_drinkingSource.OnConsumed -= base.RaiseTipsChanged;
				_subscribed = false;
			}
		}
	}
}
