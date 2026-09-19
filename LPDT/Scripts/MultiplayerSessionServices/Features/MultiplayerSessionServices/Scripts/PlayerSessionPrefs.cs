using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts
{
	public static class PlayerSessionPrefs
	{
		private static string _instanceSuffix = "_" + Application.dataPath.GetHashCode();

		private static PlayerSessionData _data;

		private static bool _isPositionSavingSuspended;

		private static string FilePath => Path.Combine(Application.persistentDataPath, "session_data" + _instanceSuffix + ".dat");

		private static string TempFilePath => FilePath + ".tmp";

		private static PlayerSessionData CurrentData => _data ?? (_data = LoadFromDisk());

		public static bool HasSavedHealth()
		{
			return CurrentData.HasHealthSaved;
		}

		public static void OverrideInstanceKey(string instanceKey)
		{
			_instanceSuffix = "_" + instanceKey;
			_data = null;
		}

		private static PlayerSessionData LoadFromDisk()
		{
			try
			{
				if (!File.Exists(FilePath))
				{
					return new PlayerSessionData();
				}
				return JsonUtility.FromJson<PlayerSessionData>(File.ReadAllText(FilePath)) ?? new PlayerSessionData();
			}
			catch (Exception)
			{
				return new PlayerSessionData();
			}
		}

		private static void Flush()
		{
			try
			{
				string contents = JsonUtility.ToJson(_data);
				File.WriteAllText(TempFilePath, contents);
				File.Copy(TempFilePath, FilePath, overwrite: true);
				File.Delete(TempFilePath);
			}
			catch (Exception)
			{
			}
		}

		public static void Save(string sessionName)
		{
			if (!string.IsNullOrEmpty(sessionName))
			{
				PlayerSessionData currentData = CurrentData;
				if (currentData.SessionName != sessionName)
				{
					currentData.ResetSessionScopedData();
				}
				currentData.SessionName = sessionName;
				currentData.IsError = true;
				Flush();
			}
		}

		public static void SavePosition(Vector3 position)
		{
			PlayerSessionData currentData = CurrentData;
			currentData.PositionX = position.x;
			currentData.PositionY = position.y;
			currentData.PositionZ = position.z;
			currentData.HasPositionSaved = true;
			Flush();
		}

		public static void ClearSavedPosition()
		{
			PlayerSessionData currentData = CurrentData;
			currentData.HasPositionSaved = false;
			currentData.PositionX = 0f;
			currentData.PositionY = 0f;
			currentData.PositionZ = 0f;
			Flush();
		}

		public static void SetPositionSavingSuspended(bool isSuspended)
		{
			_isPositionSavingSuspended = isSuspended;
		}

		public static bool IsPositionSavingSuspended()
		{
			return _isPositionSavingSuspended;
		}

		public static void SaveCrouching(bool isCrouching)
		{
			CurrentData.IsCrouching = isCrouching;
			Flush();
		}

		public static bool IsSavedCrouching()
		{
			return CurrentData.IsCrouching;
		}

		public static void SaveHealth(float health)
		{
			PlayerSessionData currentData = CurrentData;
			currentData.Health = health;
			currentData.HasHealthSaved = true;
			Flush();
		}

		public static void SetCurrentLevel(int levelId)
		{
			CurrentData.LevelId = levelId;
			Flush();
		}

		public static void SetRecoveryPhase(SessionRecoveryPhase phase)
		{
			CurrentData.RecoveryPhase = (int)phase;
			Flush();
		}

		public static SessionRecoveryPhase GetRecoveryPhase()
		{
			return (SessionRecoveryPhase)CurrentData.RecoveryPhase;
		}

		public static void ClearRecoveryPhase()
		{
			CurrentData.RecoveryPhase = 0;
			Flush();
		}

		public static bool IsError()
		{
			return CurrentData.IsError;
		}

		public static void RecordRecoveryRequest(int reason)
		{
			PlayerSessionData currentData = CurrentData;
			bool flag = currentData.RecoveryResult == 1 || currentData.RecoveryResult == 4;
			currentData.RecoveryAttemptCount = ((!flag) ? 1 : (currentData.RecoveryAttemptCount + 1));
			currentData.RecoveryReason = reason;
			currentData.RecoveryResult = 1;
			currentData.RecoveryRequestedUtc = DateTime.UtcNow.ToString("o");
			Flush();
		}

		public static bool TryGetPendingRecovery(out int reason, out int attemptCount)
		{
			PlayerSessionData currentData = CurrentData;
			reason = currentData.RecoveryReason;
			attemptCount = currentData.RecoveryAttemptCount;
			return currentData.RecoveryResult == 1;
		}

		public static bool IsRecoveryInFlight()
		{
			int recoveryResult = CurrentData.RecoveryResult;
			if (recoveryResult != 1)
			{
				return recoveryResult == 4;
			}
			return true;
		}

		public static void SetRecoveryResult(SessionRecoveryResult result)
		{
			CurrentData.RecoveryResult = (int)result;
			Flush();
		}

		public static void ClearRecovery()
		{
			PlayerSessionData currentData = CurrentData;
			currentData.RecoveryReason = 0;
			currentData.RecoveryResult = 0;
			currentData.RecoveryAttemptCount = 0;
			currentData.RecoveryRequestedUtc = null;
			Flush();
		}

		public static void SetDeadPartSpawned(bool isSpawned)
		{
			CurrentData.IsDeadPartSpawned = isSpawned;
			Flush();
		}

		public static bool IsDeadPartSpawned()
		{
			return CurrentData.IsDeadPartSpawned;
		}

		public static void PrepareForLobbyLeave(string sessionName)
		{
			PlayerSessionData currentData = CurrentData;
			if (!string.IsNullOrEmpty(sessionName))
			{
				currentData.SessionName = sessionName;
				currentData.IsError = true;
			}
			currentData.RecoveryPhase = 1;
			currentData.ResetSessionScopedData();
			Flush();
		}

		public static void Clear()
		{
			_data = null;
			try
			{
				if (File.Exists(FilePath))
				{
					File.Delete(FilePath);
				}
				if (File.Exists(TempFilePath))
				{
					File.Delete(TempFilePath);
				}
			}
			catch (Exception)
			{
			}
		}

		public static void ClearReconnectTrigger()
		{
			CurrentData.SessionName = null;
			Flush();
		}

		public static void ClearErrorTrigger()
		{
			CurrentData.IsError = false;
			Flush();
		}

		public static bool TryGetSavedSessionName(out string sessionName)
		{
			string sessionName2 = CurrentData.SessionName;
			if (string.IsNullOrEmpty(sessionName2))
			{
				sessionName = null;
				return false;
			}
			sessionName = sessionName2;
			return true;
		}

		public static bool TryGetSavedPosition(out Vector3 position)
		{
			PlayerSessionData currentData = CurrentData;
			if (!currentData.HasPositionSaved)
			{
				position = default(Vector3);
				return false;
			}
			position = new Vector3(currentData.PositionX, currentData.PositionY, currentData.PositionZ);
			return true;
		}

		public static bool HasSavedReconnectPosition(int currentLevelId)
		{
			PlayerSessionData currentData = CurrentData;
			if (currentData.HasPositionSaved)
			{
				return currentData.LevelId == currentLevelId;
			}
			return false;
		}

		public static int GetSavedLevelId()
		{
			return CurrentData.LevelId;
		}

		public static bool HasSavedPosition()
		{
			return CurrentData.HasPositionSaved;
		}

		public static void SaveCustomization(int hatPartSkinId, int torsoPartSkinId, int bottomPartSkinId, bool isFullSkin, int buttTexturePreset, int buttMeshPreset, Color variableColor)
		{
			PlayerSessionData currentData = CurrentData;
			currentData.HasCustomizationSaved = true;
			currentData.HatPartSkinId = hatPartSkinId;
			currentData.TorsoPartSkinId = torsoPartSkinId;
			currentData.BottomPartSkinId = bottomPartSkinId;
			currentData.IsFullSkin = isFullSkin;
			currentData.ButtTexturePreset = buttTexturePreset;
			currentData.ButtMeshPreset = buttMeshPreset;
			currentData.VariableColorR = variableColor.r;
			currentData.VariableColorG = variableColor.g;
			currentData.VariableColorB = variableColor.b;
			currentData.VariableColorA = variableColor.a;
			currentData.HasVariableColorSaved = true;
			Flush();
		}

		public static bool TryGetSavedCustomization(out int hatPartSkinId, out int torsoPartSkinId, out int bottomPartSkinId, out bool isFullSkin, out int buttTexturePreset, out int buttMeshPreset, out Color variableColor, out bool hasVariableColor)
		{
			PlayerSessionData currentData = CurrentData;
			hatPartSkinId = currentData.HatPartSkinId;
			torsoPartSkinId = currentData.TorsoPartSkinId;
			bottomPartSkinId = currentData.BottomPartSkinId;
			isFullSkin = currentData.IsFullSkin;
			buttTexturePreset = currentData.ButtTexturePreset;
			buttMeshPreset = currentData.ButtMeshPreset;
			variableColor = new Color(currentData.VariableColorR, currentData.VariableColorG, currentData.VariableColorB, currentData.VariableColorA);
			hasVariableColor = currentData.HasVariableColorSaved;
			return currentData.HasCustomizationSaved;
		}

		public static void SaveActiveRums(List<RumSessionEntry> activeRums)
		{
			PlayerSessionData currentData = CurrentData;
			currentData.ActiveRums = activeRums ?? new List<RumSessionEntry>();
			currentData.HasRumsSaved = true;
			Flush();
		}

		public static bool TryGetSavedRums(out List<RumSessionEntry> activeRums)
		{
			PlayerSessionData currentData = CurrentData;
			activeRums = currentData.ActiveRums ?? new List<RumSessionEntry>();
			if (currentData.HasRumsSaved)
			{
				return activeRums.Count > 0;
			}
			return false;
		}

		public static void ClearActiveRums()
		{
			PlayerSessionData currentData = CurrentData;
			currentData.HasRumsSaved = false;
			currentData.ActiveRums = new List<RumSessionEntry>();
			Flush();
		}

		public static bool TryGetSavedHealth(out float health)
		{
			PlayerSessionData currentData = CurrentData;
			if (!currentData.HasHealthSaved || currentData.Health <= 0f)
			{
				health = 0f;
				return false;
			}
			health = currentData.Health;
			return true;
		}
	}
}
