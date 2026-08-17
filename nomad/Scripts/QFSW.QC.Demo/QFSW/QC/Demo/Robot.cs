using System.Collections.Generic;
using QFSW.QC.Actions;
using UnityEngine;

namespace QFSW.QC.Demo
{
	[CommandPrefix("demo.robot.")]
	public class Robot : MonoBehaviour
	{
		[SerializeField]
		private GameObject deathFX;

		[Command("speed", Platform.AllPlatforms, MonoTargetType.Single)]
		private static float robotSpeed = 25f;

		[Command("rotation-speed", Platform.AllPlatforms, MonoTargetType.Single)]
		private static float robotRotationSpeed = 40f;

		private Vector2 direction;

		private void Start()
		{
			GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
			float num = Random.Range(0.8f, 1.2f);
			base.transform.localScale = new Vector3(num, num, num);
			direction = Quaternion.Euler(0f, 0f, Random.Range(0f, 306f)) * Vector3.up;
		}

		private void FixedUpdate()
		{
			GetComponent<Rigidbody2D>().AddForce(direction * robotSpeed * Time.fixedDeltaTime);
			direction = Quaternion.Euler(0f, 0f, robotRotationSpeed * Time.fixedDeltaTime) * direction;
		}

		[Command("kill-select", Platform.AllPlatforms, MonoTargetType.Single)]
		private static IEnumerator<ICommandAction> KillAction()
		{
			Robot robot = null;
			IEnumerable<Robot> robots = InvocationTargetFactory.FindTargets<Robot>(MonoTargetType.All);
			yield return new Value("Please select a robot");
			yield return new Choice<Robot>(robots, delegate(Robot r)
			{
				robot = r;
			});
			robot.Die();
			yield return new Typewriter(robot.name + " has been killed");
		}

		[Command("kill", MonoTargetType.Argument, Platform.AllPlatforms)]
		[Command("kill-multi", MonoTargetType.ArgumentMulti, Platform.AllPlatforms)]
		[Command("kill-all", MonoTargetType.All, Platform.AllPlatforms)]
		public void Die()
		{
			Object.Destroy(base.gameObject);
			Object.Destroy(Object.Instantiate(deathFX, base.transform.position, Quaternion.identity), 3f);
		}

		[Command("position", MonoTargetType.All, Platform.AllPlatforms)]
		private Vector3 GetPosition()
		{
			return base.transform.position;
		}
	}
}
