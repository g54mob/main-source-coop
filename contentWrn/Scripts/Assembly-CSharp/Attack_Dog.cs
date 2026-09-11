using Photon.Pun;
using UnityEngine;

public class Attack_Dog : MonoBehaviour
{
	public Light light;

	public Transform beam;

	private Player player;

	private Bot bot;

	private Bodypart hip;

	private Bodypart neck;

	private Bodypart head;

	public GameObject projectile;

	public float fireRate = 0.1f;

	public float spread;

	public Transform firePos;

	private PhotonView view;

	private float counter;

	private float aimLevel;

	public SFX_Instance targetSfx;

	private bool t;

	private void Start()
	{
		bot = GetComponent<Bot>();
		view = GetComponent<PhotonView>();
		player = GetComponentInParent<Player>();
		hip = player.refs.ragdoll.GetBodypart(BodypartType.Hip);
		neck = player.refs.ragdoll.GetBodypart(BodypartType.Neck);
		head = player.refs.ragdoll.GetBodypart(BodypartType.Head);
	}

	private void Update()
	{
		if ((bool)bot.targetPlayer && bot.CanSee(head.rig.position, bot.targetPlayer.HeadPosition(), 40f, 200f))
		{
			aimLevel = Mathf.MoveTowards(aimLevel, 3f, Time.deltaTime);
			if (!t)
			{
				targetSfx.Play(hip.transform.position);
				t = true;
			}
			AimAtTarget();
			if (view.IsMine)
			{
				Attack();
			}
		}
		else
		{
			aimLevel = Mathf.MoveTowards(aimLevel, 0f, Time.deltaTime * 2f);
			t = false;
		}
		HandleLight();
	}

	private void HandleLight()
	{
		float num = Mathf.Lerp(12.5f, 0.5f, aimLevel);
		beam.transform.localScale = new Vector3(num, num, 12.5f);
		light.spotAngle = Mathf.Lerp(67f, 10f, aimLevel);
	}

	private void Attack()
	{
		counter += Time.deltaTime;
		if (!(aimLevel < 2.99f) && !(counter < fireRate))
		{
			counter = 0f;
			view.RPC("RPCA_DogFire", RpcTarget.All, firePos.position, firePos.forward);
		}
	}

	[PunRPC]
	public void RPCA_DogFire(Vector3 pos, Vector3 dir)
	{
		Quaternion rotation = Quaternion.LookRotation(dir);
		rotation *= Quaternion.Euler(Random.insideUnitSphere * spread);
		Object.Instantiate(projectile, pos, rotation).GetComponent<Projectile>().Ignore(base.transform.root, 1f);
	}

	private void AimAtTarget()
	{
		Bodypart bodypart = player.refs.ragdoll.GetBodypart(BodypartType.Hip);
		Bodypart bodypart2 = player.refs.ragdoll.GetBodypart(BodypartType.Neck);
		Bodypart bodypart3 = player.refs.ragdoll.GetBodypart(BodypartType.Head);
		Vector3 position = bot.targetPlayer.Center() + Vector3.up * 0.5f;
		Vector3 position2 = bodypart.rig.transform.InverseTransformPoint(position);
		Vector3 vector = bodypart.animationTarget.transform.TransformPoint(position2);
		Vector3 vector2 = vector - bodypart2.animationTarget.transform.position;
		Vector3 vector3 = vector - bodypart3.animationTarget.transform.position;
		Vector3 vector4 = Vector3.Cross(bodypart2.animationTarget.transform.up, vector2).normalized * Vector3.Angle(bodypart2.animationTarget.transform.up, vector2);
		vector4 = Vector3.Project(vector4, bodypart2.animationTarget.transform.forward);
		Vector3 vector5 = Vector3.Cross(bodypart3.animationTarget.transform.up, vector3).normalized * Vector3.Angle(bodypart3.animationTarget.transform.up, vector3);
		vector5 = Vector3.Project(vector5, bodypart3.animationTarget.transform.right);
		bodypart2.animationTarget.transform.Rotate(vector4, Space.World);
		bodypart2.animationTarget.transform.localEulerAngles = new Vector3(0f, 0f, bodypart2.animationTarget.transform.localEulerAngles.z);
		bodypart3.animationTarget.transform.Rotate(vector5, Space.World);
		bodypart3.animationTarget.transform.localEulerAngles = new Vector3(bodypart3.animationTarget.transform.localEulerAngles.x, 0f, 0f);
	}
}
