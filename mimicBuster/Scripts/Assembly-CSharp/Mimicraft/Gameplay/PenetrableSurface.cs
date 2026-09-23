using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class PenetrableSurface : MonoBehaviour
	{
		[Tooltip("Geçirmezlik: bu yüzeyi delip geçmek için merminin harcadığı delme gücü. Silahın Penetration değeri (kalan gücü) bundan küçükse mermi burada durur. 0 = her mermi bedelsiz geçer.")]
		[SerializeField]
		[Min(0f)]
		private float resistance = 1f;

		[Tooltip("Geçtikten sonra merminin hasarının ne kadarı kalır. 1 = hasar düşmez, 0.5 = yarıya iner. Birden fazla yüzey geçilirse çarpanlar üst üste biner.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float damageRetained = 0.7f;

		public float Resistance => resistance;

		public float DamageRetained => damageRetained;
	}
}
