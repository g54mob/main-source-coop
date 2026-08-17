using UnityEngine;

namespace Enviro
{
	public interface ILightningEffect
	{
		void CastBolt(Vector3 origin, Vector3 target);
	}
}
