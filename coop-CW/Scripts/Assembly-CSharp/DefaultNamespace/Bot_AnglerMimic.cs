using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace DefaultNamespace
{
	public class Bot_AnglerMimic : MonoBehaviour
	{
		public bool avoidingPlayer;

		public float avoidPlayerDistance = 10f;

		public bool tauntingPlayer;

		public float tauntDistance = 5f;

		public float runFromPlayerDistance = 3f;

		public float runToPlayerDistance = 5f;

		public float customRotationSpeed = 6f;

		public float minTimeBeforeWalkSprintSwitch = 1f;

		public float minTimeBetweenJumps = 10f;

		public float maxTimeBetweenJumps = 10f;

		[HideInInspector]
		public Player player;

		private Bot_Angler angler;

		private Bot bot;

		private FakeFlashLight fakeFlashLight_grc;

		private float timeSinceWalkSprintSwitch;

		private float timeToNextJump;

		private PhotonView view;

		public float DistToTarget => HelperFunctions.FlatDistance(base.transform.position, angler.bot.targetPlayer.Center());

		public bool Sprint
		{
			get
			{
				return bot.syncData.sprint;
			}
			set
			{
				if (value != Sprint && !(timeSinceWalkSprintSwitch < minTimeBeforeWalkSprintSwitch))
				{
					timeSinceWalkSprintSwitch = Random.Range(minTimeBeforeWalkSprintSwitch * -0.5f, minTimeBeforeWalkSprintSwitch * 0.5f);
					bot.syncData.sprint = value;
				}
			}
		}

		public bool IsFlashLightOn
		{
			get
			{
				if (angler.mimicingPlayer == null)
				{
					return true;
				}
				return angler.mimicingPlayer.HasFlashLightThatIsOn();
			}
		}

		public bool AvoidingPlayer
		{
			get
			{
				if (angler.bot.targetPlayer != null)
				{
					return avoidingPlayer;
				}
				return false;
			}
			set
			{
				avoidingPlayer = value;
			}
		}

		public bool TauntingPlayer
		{
			get
			{
				if (angler.bot.targetPlayer != null)
				{
					return tauntingPlayer;
				}
				return false;
			}
			set
			{
				tauntingPlayer = value;
			}
		}

		public bool HasAnglerDaddy => angler != null;

		private void Start()
		{
			player = GetComponentInParent<Player>();
			view = GetComponent<PhotonView>();
			bot = GetComponent<Bot>();
			fakeFlashLight_grc = base.transform.root.GetComponentInChildren<FakeFlashLight>();
		}

		[PunRPC]
		private void RPCA_ToggleFlashLight(bool on)
		{
			fakeFlashLight_grc.Toggle(on);
		}

		private void Update()
		{
			if (!HasAnglerDaddy)
			{
				bot.StandStill();
				return;
			}
			if (angler.m_RemoteMimic != null && angler.m_RemoteMimic.MimicTarget != null)
			{
				Debug.DrawLine(base.transform.position, angler.m_RemoteMimic.MimicTarget.RemoteVoice.transform.position, Color.red);
			}
			if (!player.refs.view.IsMine)
			{
				return;
			}
			bool isFlashLightOn = IsFlashLightOn;
			if (isFlashLightOn != fakeFlashLight_grc.isOn)
			{
				view.RPC("RPCA_ToggleFlashLight", RpcTarget.All, isFlashLightOn);
			}
			if (angler.mimicingPlayer != null && !angler.mimicingPlayer.data.isGrounded && angler.mimicingPlayer.data.sinceJump < 0.2f)
			{
				Jump();
			}
			timeToNextJump -= Time.deltaTime;
			timeSinceWalkSprintSwitch += Time.deltaTime;
			if (timeToNextJump < 0f)
			{
				Jump();
			}
			if (angler.bot.targetPlayer == null)
			{
				Debug.Log("angler.bot.targetPlaye == null");
				Vector3 vector = angler.bot.syncData.lookDireciton * angler.defaultMimicDistance;
				Vector3 vector2 = angler.transform.position + vector;
				vector2 = HelperFunctions.GetGroundPos(vector2 + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp);
				bot.navTargetPos_Set = vector2;
				Debug.DrawLine(base.transform.position, bot.navTargetPos_Set, Color.blue);
				if (Vector3.Distance(base.transform.position, bot.navTargetPos_Set) < 1f)
				{
					bot.StandStill();
				}
				else
				{
					bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
				}
				Sprint = true;
			}
			else if (!TauntingPlayer)
			{
				if (AvoidingPlayer)
				{
					Vector3 position = angler.suckPoint.transform.position;
					position = HelperFunctions.GetGroundPos(position + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp);
					bot.navTargetPos_Set = position;
					Debug.DrawLine(base.transform.position, bot.navTargetPos_Set, Color.yellow);
					if (DistToTarget < runFromPlayerDistance)
					{
						Sprint = true;
					}
					else
					{
						Sprint = false;
					}
				}
				else
				{
					Vector3 vector3 = angler.bot.targetPlayer.CenterGroundPos();
					vector3 = HelperFunctions.GetGroundPos(vector3 + Vector3.up * 1f, HelperFunctions.LayerType.TerrainProp);
					bot.navTargetPos_Set = vector3;
					Debug.DrawLine(base.transform.position, bot.navTargetPos_Set, Color.green);
					if (HelperFunctions.FlatDistance(vector3, base.transform.position) - (avoidPlayerDistance + tauntDistance) > runToPlayerDistance)
					{
						Sprint = true;
					}
					else
					{
						Sprint = false;
					}
				}
				bot.RotateThenMove(bot.navDirection_Read, customRotationSpeed);
			}
			else
			{
				bot.StandStill();
				bot.LookAt(angler.bot.targetPlayer.Center());
			}
			if (angler.bot.targetPlayer == null)
			{
				avoidingPlayer = false;
				tauntingPlayer = false;
			}
			else if (!avoidingPlayer && DistToTarget < avoidPlayerDistance)
			{
				Debug.Log("!avoidingPlayer");
				avoidingPlayer = true;
				tauntingPlayer = false;
			}
			else if (avoidingPlayer && DistToTarget > avoidPlayerDistance + tauntDistance - 1f)
			{
				Debug.Log("avoidingPlayer");
				avoidingPlayer = false;
				tauntingPlayer = false;
			}
			else if (avoidingPlayer)
			{
				tauntingPlayer = false;
			}
			else if (!tauntingPlayer && DistToTarget < avoidPlayerDistance + tauntDistance)
			{
				Debug.Log("!tauntingPlayer");
				tauntingPlayer = true;
			}
			else if (tauntingPlayer && DistToTarget > avoidPlayerDistance + tauntDistance * 2f)
			{
				Debug.Log("tauntingPlayer");
				tauntingPlayer = false;
			}
		}

		public void AssignAngler(Bot_Angler botAngler)
		{
			angler = botAngler;
		}

		private void FindAngler()
		{
			float num = float.MaxValue;
			foreach (Bot_Angler item in Object.FindObjectsByType<Bot_Angler>(FindObjectsSortMode.None).ToList())
			{
				float num2 = Vector3.Distance(item.transform.position, base.transform.position);
				if (num2 < num)
				{
					num = num2;
					angler = item;
				}
			}
		}

		private void Jump()
		{
			timeToNextJump = Random.Range(minTimeBetweenJumps, maxTimeBetweenJumps);
			view.RPC("RPCA_MimicJump", RpcTarget.All);
		}

		[PunRPC]
		private void RPCA_MimicJump()
		{
			player.refs.controller.TryJump();
		}
	}
}
