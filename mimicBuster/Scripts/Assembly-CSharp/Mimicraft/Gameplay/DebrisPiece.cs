using UnityEngine;

namespace Mimicraft.Gameplay
{
	public class DebrisPiece : MonoBehaviour
	{
		private float lifetime;

		private float shrinkSeconds;

		private float age;

		private Vector3 bornScale;

		public void Begin(float lifetime, float shrinkSeconds)
		{
			this.lifetime = Mathf.Max(0f, lifetime);
			this.shrinkSeconds = Mathf.Max(0.01f, shrinkSeconds);
			bornScale = base.transform.localScale;
		}

		private void Update()
		{
			age += Time.deltaTime;
			if (!(age < lifetime))
			{
				float num = (age - lifetime) / shrinkSeconds;
				if (num >= 1f)
				{
					Object.Destroy(base.gameObject);
				}
				else
				{
					base.transform.localScale = bornScale * (1f - num);
				}
			}
		}
	}
}
