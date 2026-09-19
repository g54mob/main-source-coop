using System.Collections.Generic;
using Features.InteractModule.Scripts;
using Features.TipsModule.Scripts.Data;
using UnityEngine;

namespace Features.GrabModule.Scripts.Tips
{
	public class ToggleGrabbableTipsReactor : GrabbableTipsReactorBase
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
		private TipType _activateTip = TipType.ActivateItemTip;

		[SerializeField]
		private TipType _deactivateTip = TipType.DeactivateItemTip;

		private IToggleableInteractable _toggleSource;

		private bool _subscribed;

		private readonly List<TipType> _tipsBuffer = new List<TipType>();

		private IToggleableInteractable ToggleSource
		{
			get
			{
				if (_toggleSource != null)
				{
					return _toggleSource;
				}
				_toggleSource = GetComponentInParent<IToggleableInteractable>();
				return _toggleSource;
			}
		}

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
			bool flag = ToggleSource != null && ToggleSource.IsToggledOn;
			_tipsBuffer.Add(flag ? _deactivateTip : _activateTip);
			return _tipsBuffer;
		}

		public override IReadOnlyList<TipType> GetRaycastTips()
		{
			return _raycastTips;
		}

		private void Subscribe()
		{
			if (!_subscribed && ToggleSource != null)
			{
				ToggleSource.OnToggleChanged += base.RaiseTipsChanged;
				_subscribed = true;
			}
		}

		private void Unsubscribe()
		{
			if (_subscribed && ToggleSource != null)
			{
				ToggleSource.OnToggleChanged -= base.RaiseTipsChanged;
				_subscribed = false;
			}
		}
	}
}
