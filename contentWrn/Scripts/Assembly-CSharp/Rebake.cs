using UnityEngine;
using UnityEngine.AI;

public class Rebake : MonoBehaviour
{
	public Transform target;

	private void Test()
	{
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		component.SetDestination(target.position);
		Debug.Log(Vector3.Distance(component.pathEndPosition, component.destination) < 2f);
	}
}
