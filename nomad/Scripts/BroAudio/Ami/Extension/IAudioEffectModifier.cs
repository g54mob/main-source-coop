using UnityEngine;

namespace Ami.Extension
{
	public interface IAudioEffectModifier
	{
		void TransferValueTo<T>(T target) where T : Behaviour;
	}
}
