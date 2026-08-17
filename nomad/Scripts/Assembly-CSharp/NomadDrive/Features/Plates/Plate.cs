using System;
using System.Runtime.InteropServices;
using System.Text;
using Cysharp.Threading.Tasks;
using EvilCore.EvilSave;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using Mirror;
using NomadDrive.Features.Interaction;
using NomadDrive.Features.Plates.UI;
using NomadDrive.Features.SaveSystem;
using TMPro;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Plates
{
	public class Plate : HeldItem, INetworkSaveable
	{
		[Header("Plate")]
		[SerializeField]
		private PlateConfig plateConfig;

		[SerializeField]
		private TMP_Text plateCodeText;

		[SerializeField]
		private TMP_Text plateCityText;

		[SerializeField]
		private MeshRenderer plateFrameRenderer;

		[SyncVar]
		private byte _cityIndex;

		[SyncVar]
		private bool _isFoil;

		[SyncVar]
		private byte _qualityByte;

		[SyncVar(hook = "OnPlateCodeChanged")]
		private string _plateCode;

		[Inject]
		private ILocalizationService _localizationService;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		[Inject]
		private PlateInfoPanel _plateInfoPanel;

		private const string LetterPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private bool _serverInitialized;

		private bool _rareNotificationShown;

		public Action<string, string> _Mirror_SyncVarHookDelegate__plateCode;

		public string PlateCode => _plateCode;

		public bool IsFoil => _isFoil;

		public float Quality => (float)(int)_qualityByte / 255f;

		public string CityDisplayName
		{
			get
			{
				PlateConfig.CityEntry cityEntry = ((plateConfig != null) ? plateConfig.GetCity(_cityIndex) : null);
				if (cityEntry == null)
				{
					return string.Empty;
				}
				return cityEntry.cityName;
			}
		}

		public long CityOddsDenominator
		{
			get
			{
				if (!(plateConfig != null))
				{
					return 1L;
				}
				return plateConfig.GetCityOddsDenominator(_cityIndex);
			}
		}

		public PlateConfig.PatternEntry Pattern
		{
			get
			{
				if (!(plateConfig != null))
				{
					return null;
				}
				return plateConfig.ResolveBestPattern(_plateCode);
			}
		}

		public long PatternOddsDenominator
		{
			get
			{
				PlateConfig.PatternEntry pattern = Pattern;
				return (pattern == null) ? 1 : Math.Max(1, pattern.oddsDenominator);
			}
		}

		public long CumulativeOdds
		{
			get
			{
				long num = CityOddsDenominator * PatternOddsDenominator;
				if (_isFoil && plateConfig != null)
				{
					num *= Math.Max(1, plateConfig.FoilOddsDenominator);
				}
				return num;
			}
		}

		public PlateConfig.TierThreshold Tier
		{
			get
			{
				if (!(plateConfig != null))
				{
					return null;
				}
				return plateConfig.ResolveTier(CumulativeOdds);
			}
		}

		public string ContributorKey => "plate";

		public byte Network_cityIndex
		{
			get
			{
				return _cityIndex;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _cityIndex, 512uL, null);
			}
		}

		public bool Network_isFoil
		{
			get
			{
				return _isFoil;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _isFoil, 1024uL, null);
			}
		}

		public byte Network_qualityByte
		{
			get
			{
				return _qualityByte;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _qualityByte, 2048uL, null);
			}
		}

		public string Network_plateCode
		{
			get
			{
				return _plateCode;
			}
			[param: In]
			set
			{
				GeneratedSyncVarSetter(value, ref _plateCode, 4096uL, _Mirror_SyncVarHookDelegate__plateCode);
			}
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			FallbackRollIfUninitialized().Forget();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			ApplyVisuals();
		}

		[Server]
		public void ServerInitializeFromSeed(int seed)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Plates.Plate::ServerInitializeFromSeed(System.Int32)' called when server was not active");
			}
			else
			{
				RollFromSeed(seed);
			}
		}

		private async UniTaskVoid FallbackRollIfUninitialized()
		{
			await UniTask.NextFrame(this.GetCancellationTokenOnDestroy());
			if (!_serverInitialized)
			{
				RollFromSeed((int)base.netId);
			}
		}

		[Server]
		private void RollFromSeed(int seed)
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void NomadDrive.Features.Plates.Plate::RollFromSeed(System.Int32)' called when server was not active");
				return;
			}
			System.Random random = new System.Random(seed);
			string network_plateCode = GenerateCode(random);
			byte network_cityIndex = (byte)((plateConfig != null) ? ((byte)plateConfig.SelectCityIndex(random)) : 0);
			bool network_isFoil = plateConfig != null && random.NextDouble() * 100.0 < (double)plateConfig.FoilChancePercent;
			byte network_qualityByte = (byte)Mathf.Clamp(Mathf.RoundToInt((float)random.NextDouble() * 255f), 0, 255);
			Network_cityIndex = network_cityIndex;
			Network_isFoil = network_isFoil;
			Network_qualityByte = network_qualityByte;
			Network_plateCode = network_plateCode;
			_serverInitialized = true;
			ApplyVisuals();
		}

		private static string GenerateCode(System.Random random)
		{
			StringBuilder stringBuilder = new StringBuilder(7);
			stringBuilder.Append((char)(48 + random.Next(0, 10)));
			for (int i = 0; i < 3; i++)
			{
				stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length)]);
			}
			for (int j = 0; j < 3; j++)
			{
				stringBuilder.Append((char)(48 + random.Next(0, 10)));
			}
			return stringBuilder.ToString();
		}

		private void OnPlateCodeChanged(string oldValue, string newValue)
		{
			if (IsLateJoinCompleted)
			{
				ApplyVisuals();
			}
		}

		private void ApplyVisuals()
		{
			if (string.IsNullOrEmpty(_plateCode))
			{
				return;
			}
			if (plateCodeText != null)
			{
				plateCodeText.text = _plateCode;
			}
			if (plateCityText != null)
			{
				plateCityText.text = CityDisplayName;
			}
			float dilate = Mathf.Lerp((plateConfig != null) ? plateConfig.MinFaceDilate : (-0.2f), (plateConfig != null) ? plateConfig.MaxFaceDilate : 0.12f, Quality);
			ApplyFaceDilate(plateCodeText, dilate);
			ApplyFaceDilate(plateCityText, dilate);
			if (plateFrameRenderer != null && plateConfig != null)
			{
				Material material = (_isFoil ? plateConfig.FoilPlateMaterial : plateConfig.NormalPlateMaterial);
				if (material != null)
				{
					plateFrameRenderer.material = material;
				}
			}
		}

		private static void ApplyFaceDilate(TMP_Text text, float dilate)
		{
			if (!(text == null))
			{
				Material fontMaterial = text.fontMaterial;
				if (!(fontMaterial == null) && fontMaterial.HasProperty(ShaderUtilities.ID_FaceDilate))
				{
					fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, dilate);
					text.UpdateMeshPadding();
				}
			}
		}

		protected override void OnHovered()
		{
			base.OnHovered();
			_plateInfoPanel?.SetPlate(this);
			TryShowRareNotification();
		}

		protected override void OnUnhovered()
		{
			base.OnUnhovered();
			_plateInfoPanel?.SetPlate(null);
		}

		private void TryShowRareNotification()
		{
			if (!_rareNotificationShown && !(plateConfig == null) && !(_uiFeedbackManager == null) && _localizationService != null)
			{
				PlateConfig.TierThreshold tier = Tier;
				if (tier != null && (int)tier.tier >= (int)plateConfig.RareNotificationMinTier)
				{
					_rareNotificationShown = true;
					string text = _localizationService.Localize(tier.nameKey);
					string cityDisplayName = CityDisplayName;
					string message = _localizationService.Localize("@plate.rare_found", text, cityDisplayName);
					_uiFeedbackManager.CreateFloatingMessage(message, FeedbackType.Success);
				}
			}
		}

		public void CaptureState(EvilWriter writer, ISaveContext ctx)
		{
			writer.Write((byte)1);
			writer.Write(_cityIndex);
			writer.Write(_isFoil);
			writer.Write(_qualityByte);
			writer.Write(_plateCode ?? string.Empty);
		}

		public void RestoreSelfState(EvilReader reader, ISaveContext ctx)
		{
			reader.ReadByte();
			byte network_cityIndex = reader.ReadByte();
			bool network_isFoil = reader.ReadBool();
			byte network_qualityByte = reader.ReadByte();
			string network_plateCode = reader.ReadString();
			if (NetworkServer.active)
			{
				Network_cityIndex = network_cityIndex;
				Network_isFoil = network_isFoil;
				Network_qualityByte = network_qualityByte;
				Network_plateCode = network_plateCode;
				_serverInitialized = true;
				ApplyVisuals();
			}
		}

		public void RestoreLinks(EvilReader reader, ISaveContext ctx)
		{
		}

		public Plate()
		{
			_Mirror_SyncVarHookDelegate__plateCode = OnPlateCodeChanged;
		}

		public override bool Weaved()
		{
			return true;
		}

		public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
		{
			base.SerializeSyncVars(writer, forceAll);
			if (forceAll)
			{
				NetworkWriterExtensions.WriteByte(writer, _cityIndex);
				writer.WriteBool(_isFoil);
				NetworkWriterExtensions.WriteByte(writer, _qualityByte);
				writer.WriteString(_plateCode);
				return;
			}
			writer.WriteVarULong(syncVarDirtyBits);
			if ((syncVarDirtyBits & 0x200L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _cityIndex);
			}
			if ((syncVarDirtyBits & 0x400L) != 0L)
			{
				writer.WriteBool(_isFoil);
			}
			if ((syncVarDirtyBits & 0x800L) != 0L)
			{
				NetworkWriterExtensions.WriteByte(writer, _qualityByte);
			}
			if ((syncVarDirtyBits & 0x1000L) != 0L)
			{
				writer.WriteString(_plateCode);
			}
		}

		public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
			base.DeserializeSyncVars(reader, initialState);
			if (initialState)
			{
				GeneratedSyncVarDeserialize(ref _cityIndex, null, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _isFoil, null, reader.ReadBool());
				GeneratedSyncVarDeserialize(ref _qualityByte, null, NetworkReaderExtensions.ReadByte(reader));
				GeneratedSyncVarDeserialize(ref _plateCode, _Mirror_SyncVarHookDelegate__plateCode, reader.ReadString());
				return;
			}
			long num = (long)reader.ReadVarULong();
			if ((num & 0x200L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _cityIndex, null, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x400L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _isFoil, null, reader.ReadBool());
			}
			if ((num & 0x800L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _qualityByte, null, NetworkReaderExtensions.ReadByte(reader));
			}
			if ((num & 0x1000L) != 0L)
			{
				GeneratedSyncVarDeserialize(ref _plateCode, _Mirror_SyncVarHookDelegate__plateCode, reader.ReadString());
			}
		}
	}
}
