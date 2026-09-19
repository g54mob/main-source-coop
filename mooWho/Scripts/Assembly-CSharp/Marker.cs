using UnityEngine;

public class Marker : MonoBehaviour
{
	public MarkerDefinition markerDefinition;

	private MarkerHandler markerHandler;

	private Transform objToFollow;

	private float timeAlive;

	public Vector3 targetPos { get; private set; }

	public GameObject worldObject { get; private set; }

	private void Update()
	{
		if ((bool)objToFollow)
		{
			targetPos = objToFollow.transform.position;
		}
		if (!markerDefinition.unlimitedLife)
		{
			timeAlive += Time.deltaTime;
			if (timeAlive > markerDefinition.lifeDuration)
			{
				DestroyMarker();
			}
		}
	}

	public virtual void InitializeMarker(MarkerHandler markerHandler, GameObject worldObject, Vector3 targetPos, Transform objToFollow)
	{
		this.markerHandler = markerHandler;
		this.worldObject = worldObject;
		this.targetPos = targetPos;
		this.objToFollow = objToFollow;
	}

	public virtual void DestroyMarker()
	{
		markerHandler.RemoveMarker(this);
	}
}
