using System;
using UnityEngine;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialUIDataHolder
	{
		private RectTransform _keysTipHolder;

		private RectTransform _ctrlTipHolder;

		private RectTransform _mouseUpTipHolder;

		private RectTransform _turnAroundTipHolder;

		public RectTransform KeysTipHolder
		{
			get
			{
				return _keysTipHolder;
			}
			set
			{
				_keysTipHolder = value;
				this.OnKeysTipHolderChanged?.Invoke(_keysTipHolder);
			}
		}

		public RectTransform CtrlTipHolder
		{
			get
			{
				return _ctrlTipHolder;
			}
			set
			{
				_ctrlTipHolder = value;
				this.OnCtrlTipHolderChanged?.Invoke(_ctrlTipHolder);
			}
		}

		public RectTransform MouseUpTipHolder
		{
			get
			{
				return _mouseUpTipHolder;
			}
			set
			{
				_mouseUpTipHolder = value;
				this.OnMouseUpTipHolderChanged?.Invoke(_mouseUpTipHolder);
			}
		}

		public RectTransform TurnAroundTipHolder
		{
			get
			{
				return _turnAroundTipHolder;
			}
			set
			{
				_turnAroundTipHolder = value;
				this.OnTurnAroundTipHolderChanged?.Invoke(_turnAroundTipHolder);
			}
		}

		public event Action<RectTransform> OnKeysTipHolderChanged;

		public event Action<RectTransform> OnCtrlTipHolderChanged;

		public event Action<RectTransform> OnMouseUpTipHolderChanged;

		public event Action<RectTransform> OnTurnAroundTipHolderChanged;
	}
}
