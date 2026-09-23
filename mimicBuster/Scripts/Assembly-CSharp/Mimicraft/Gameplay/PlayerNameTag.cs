using Mimicraft.Networking;
using Mimicraft.Voice;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.Gameplay
{
	[RequireComponent(typeof(NetworkObject))]
	public class PlayerNameTag : NetworkBehaviour
	{
		public const float HeightAboveFeet = 2.1f;

		private const float ClearanceAboveBody = 0.35f;

		private const float FontSize = 3f;

		private const float MaxVisibleDistance = 40f;

		[SerializeField]
		private TextMeshPro label;

		[Tooltip("Konuşan bir oyuncunun ismi bu kalıba sokulur. {0} isim. Varsayılan ASCII - TMP'nin standart fontunda kesinlikle var. Bir sprite asset eklersen \"<sprite=0> {0}\" gibi bir şeyle değiştirebilirsin.")]
		[SerializeField]
		private string speakingFormat = "{0} *";

		[Tooltip("Herkese konuşurken ismin rengi.")]
		[SerializeField]
		private Color speakingAllColor = new Color(1f, 0.93f, 0.6f);

		[Tooltip("Takımına konuşurken ismin rengi.")]
		[SerializeField]
		private Color speakingTeamColor = new Color(0.55f, 1f, 0.6f);

		private GameModeController roundManager;

		private PlayerVoxelBody voxelBody;

		private bool appliedVisible;

		private string appliedCaption;

		private Color appliedColor;

		private bool appliedStyle;

		private Color restingColor = Color.white;

		private int appliedBodyVersion = -1;

		private bool appliedOnce;

		private void Awake()
		{
			if (label == null)
			{
				label = BuildLabel();
			}
			restingColor = label.color;
			label.text = string.Empty;
			label.gameObject.SetActive(value: false);
		}

		private TextMeshPro BuildLabel()
		{
			GameObject gameObject = new GameObject("NameTag");
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			gameObject.transform.localPosition = Vector3.up * 2.1f;
			TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
			textMeshPro.fontSize = 3f;
			textMeshPro.alignment = TextAlignmentOptions.Center;
			textMeshPro.color = Color.white;
			textMeshPro.textWrappingMode = TextWrappingModes.NoWrap;
			gameObject.SetActive(value: false);
			return textMeshPro;
		}

		private void LateUpdate()
		{
			if (label == null || !base.IsSpawned)
			{
				return;
			}
			if (roundManager == null)
			{
				roundManager = GameModeController.Current;
				if (roundManager == null)
				{
					return;
				}
			}
			Camera main = Camera.main;
			bool flag = main != null && ShouldShow(main);
			if (!appliedOnce || flag != appliedVisible)
			{
				appliedOnce = true;
				appliedVisible = flag;
				label.gameObject.SetActive(flag);
			}
			if (flag)
			{
				UpdateHeight();
				string playerName = roundManager.GetPlayerName(base.OwnerClientId);
				VoiceSpeakers.Speaker speaker;
				bool num = VoiceSpeakers.TryGet(base.OwnerClientId, out speaker);
				string text = ((num && !string.IsNullOrEmpty(speakingFormat)) ? string.Format(speakingFormat, playerName) : playerName);
				Color color = ((!num) ? restingColor : ((speaker.Channel == VoiceChannel.Team) ? speakingTeamColor : speakingAllColor));
				if (!appliedStyle || text != appliedCaption)
				{
					appliedCaption = text;
					label.text = text;
				}
				if (!appliedStyle || color != appliedColor)
				{
					appliedColor = color;
					label.color = color;
				}
				appliedStyle = true;
				Transform obj = label.transform;
				obj.rotation = Quaternion.LookRotation(obj.position - main.transform.position);
			}
		}

		private void UpdateHeight()
		{
			if (voxelBody == null)
			{
				voxelBody = GetComponent<PlayerVoxelBody>();
				if (voxelBody == null)
				{
					appliedBodyVersion = 0;
					return;
				}
			}
			if (appliedBodyVersion != voxelBody.BodyVersion)
			{
				appliedBodyVersion = voxelBody.BodyVersion;
				float topY;
				float y = (voxelBody.TryGetBodyTopLocalY(out topY) ? (topY + 0.35f) : 2.1f);
				Vector3 localPosition = label.transform.localPosition;
				label.transform.localPosition = new Vector3(localPosition.x, y, localPosition.z);
			}
		}

		private bool ShouldShow(Camera cam)
		{
			if ((cam.transform.position - base.transform.position).sqrMagnitude > 1600f)
			{
				return false;
			}
			if (RoundCinematicDirector.IsPlaying)
			{
				return true;
			}
			if (base.IsOwner)
			{
				return false;
			}
			return roundManager.ShouldShowNameTag(base.OwnerClientId);
		}

		protected override void __initializeVariables()
		{
			base.__initializeVariables();
		}

		protected override void __initializeRpcs()
		{
			base.__initializeRpcs();
		}

		protected internal override string __getTypeName()
		{
			return "PlayerNameTag";
		}
	}
}
