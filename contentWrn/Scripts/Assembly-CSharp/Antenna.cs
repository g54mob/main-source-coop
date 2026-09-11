using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class Antenna : MonoBehaviour
{
	public Transform root;

	[FormerlySerializedAs("defaultTarget")]
	[FormerlySerializedAs("target")]
	public Transform mimicPullTarget;

	private Spline spline;

	private SplineContainer container;

	private LineRenderer lineRenderer;

	public int samples;

	private Bot_Angler angler;

	public float upPosBetweenMeAndMimic;

	public float upPosY = 10f;

	public bool IsPullingMimic;

	public float pullForce = 2f;

	private PhotonView view_g;

	public Vector3 GetUpPostion
	{
		get
		{
			Vector3 result = Vector3.Lerp(angler.transform.position, angler.mimic.transform.position, upPosBetweenMeAndMimic);
			result.y = root.transform.position.y + upPosY;
			return result;
		}
	}

	public Vector3 GetTargetPosition => angler.mimic.player.HeadPosition();

	public Vector3 GetRootPosition => root.position;

	public void Start()
	{
		angler = GetComponent<Bot_Angler>();
		view_g = GetComponent<PhotonView>();
		container = GetComponent<SplineContainer>();
		lineRenderer = GetComponent<LineRenderer>();
	}

	public void Go()
	{
		spline = container.Spline;
		BezierKnot value = spline[0];
		value.Position = GetRootPosition;
		spline[0] = value;
		value = spline[1];
		value.Position = GetUpPostion;
		spline[1] = value;
		value = spline[2];
		value.Position = GetTargetPosition;
		spline[2] = value;
		container.Spline = spline;
		float length = spline.GetLength();
		float num = length / (float)samples;
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < samples; i++)
		{
			list.Add(spline.EvaluatePosition(Mathf.InverseLerp(0f, length, num * (float)i)));
		}
		lineRenderer.positionCount = samples;
		lineRenderer.SetPositions(list.ToArray());
	}

	public void PullMimic()
	{
		if (view_g.IsMine)
		{
			Vector3 vector = mimicPullTarget.position - angler.mimic.player.HeadPosition();
			vector = Vector3.ClampMagnitude(vector, 5f);
			vector *= pullForce;
			angler.mimic.player.refs.ragdoll.GetBodypart(BodypartType.Head).rig.AddForce(vector, ForceMode.Acceleration);
			angler.mimic.player.refs.ragdoll.GetBodypart(BodypartType.Torso).rig.AddForce(vector, ForceMode.Acceleration);
			angler.mimic.player.refs.ragdoll.AddForce(vector * 0.5f, ForceMode.Acceleration);
			angler.mimic.player.data.sinceGrounded = Mathf.Clamp(angler.mimic.player.data.sinceGrounded, 0f, 1f);
		}
	}

	private void FixedUpdate()
	{
		if (angler.IsSucking)
		{
			PullMimic();
		}
	}

	private void Update()
	{
		if (angler.IsSucking)
		{
			if (!lineRenderer.enabled)
			{
				lineRenderer.enabled = true;
			}
			Go();
		}
		else if (lineRenderer.enabled)
		{
			lineRenderer.enabled = false;
		}
	}
}
