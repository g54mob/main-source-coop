using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShadedTechnology.WindshieldRainAsset
{
	[Serializable]
	public class WiperObject
	{
		public Transform originPos;

		public Transform startPos;

		public Transform endPos;

		[HideInInspector]
		public Queue<Vector2> prevStartPositions = new Queue<Vector2>();

		public Queue<Vector2> prevEndPositions = new Queue<Vector2>();

		[HideInInspector]
		public Vector2 lastStartPos;

		[HideInInspector]
		public Vector2 lastEndPos;

		public void InitWiper(int delay, WindshieldPlane windshieldPlane)
		{
			Vector2 item = windshieldPlane.WorldPosToWindshieldPos(startPos.position);
			Vector2 item2 = windshieldPlane.WorldPosToWindshieldPos(endPos.position);
			lastStartPos = item;
			lastEndPos = item2;
			for (int i = 0; i < delay; i++)
			{
				prevStartPositions.Enqueue(item);
				prevEndPositions.Enqueue(item2);
			}
		}

		public void UpdateWiper(WindshieldPlane windshieldPlane)
		{
			Vector2 item = windshieldPlane.WorldPosToWindshieldPos(startPos.position);
			Vector2 item2 = windshieldPlane.WorldPosToWindshieldPos(endPos.position);
			prevStartPositions.Enqueue(item);
			prevEndPositions.Enqueue(item2);
			lastStartPos = prevStartPositions.Dequeue();
			lastEndPos = prevEndPositions.Dequeue();
		}
	}
}
