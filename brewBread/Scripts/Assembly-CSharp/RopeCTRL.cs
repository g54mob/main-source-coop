using System.Collections.Generic;
using UnityEngine;

public class RopeCTRL : MonoBehaviour
{
	public struct RopeSegment
	{
		public Vector2 posNow;

		public Vector2 posOld;

		public RopeSegment(Vector2 pos)
		{
			posNow = pos;
			posOld = pos;
		}
	}

	private LineRenderer lineRenderer;

	private List<RopeSegment> ropeSegments = new List<RopeSegment>();

	public Transform startPosition;

	public Transform endPosition;

	public float ropeSegLen = 0.25f;

	public int segmentLenght = 35;

	public float lineWidth = 0.1f;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		Vector3 position = startPosition.position;
		for (int i = 0; i < segmentLenght; i++)
		{
			ropeSegments.Add(new RopeSegment(position));
			position.y -= ropeSegLen;
		}
	}

	private void Update()
	{
		DrawRope();
		PositionConstraint();
	}

	private void PositionConstraint()
	{
		RopeSegment value = ropeSegments[0];
		value.posNow = startPosition.position;
		ropeSegments[0] = value;
		if (endPosition != null)
		{
			RopeSegment value2 = ropeSegments[ropeSegments.Count - 1];
			value2.posNow = endPosition.position;
			ropeSegments[ropeSegments.Count - 1] = value2;
		}
	}

	private void FixedUpdate()
	{
		SimulateRope();
	}

	private void SimulateRope()
	{
		Vector2 vector = new Vector2(0f, -1f);
		for (int i = 0; i < segmentLenght; i++)
		{
			RopeSegment value = ropeSegments[i];
			Vector2 vector2 = value.posNow - value.posOld;
			value.posOld = value.posNow;
			value.posNow += vector2;
			value.posNow += vector * Time.fixedDeltaTime;
			ropeSegments[i] = value;
		}
		for (int j = 0; j < 50; j++)
		{
			ContraintRope();
		}
	}

	private void ContraintRope()
	{
		for (int i = 0; i < segmentLenght - 1; i++)
		{
			RopeSegment value = ropeSegments[i];
			RopeSegment value2 = ropeSegments[i + 1];
			float magnitude = (value.posNow - value2.posNow).magnitude;
			float num = Mathf.Abs(magnitude - ropeSegLen);
			Vector2 vector = Vector2.zero;
			if (magnitude > ropeSegLen)
			{
				vector = (value.posNow - value2.posNow).normalized;
			}
			else if (magnitude < ropeSegLen)
			{
				vector = (value2.posNow - value.posNow).normalized;
			}
			Vector2 vector2 = vector * num;
			if (i != 0)
			{
				value.posNow -= vector2 * 0.5f;
				ropeSegments[i] = value;
				value2.posNow += vector2 * 0.5f;
				ropeSegments[i + 1] = value2;
			}
			else
			{
				value2.posNow += vector2;
				ropeSegments[i + 1] = value2;
			}
		}
	}

	private void DrawRope()
	{
		float num = lineWidth;
		lineRenderer.startWidth = num;
		lineRenderer.endWidth = num;
		Vector3[] array = new Vector3[segmentLenght];
		for (int i = 0; i < segmentLenght; i++)
		{
			array[i] = ropeSegments[i].posNow;
		}
		lineRenderer.positionCount = array.Length;
		lineRenderer.SetPositions(array);
	}
}
