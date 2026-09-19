using UnityEngine;

namespace Mirror.Examples.Benchmark
{
	public class MonsterMovement : NetworkBehaviour
	{
		public float speed = 1f;

		[Header("Note: use 0.1 to test change detection, 0.5 is too high!")]
		public float movementProbability = 0.1f;

		public float movementDistance = 20f;

		private bool moving;

		private Vector3 start;

		private Vector3 destination;

		public override void OnStartServer()
		{
			start = base.transform.position;
		}

		[ServerCallback]
		private void Update()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (moving)
			{
				if (Vector3.Distance(base.transform.position, destination) <= 0.01f)
				{
					base.transform.position = destination;
					moving = false;
				}
				else
				{
					base.transform.position = Vector3.MoveTowards(base.transform.position, destination, speed * Time.deltaTime);
				}
			}
			else if (Random.value < movementProbability * Time.deltaTime)
			{
				Vector2 insideUnitCircle = Random.insideUnitCircle;
				Vector3 vector = new Vector3(insideUnitCircle.x, 0f, insideUnitCircle.y);
				destination = start + vector * movementDistance;
				moving = true;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
