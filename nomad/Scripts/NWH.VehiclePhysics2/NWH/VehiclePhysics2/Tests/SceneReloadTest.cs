using UnityEngine;
using UnityEngine.SceneManagement;

namespace NWH.VehiclePhysics2.Tests
{
	public class SceneReloadTest : MonoBehaviour
	{
		private void Update()
		{
			if (Time.timeSinceLevelLoad > 3f)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}
}
