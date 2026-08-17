using UnityEngine;

namespace NomadDrive.Features.Rope
{
	public class RopeManager : MonoBehaviour
	{
		private Rope[] ropes;

		private void Start()
		{
			ropes = Object.FindObjectsByType<Rope>(FindObjectsSortMode.None);
		}

		private void Update()
		{
			for (int i = 0; i < ropes.Length; i++)
			{
				ropes[i].RopeUpdate();
			}
		}

		private void FixedUpdate()
		{
			for (int i = 0; i < ropes.Length; i++)
			{
				ropes[i].RopeFixedUpdate();
			}
		}
	}
}
