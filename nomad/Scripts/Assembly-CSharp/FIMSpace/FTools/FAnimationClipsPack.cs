using UnityEngine;

namespace FIMSpace.FTools
{
	[CreateAssetMenu(fileName = "Animation Clips Pack", menuName = "FImpossible Creations/Utilities/Animation Clips Pack", order = 400)]
	public class FAnimationClipsPack : FContainerBase
	{
		public override void AddAsset(Object obj)
		{
			if (!(obj == null))
			{
				bool flag = false;
				if (obj is AnimationClip)
				{
					flag = true;
				}
				if (!flag)
				{
					Debug.Log("[Animation Clips Pack] Wrong asset type! You're trying to add '" + obj.GetType()?.ToString() + "'!'");
				}
				else
				{
					base.AddAsset(obj);
				}
			}
		}
	}
}
