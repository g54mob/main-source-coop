using System.Collections.Generic;
using UnityEngine;

public class MarkerHandler : MonoBehaviour
{
	public static MarkerHandler instance;

	[SerializeField]
	private MarkerDefinition[] allMarkerDefinitions;

	[SerializeField]
	private Transform markerContainer;

	private List<Marker> markerInstances = new List<Marker>();

	private Camera cam;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		cam = Camera.main;
	}

	private void LateUpdate()
	{
		UpdateMarkers();
	}

	private void UpdateMarkers()
	{
		for (int i = 0; i < markerInstances.Count; i++)
		{
			Marker marker = markerInstances[i];
			marker.transform.position = cam.WorldToScreenPoint(marker.targetPos);
			_ = marker.transform.position;
			Vector3.Dot(cam.transform.forward, (cam.transform.position - marker.targetPos).normalized);
		}
	}

	public Marker SpawnMarker(byte markerID, Vector3 targetPos, Transform objToFollow)
	{
		MarkerDefinition markerDefinition = allMarkerDefinitions[markerID];
		GameObject obj = Object.Instantiate(markerDefinition.markerLocalObj, markerContainer);
		GameObject worldObject = null;
		Marker component = obj.GetComponent<Marker>();
		if ((bool)markerDefinition.markerWorldObj)
		{
			worldObject = Object.Instantiate(markerDefinition.markerWorldObj, targetPos, Quaternion.identity);
		}
		component.InitializeMarker(this, worldObject, targetPos, objToFollow);
		AddMarker(component);
		return component;
	}

	public void AddMarker(Marker marker)
	{
		markerInstances.Add(marker);
	}

	public void RemoveMarker(Marker marker)
	{
		markerInstances.Remove(marker);
		if ((bool)marker.worldObject)
		{
			Object.Destroy(marker.worldObject);
		}
		Object.Destroy(marker.gameObject);
	}
}
