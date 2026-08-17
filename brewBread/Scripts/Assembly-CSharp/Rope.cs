using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
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

	public Transform StartPoint;

	public Transform EndPoint;

	private bool _initialized;

	private LineRenderer _lineRenderer;

	private List<RopeSegment> _ropeSegments = new List<RopeSegment>();

	[SerializeField]
	private float _ropeSegLen = 0.25f;

	[SerializeField]
	private int _segmentLength = 35;

	[SerializeField]
	private float _lineWidth = 0.1f;

	[SerializeField]
	private bool _autoConfigure = true;

	private void Start()
	{
		if (_initialized)
		{
			return;
		}
		if (_autoConfigure)
		{
			float num = Vector3.Distance(EndPoint.position, StartPoint.position);
			_segmentLength = Mathf.CeilToInt(num / _ropeSegLen);
			if ((float)_segmentLength * _ropeSegLen != num)
			{
				_ropeSegLen = num / (float)_segmentLength;
			}
		}
		_lineRenderer = GetComponent<LineRenderer>();
		Vector3 position = StartPoint.position;
		for (int i = 0; i < _segmentLength; i++)
		{
			_ropeSegments.Add(new RopeSegment(position));
			position.y -= _ropeSegLen;
		}
	}

	public void ShowRope(bool value)
	{
		_lineRenderer.enabled = value;
	}

	public void Initialize()
	{
	}

	private void Update()
	{
		DrawRope();
	}

	private void FixedUpdate()
	{
		Simulate();
	}

	private void Simulate()
	{
		Vector2 vector = new Vector2(0f, -1f);
		for (int i = 1; i < _segmentLength; i++)
		{
			RopeSegment value = _ropeSegments[i];
			Vector2 vector2 = value.posNow - value.posOld;
			value.posOld = value.posNow;
			value.posNow += vector2;
			value.posNow += vector * Time.fixedDeltaTime;
			_ropeSegments[i] = value;
		}
		for (int j = 0; j < 50; j++)
		{
			ApplyConstraint();
		}
	}

	private void ApplyConstraint()
	{
		RopeSegment value = _ropeSegments[0];
		value.posNow = StartPoint.position;
		_ropeSegments[0] = value;
		RopeSegment value2 = _ropeSegments[_ropeSegments.Count - 1];
		value2.posNow = EndPoint.position;
		_ropeSegments[_ropeSegments.Count - 1] = value2;
		for (int i = 0; i < _segmentLength - 1; i++)
		{
			RopeSegment value3 = _ropeSegments[i];
			RopeSegment value4 = _ropeSegments[i + 1];
			float magnitude = (value3.posNow - value4.posNow).magnitude;
			float num = Mathf.Abs(magnitude - _ropeSegLen);
			Vector2 vector = Vector2.zero;
			if (magnitude > _ropeSegLen)
			{
				vector = (value3.posNow - value4.posNow).normalized;
			}
			else if (magnitude < _ropeSegLen)
			{
				vector = (value4.posNow - value3.posNow).normalized;
			}
			Vector2 vector2 = vector * num;
			if (i != 0)
			{
				value3.posNow -= vector2 * 0.5f;
				_ropeSegments[i] = value3;
				value4.posNow += vector2 * 0.5f;
				_ropeSegments[i + 1] = value4;
			}
			else
			{
				value4.posNow += vector2;
				_ropeSegments[i + 1] = value4;
			}
		}
	}

	private void DrawRope()
	{
		float lineWidth = _lineWidth;
		_lineRenderer.startWidth = lineWidth;
		_lineRenderer.endWidth = lineWidth;
		Vector3[] array = new Vector3[_segmentLength];
		for (int i = 0; i < _segmentLength; i++)
		{
			array[i] = _ropeSegments[i].posNow;
		}
		_lineRenderer.positionCount = array.Length;
		_lineRenderer.SetPositions(array);
	}
}
