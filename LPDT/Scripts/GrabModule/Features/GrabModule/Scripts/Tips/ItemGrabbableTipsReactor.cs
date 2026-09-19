using System.Collections.Generic;
using Features.TipsModule.Scripts.Data;
using UnityEngine;

namespace Features.GrabModule.Scripts.Tips
{
	public class ItemGrabbableTipsReactor : GrabbableTipsReactorBase
	{
		[SerializeField]
		private List<TipType> _tips = new List<TipType>
		{
			TipType.UseTip,
			TipType.ThrowTip,
			TipType.ZoomInOutTip
		};

		[SerializeField]
		private List<TipType> _raycastTips = new List<TipType> { TipType.PickUpTip };

		public override IReadOnlyList<TipType> GetTips()
		{
			return _tips;
		}

		public override IReadOnlyList<TipType> GetRaycastTips()
		{
			return _raycastTips;
		}
	}
}
