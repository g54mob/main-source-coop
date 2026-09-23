using UnityEngine;

namespace Mimicraft.Customization
{
	public interface IPortraitStudio<in TSubject>
	{
		void Dress(TSubject subject);

		Texture2D Shoot();
	}
}
