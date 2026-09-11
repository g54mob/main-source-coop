using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;

public static class ContentPolling
{
	private const int m_resolution = 20;

	private static Camera m_currentPollingCamera;

	private static int m_currentPollingY;

	private static List<ContentEventFrame> contentEvents = new List<ContentEventFrame>();

	private static Dictionary<ContentProvider, int> contentProviders = new Dictionary<ContentProvider, int>();

	private static float m_clipTime;

	public static void StartPolling(Camera camera, float time)
	{
		if (m_currentPollingCamera != null)
		{
			throw new Exception("Polling already in progress, you must complete the current polling before starting a new one.");
		}
		contentEvents = new List<ContentEventFrame>();
		contentProviders = new Dictionary<ContentProvider, int>();
		m_currentPollingCamera = camera;
		m_currentPollingY = 0;
		m_clipTime = time;
	}

	public static void TickPoll()
	{
		if (m_currentPollingCamera == null || m_currentPollingY >= 20)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 20; j++)
			{
				int currentPollingY = m_currentPollingY;
				Poll(j, currentPollingY, m_currentPollingCamera);
			}
			m_currentPollingY++;
		}
	}

	private static void Poll(int x, int y, Camera camera)
	{
		float x2 = (float)x / 20f;
		float y2 = (float)y / 20f;
		Ray ray = camera.ViewportPointToRay(new Vector3(x2, y2, 0f));
		float num = 200f;
		Vector3 end = ray.origin + ray.direction * num;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray, out hitInfo, num, int.MaxValue, QueryTriggerInteraction.Ignore);
		if (flag)
		{
			end = hitInfo.point;
			flag = false;
			ContentProvider componentInParent = hitInfo.collider.GetComponentInParent<ContentProvider>();
			if (componentInParent != null)
			{
				if (!contentProviders.TryAdd(componentInParent, 1))
				{
					contentProviders[componentInParent]++;
				}
				flag = true;
			}
		}
		Debug.DrawLine(ray.origin, end, flag ? Color.green : Color.red);
	}

	public static bool CompletePolling(out List<ContentEventFrame> eventFrames)
	{
		if (m_currentPollingCamera == null)
		{
			eventFrames = null;
			return false;
		}
		int num = 20 - m_currentPollingY;
		for (int i = 0; i < 20; i++)
		{
			for (int j = 0; j < num; j++)
			{
				Poll(i, j, m_currentPollingCamera);
			}
		}
		foreach (KeyValuePair<ContentProvider, int> contentProvider in contentProviders)
		{
			float seenAmount = (float)contentProvider.Value / 400f;
			contentProvider.Key.GetContent(contentEvents, seenAmount, m_currentPollingCamera, m_clipTime);
		}
		contentProviders.Clear();
		m_currentPollingCamera = null;
		m_currentPollingY = 0;
		eventFrames = contentEvents;
		return true;
	}

	public static List<ContentEventFrame> CombineAndGetBestContentEvent(List<ContentEventFrame> events)
	{
		events = ContentCombiningSystem.CombineEvents(events);
		return events;
	}

	public static List<ContentEventFrame> CombineDuplicateContentEvents(List<ContentEventFrame> events)
	{
		Dictionary<ContentEventFrame, List<ContentEventFrame>> dictionary = new Dictionary<ContentEventFrame, List<ContentEventFrame>>();
		foreach (ContentEventFrame @event in events)
		{
			if (!dictionary.TryAdd(@event, new List<ContentEventFrame> { @event }))
			{
				dictionary[@event].Add(@event);
			}
		}
		List<ContentEventFrame> list = new List<ContentEventFrame>();
		foreach (KeyValuePair<ContentEventFrame, List<ContentEventFrame>> item in dictionary)
		{
			float seenAmount = item.Value.Sum((ContentEventFrame eventFrame) => eventFrame.seenAmount);
			float time = item.Key.time;
			ContentEvent contentEvent = item.Value.Select((ContentEventFrame eventFrame) => eventFrame.contentEvent).MaxBy((ContentEvent eventFrame) => eventFrame.GetContentValue());
			list.Add(new ContentEventFrame(contentEvent, seenAmount, time));
		}
		return list;
	}
}
