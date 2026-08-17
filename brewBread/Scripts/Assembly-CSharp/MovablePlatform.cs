using System;
using System.Collections;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
	private enum movablePlatformType
	{
		CONTINUE = 0,
		ENDS_STOP = 1,
		START_STOP = 2,
		FINISH_STOP = 3,
		STEP_STOP = 4
	}

	[Serializable]
	public struct MovablePlatformPoint
	{
		[SerializeField]
		public Transform Point;

		[SerializeField]
		public float WaitTime;
	}

	[SerializeField]
	private movablePlatformType platformType;

	public GameObject platform;

	private PlatformAnimator _platformAnimator;

	public bool closeCircuit;

	public float speed;

	[SerializeField]
	private Vector3 _offset;

	public MovablePlatformPoint[] points;

	private int actualPoint;

	private bool growing = true;

	private Vector3 target;

	private bool CRTrunning;

	private void Start()
	{
		platform.transform.position = points[0].Point.position;
		target = points[1].Point.position + _offset;
		actualPoint = 1;
		if (!platform.TryGetComponent<PlatformAnimator>(out _platformAnimator))
		{
			Debug.LogError("Couldn't find the component: Platform Animator on your platform");
		}
		else
		{
			_platformAnimator?.StartWeels(value: true);
		}
	}

	private void Update()
	{
		if (platform.transform.position == target && !CRTrunning)
		{
			switch (platformType)
			{
			case movablePlatformType.CONTINUE:
				SetNextPoint();
				break;
			case movablePlatformType.ENDS_STOP:
				if (actualPoint == points.Length - 1 || actualPoint == 0)
				{
					StartCoroutine(SetNextPointDelay(points[actualPoint].WaitTime));
				}
				else
				{
					SetNextPoint();
				}
				break;
			case movablePlatformType.START_STOP:
				if (actualPoint == 0)
				{
					StartCoroutine(SetNextPointDelay(points[actualPoint].WaitTime));
				}
				else
				{
					SetNextPoint();
				}
				break;
			case movablePlatformType.FINISH_STOP:
				if (actualPoint == points.Length - 1)
				{
					StartCoroutine(SetNextPointDelay(points[actualPoint].WaitTime));
				}
				else
				{
					SetNextPoint();
				}
				break;
			case movablePlatformType.STEP_STOP:
				StartCoroutine(SetNextPointDelay(points[actualPoint].WaitTime));
				break;
			}
		}
		platform.transform.position = Vector3.MoveTowards(platform.transform.position, target, speed * Time.deltaTime);
	}

	private IEnumerator SetNextPointDelay(float time)
	{
		CRTrunning = true;
		_platformAnimator?.StartWeels(value: false);
		yield return new WaitForSeconds(time);
		SetNextPoint();
		CRTrunning = false;
	}

	private void SetNextPoint()
	{
		_platformAnimator?.StartWeels(value: true);
		if (closeCircuit)
		{
			if (points.Length == actualPoint + 1)
			{
				actualPoint = 0;
			}
			else
			{
				actualPoint++;
			}
		}
		else if (points.Length == actualPoint + 1)
		{
			growing = false;
			actualPoint--;
		}
		else if (actualPoint == 0)
		{
			growing = true;
			actualPoint++;
		}
		else if (growing)
		{
			actualPoint++;
		}
		else
		{
			actualPoint--;
		}
		target = points[actualPoint].Point.position + _offset;
	}
}
