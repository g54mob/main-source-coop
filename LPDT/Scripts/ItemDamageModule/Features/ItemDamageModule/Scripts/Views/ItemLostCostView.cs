using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts.Views
{
	public class ItemLostCostView : ItemLostCostViewBase
	{
		[SerializeField]
		private List<LostCostAnimation> _lostCostAnimations;

		public override List<LostCostAnimation> LostCostAnimations => _lostCostAnimations;

		public override void PlayLostCostAnimation(float lostCost, Vector3 hitPosition, Camera currentCamera)
		{
			if (_lostCostAnimations != null && _lostCostAnimations.Count != 0)
			{
				LostCostAnimation lostCostAnimation = _lostCostAnimations.FirstOrDefault((LostCostAnimation anim) => !anim.IsActive);
				if (lostCostAnimation == null)
				{
					lostCostAnimation = _lostCostAnimations[0];
				}
				lostCostAnimation.Activate(lostCost, hitPosition, currentCamera, (currentCamera.transform.position - hitPosition).sqrMagnitude);
			}
		}
	}
}
