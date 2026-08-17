using UnityEngine;

namespace StylizedGrass
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class GrassMaskingSphere : MonoBehaviour
	{
		[Min(0.1f)]
		public float radius = 0.5f;

		public Vector3 offset;

		private Vector4 vector;

		private readonly int _PlayerSphereID = Shader.PropertyToID("_PlayerSphere");

		public void Update()
		{
			UpdateProperties();
		}

		private void UpdateProperties()
		{
			vector = base.transform.position + offset;
			vector.w = radius * base.transform.lossyScale.magnitude;
			Shader.SetGlobalVector(_PlayerSphereID, vector);
		}

		private void OnDisable()
		{
			Shader.SetGlobalVector(_PlayerSphereID, Vector4.zero);
		}

		private void OnDrawGizmosSelected()
		{
			UpdateProperties();
			Gizmos.DrawWireSphere(vector, vector.w);
		}
	}
}
