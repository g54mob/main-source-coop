using UnityEngine;

namespace Assets._Scripts.InteractiveRope
{
	public class RopeCollision : MonoBehaviour
	{
		private Rigidbody2D _rb;

		private void Start()
		{
			_rb = GetComponent<Rigidbody2D>();
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (collider.gameObject.layer == 9)
			{
				_rb.AddForce((base.transform.position - collider.transform.position).normalized * 100f);
			}
		}
	}
}
