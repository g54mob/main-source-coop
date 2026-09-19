using System;
using System.Collections.Generic;

namespace Features.MultiplayerSessionServices.Scripts
{
	[Serializable]
	public class PlayerSessionData
	{
		public string SessionName;

		public float PositionX;

		public float PositionY;

		public float PositionZ;

		public bool IsCrouching;

		public bool HasPositionSaved;

		public int LevelId;

		public float Health;

		public bool HasHealthSaved;

		public int RecoveryPhase;

		public bool IsDeadPartSpawned;

		public bool HasCustomizationSaved;

		public int HatPartSkinId;

		public int TorsoPartSkinId;

		public int BottomPartSkinId;

		public bool IsFullSkin;

		public int ButtTexturePreset;

		public int ButtMeshPreset;

		public float VariableColorR;

		public float VariableColorG;

		public float VariableColorB;

		public float VariableColorA;

		public bool HasVariableColorSaved;

		public bool HasRumsSaved;

		public List<RumSessionEntry> ActiveRums = new List<RumSessionEntry>();

		public bool IsError;

		public int RecoveryReason;

		public int RecoveryResult;

		public int RecoveryAttemptCount;

		public string RecoveryRequestedUtc;

		public void ResetSessionScopedData()
		{
			HasPositionSaved = false;
			PositionX = 0f;
			PositionY = 0f;
			PositionZ = 0f;
			IsCrouching = false;
			LevelId = 0;
			Health = 0f;
			HasHealthSaved = false;
			IsDeadPartSpawned = false;
			HasCustomizationSaved = false;
			HatPartSkinId = 0;
			TorsoPartSkinId = 0;
			BottomPartSkinId = 0;
			IsFullSkin = false;
			ButtTexturePreset = 0;
			ButtMeshPreset = 0;
			VariableColorR = 0f;
			VariableColorG = 0f;
			VariableColorB = 0f;
			VariableColorA = 0f;
			HasVariableColorSaved = false;
			HasRumsSaved = false;
			ActiveRums = new List<RumSessionEntry>();
		}
	}
}
