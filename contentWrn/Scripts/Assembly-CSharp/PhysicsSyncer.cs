using Photon.Pun;
using UnityEngine;

public class PhysicsSyncer : MonoBehaviour, IPunObservable
{
	[SerializeField]
	private PhotonView view;

	[SerializeField]
	private Rigidbody rig;

	private bool hasRecieved;

	private float sinceLastPackage;

	private Vector3 lastPos;

	private Vector3 targetPos;

	private Quaternion targetRot;

	private void FixedUpdate()
	{
		if (view == null)
		{
			Debug.LogError("PhotonView null in PhysicsSyncer.FixedUpdate");
			view = GetComponent<PhotonView>();
		}
		if (rig == null)
		{
			Debug.LogError("Rig null in PhysicsSyncer.FixedUpdate");
			rig = GetComponent<Rigidbody>();
		}
		if (!view.IsMine && hasRecieved)
		{
			float num = 1f / (float)PhotonNetwork.SerializationRate;
			sinceLastPackage += Time.fixedDeltaTime;
			Vector3 vector = Vector3.Lerp(lastPos, targetPos, sinceLastPackage / num);
			Vector3 position = base.transform.position;
			Vector3 vector2 = vector - position;
			rig.MovePosition(rig.position + vector2 * 0.5f);
			rig.MoveRotation(Quaternion.RotateTowards(rig.rotation, targetRot, Time.fixedDeltaTime * 90f));
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		bool flag = false;
		if (view == null)
		{
			view = GetComponent<PhotonView>();
			Debug.LogError(string.Format("{0} {1} null, refetch: {2}", "PhysicsSyncer", "view", view));
			flag = true;
		}
		if (rig == null)
		{
			rig = GetComponent<Rigidbody>();
			Debug.LogError(string.Format("{0} {1} null, refetch: {2}", "PhysicsSyncer", "rig", rig));
			flag = true;
		}
		if ((flag && (view == null || rig == null)) || stream == null)
		{
			Debug.LogError(string.Format("{0} stuff null: {1} {2} {3}", "PhysicsSyncer", view == null, rig == null, stream == null));
			if (stream != null)
			{
				if (stream.IsWriting)
				{
					stream.SendNext(Vector3.zero);
					stream.SendNext(Vector3.zero);
					stream.SendNext(Quaternion.identity);
					stream.SendNext(Vector3.zero);
				}
				else
				{
					Debug.Log($"PhysicsSyncer Err0: {stream.ReceiveNext()}");
					Debug.Log($"PhysicsSyncer Err1: {stream.ReceiveNext()}");
					Debug.Log($"PhysicsSyncer Err2: {stream.ReceiveNext()}");
					Debug.Log($"PhysicsSyncer Err3: {stream.ReceiveNext()}");
				}
			}
			return;
		}
		if (stream.IsWriting)
		{
			stream.SendNext(rig.position);
			stream.SendNext(rig.linearVelocity);
			stream.SendNext(rig.rotation);
			stream.SendNext(rig.angularVelocity);
			return;
		}
		object obj = stream.ReceiveNext();
		Vector3 vector;
		if (obj is Vector3)
		{
			vector = (Vector3)obj;
		}
		else
		{
			Debug.LogError("stream.ReceiveNext() did not produce a Vector3 pos");
			vector = Vector3.zero;
		}
		obj = stream.ReceiveNext();
		Vector3 linearVelocity;
		if (obj is Vector3)
		{
			linearVelocity = (Vector3)obj;
		}
		else
		{
			Debug.LogError("stream.ReceiveNext() did not produce a Vector3 vel");
			linearVelocity = Vector3.zero;
		}
		obj = stream.ReceiveNext();
		Quaternion quaternion;
		if (obj is Quaternion)
		{
			quaternion = (Quaternion)obj;
		}
		else
		{
			Debug.LogError("stream.ReceiveNext() did not produce a Vector3 rot");
			quaternion = Quaternion.identity;
		}
		obj = stream.ReceiveNext();
		Vector3 angularVelocity;
		if (obj is Vector3)
		{
			angularVelocity = (Vector3)obj;
		}
		else
		{
			Debug.LogError("stream.ReceiveNext() did not produce a Vector3 angVel");
			angularVelocity = Vector3.zero;
		}
		if (!hasRecieved)
		{
			hasRecieved = true;
			lastPos = vector;
			targetPos = vector;
		}
		lastPos = targetPos;
		targetPos = vector;
		rig.linearVelocity = linearVelocity;
		targetRot = quaternion;
		rig.angularVelocity = angularVelocity;
	}
}
