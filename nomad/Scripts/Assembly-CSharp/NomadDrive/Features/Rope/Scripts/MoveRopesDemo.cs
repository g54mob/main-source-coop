using System;
using UnityEngine;

namespace NomadDrive.Features.Rope.Scripts
{
	public class MoveRopesDemo : MonoBehaviour
	{
		private Vector3 startPos;

		public float radius = 1f;

		public float angle;

		public float speed = 1f;

		private Vector3 movePos;

		private Vector3 cmovePos;

		private Vector3 movevel;

		private float moveDamp = 0.25f;

		private void Start()
		{
			startPos = base.transform.position;
		}

		private void Update()
		{
			angle += Time.deltaTime * speed;
			angle = Mathf.Repeat(angle, (float)Math.PI * 2f);
			Vector3 zero = Vector3.zero;
			zero.x = Mathf.Sin(angle) * radius;
			zero.y = Mathf.Cos(angle) * radius;
			if (FullDemoInputs.GetInput(FullDemoInputs.DemoInputs.MoveRopesEnabled))
			{
				movePos.x -= Input.GetAxis("Mouse X") * 0.1f;
				movePos.y += Input.GetAxis("Mouse Y") * 0.1f;
			}
			if (movePos.x > 1f)
			{
				movePos.x = 1f;
			}
			if (movePos.x < -4f)
			{
				movePos.x = -4f;
			}
			if (movePos.y < -2.8f)
			{
				movePos.x = -2.8f;
			}
			if (movePos.y > 2.8f)
			{
				movePos.x = 2.8f;
			}
			cmovePos = Vector3.SmoothDamp(cmovePos, movePos, ref movevel, moveDamp);
			base.transform.position = startPos + cmovePos;
		}
	}
}
