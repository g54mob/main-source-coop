using UnityEngine;
using UnityEngine.AI;

namespace Features.AIModule.Scripts.Services
{
	public class TestDebug : MonoBehaviour
	{
		private void Awake()
		{
			Debug.LogError(GetComponent<NavMeshAgent>().agentTypeID, this);
		}
	}
}
