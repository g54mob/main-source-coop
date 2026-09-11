using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Level : SerializedMonoBehaviour
{
	public RichPresenceState richPresenceState;

	public Dictionary<PatrolPoint.PatrolGroup, List<PatrolPoint>> patrolGroups = new Dictionary<PatrolPoint.PatrolGroup, List<PatrolPoint>>();

	public List<Light> lights = new List<Light>();

	public static Level currentLevel;

	public bool levelIsReady;

	public Action OnLevelFinishedSetUp;

	public Action<bool, Vector3, float> toggleLights;

	public List<LightToggler> lightTogglers;

	private int checkId;

	private bool m_disableShadows;

	private bool m_enableLights = true;

	private void Awake()
	{
		currentLevel = this;
	}

	private IEnumerator Start()
	{
		for (int i = 0; i < 2; i++)
		{
			yield return null;
		}
		GetComponentInChildren<PatrolPoint>().GetClosestPoints_All();
		PatrolPoint[] componentsInChildren = GetComponentsInChildren<PatrolPoint>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (!patrolGroups.ContainsKey(componentsInChildren[j].group))
			{
				patrolGroups.Add(componentsInChildren[j].group, new List<PatrolPoint>());
			}
			if (componentsInChildren[j].connectedPoints.Count > 0)
			{
				patrolGroups[componentsInChildren[j].group].Add(componentsInChildren[j]);
			}
			else
			{
				componentsInChildren[j].gameObject.SetActive(value: false);
			}
		}
		SetupFinished();
		if (SurfaceNetworkHandler.HasStarted)
		{
			RichPresenceState richPresenceState = this.richPresenceState;
			if (richPresenceState == RichPresenceState.Status_MainMenu)
			{
				richPresenceState = RichPresenceState.Status_InFactory;
			}
			RichPresenceHandler.SetPresenceState(richPresenceState);
		}
	}

	private void SetupFinished()
	{
		OnLevelFinishedSetUp?.Invoke();
		levelIsReady = true;
	}

	public PatrolPoint GetClosestPoint(List<PatrolPoint.PatrolGroup> groups, Vector3 pos, PatrolPoint current, float maxHeightDif = 100000f, bool includeTemporary = false)
	{
		List<PatrolPoint> pointsInGroups = GetPointsInGroups(groups);
		float num = float.PositiveInfinity;
		PatrolPoint patrolPoint = null;
		for (int i = 0; i < pointsInGroups.Count; i++)
		{
			if (includeTemporary || !pointsInGroups[i].temporaryPoint)
			{
				float num2 = Vector3.Distance(pos, pointsInGroups[i].transform.position);
				if (!(Mathf.Abs(pos.y - pointsInGroups[i].transform.position.y) > maxHeightDif) && (!(current != null) || !(pointsInGroups[i] == current)) && num2 < num)
				{
					num = num2;
					patrolPoint = pointsInGroups[i];
				}
			}
		}
		if (!patrolPoint && (bool)current)
		{
			return current;
		}
		return patrolPoint;
	}

	public List<PatrolPoint> GetPointsInGroups(List<PatrolPoint.PatrolGroup> groups)
	{
		List<PatrolPoint> list = new List<PatrolPoint>();
		for (int i = 0; i < groups.Count; i++)
		{
			if (patrolGroups.ContainsKey(groups[i]))
			{
				list.AddRange(patrolGroups[groups[i]]);
			}
		}
		return list;
	}

	internal void ToggleLights(bool setLightsOn, Vector3 position, float range)
	{
		toggleLights?.Invoke(setLightsOn, position, range);
	}

	public void ToggleLightsForSeconds(bool setLightsOn, Vector3 position, float range, float time)
	{
		StartCoroutine(IToggleLights());
		IEnumerator IToggleLights()
		{
			toggleLights(setLightsOn, position, range);
			yield return new WaitForSeconds(time);
			toggleLights(arg1: true, position, range);
		}
	}

	internal PatrolPoint GetFreePointWithDistance(List<PatrolPoint.PatrolGroup> pgs, Vector3 position, int minDistance, float maxHeightDif, bool includeTemporary = false)
	{
		List<PatrolPoint> pointsInGroups = GetPointsInGroups(pgs);
		int num = 100;
		while (num > 0)
		{
			num--;
			PatrolPoint patrolPoint = pointsInGroups[UnityEngine.Random.Range(0, pointsInGroups.Count)];
			if ((includeTemporary || !patrolPoint.temporaryPoint) && !(Vector3.Distance(patrolPoint.transform.position, position) < (float)minDistance) && Physics.OverlapSphere(patrolPoint.transform.position + Vector3.up * 1.5f, 1f).Length == 0 && !(Mathf.Abs(position.y - patrolPoint.transform.position.y) > maxHeightDif))
			{
				return patrolPoint;
			}
		}
		return null;
	}

	internal List<PatrolPoint> GetPointsOutsideMinDistanceSortedOnClosest(List<PatrolPoint.PatrolGroup> pgs, Vector3 position, float minDistance, float maxHeightDif, bool includeTemporary = false)
	{
		return (from tuple in (from point in GetPointsInGroups(pgs)
				select (point: point, distance: Vector3.Distance(point.transform.position, position)) into tuple
				orderby tuple.distance
				select tuple).Where(delegate((PatrolPoint point, float distance) pointTuple)
			{
				if (!includeTemporary && pointTuple.point.temporaryPoint)
				{
					return false;
				}
				if (pointTuple.distance < minDistance)
				{
					return false;
				}
				return !(Mathf.Abs(position.y - pointTuple.point.transform.position.y) > maxHeightDif);
			})
			select tuple.point).ToList();
	}

	internal PatrolPoint GetClosestHiddenPoint(Vector3 vector3, bool includeTemporary = false)
	{
		List<PatrolPoint> pointsOutsideMinDistanceSortedOnClosest = GetPointsOutsideMinDistanceSortedOnClosest(new List<PatrolPoint.PatrolGroup>
		{
			PatrolPoint.PatrolGroup.Bear,
			PatrolPoint.PatrolGroup.Dog
		}, vector3, 20f, 5f);
		for (int i = 0; i < pointsOutsideMinDistanceSortedOnClosest.Count; i++)
		{
			if ((includeTemporary || !pointsOutsideMinDistanceSortedOnClosest[i].temporaryPoint) && !PlayerHandler.instance.CanAnAlivePlayerSeePoint(pointsOutsideMinDistanceSortedOnClosest[i].transform.position, out var _))
			{
				return pointsOutsideMinDistanceSortedOnClosest[i];
			}
		}
		return null;
	}

	internal void AddPoint(PatrolPoint patrolPoint)
	{
		if (!patrolGroups.ContainsKey(patrolPoint.group))
		{
			patrolGroups.Add(patrolPoint.group, new List<PatrolPoint>());
		}
		patrolGroups[patrolPoint.group].Add(patrolPoint);
	}

	internal void RemovePoint(PatrolPoint patrolPoint)
	{
		if (patrolGroups[patrolPoint.group].Contains(patrolPoint))
		{
			patrolGroups[patrolPoint.group].Remove(patrolPoint);
		}
	}

	internal PatrolPoint GetRandomPoint(List<PatrolPoint.PatrolGroup> groups)
	{
		List<PatrolPoint> pointsInGroups = GetPointsInGroups(groups);
		PatrolPoint patrolPoint = null;
		int num = 100;
		while (num > 0)
		{
			num--;
			patrolPoint = pointsInGroups[UnityEngine.Random.Range(0, pointsInGroups.Count)];
			if (!patrolPoint.temporaryPoint)
			{
				break;
			}
		}
		return patrolPoint;
	}

	private void Update()
	{
		checkId++;
		if (checkId > 10)
		{
			checkId = 0;
			Check();
		}
	}

	private void Check()
	{
		foreach (LightToggler lightToggler in lightTogglers)
		{
			lightToggler.Check(m_disableShadows, m_enableLights);
		}
	}
}
