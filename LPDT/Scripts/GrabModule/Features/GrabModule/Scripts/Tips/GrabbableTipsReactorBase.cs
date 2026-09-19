using System;
using System.Collections.Generic;
using Features.TipsModule.Scripts.Data;
using UnityEngine;

namespace Features.GrabModule.Scripts.Tips
{
	public abstract class GrabbableTipsReactorBase : MonoBehaviour
	{
		[SerializeField]
		private bool _repaintAllTipsOnChange;

		public bool RepaintAllTipsOnChange => _repaintAllTipsOnChange;

		public event Action OnTipsChanged;

		public abstract IReadOnlyList<TipType> GetTips();

		public abstract IReadOnlyList<TipType> GetRaycastTips();

		protected void RaiseTipsChanged()
		{
			this.OnTipsChanged?.Invoke();
		}
	}
}
