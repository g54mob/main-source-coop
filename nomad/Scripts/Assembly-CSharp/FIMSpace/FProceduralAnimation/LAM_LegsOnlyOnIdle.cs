using UnityEngine;

namespace FIMSpace.FProceduralAnimation
{
	public class LAM_LegsOnlyOnIdle : LegsAnimatorControlModuleBase
	{
		public override void OnUpdate(LegsAnimator.LegsAnimatorCustomModuleHelper helper)
		{
			if (base.LA.IsMoving)
			{
				base.LA.LegsAnimatorBlend = Mathf.MoveTowards(base.LA.LegsAnimatorBlend, 0.001f, base.LA.DeltaTime * 5f);
			}
			else
			{
				base.LA.LegsAnimatorBlend = Mathf.MoveTowards(base.LA.LegsAnimatorBlend, 1f, base.LA.DeltaTime * 8f);
			}
		}
	}
}
