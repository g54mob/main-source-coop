using System;
using System.Collections;
using UnityEngine;

namespace Portningsbolaget.Platforms
{
	public interface IPlatform
	{
		bool Initialized => true;

		string NickName => "PlayerName";

		ulong UserID => (ulong)UnityEngine.Random.Range(1000, int.MaxValue);

		string FileOutputDirectory => string.Empty;

		PlatformUtility.PlatformFamily PlatformFamily => PlatformUtility.PlatformFamily.None;

		Action<string, string> OnJoinedSession { get; set; }

		void InitializeAfterAssembliesLoaded();

		void InitializeAfterSceneLoad();

		void UnlockAchievement(Achievements achievement);

		void ProgressAchievement(Achievements achievement);

		void ProgressAchievement(Achievements achievement, int amount)
		{
		}

		void SetAchievementProgress(Achievements achievement, int progress)
		{
		}

		void ClearAchievementsProgress();

		void OpenDialog(string title, DialogType dialogType, Action<string, DialogueResult> onDone)
		{
		}

		void Teardown();

		void Update();

		void VerifyString(string stringToVerify, Action<string> callback)
		{
			callback?.Invoke(stringToVerify);
		}

		void RequestSaveExists(string fileName, Action<FileResult> onSaveOperationFinished)
		{
		}

		void RequestSaveExists(string fileName, int saveSlot, Action<FileResult> onSaveOperationFinished)
		{
		}

		void RequestSave(string fileName, byte[] dataToSave, Action<FileResult> onSaveOperationFinished)
		{
		}

		void RequestSave(string fileName, byte[] dataToSave, int saveSlot, Action<FileResult> onSaveOperationFinished)
		{
		}

		void RequestLoadAsync(string fileName, Action<FileResult, byte[]> onLoadOperationFinished)
		{
		}

		void RequestLoadAsync(string fileName, int saveSlot, Action<FileResult, byte[]> onLoadOperationFinished)
		{
		}

		IEnumerator RequestLoad(string fileName, Action<FileResult, byte[]> onLoadOperationFinished)
		{
			yield break;
		}

		IEnumerator RequestLoad(string fileName, int saveSlot, Action<FileResult, byte[]> onLoadOperationFinished)
		{
			yield break;
		}

		void RequestDelete(string fileName, Action<FileResult> onDeleteOperationFinished)
		{
		}

		void RequestDelete(string fileName, int saveSlot, Action<FileResult> onDeleteOperationFinished)
		{
		}

		DateTime GetSaveDate(string fileName)
		{
			return DateTime.MinValue;
		}

		void UpdateSaveDate(string fileName)
		{
		}

		void DeleteSaveDate(string fileName)
		{
		}

		void HasInternetConnection(Action<bool> callback)
		{
			callback?.Invoke(obj: true);
		}

		void CanPlayMultiplayer(Action<bool> callback)
		{
			callback?.Invoke(obj: true);
		}

		void CanCommunicate(Action<bool> callback)
		{
			callback?.Invoke(obj: true);
		}

		void CanCommunicateWithUser(ulong targetUserID, Action<bool> callback)
		{
			callback?.Invoke(obj: true);
		}

		void GetAvatarAsync(Action<Texture2D> callback)
		{
			callback?.Invoke(Texture2D.redTexture);
		}

		void GetAvatarAsync(ulong xuid, Action<Texture2D> callback)
		{
			callback?.Invoke(Texture2D.redTexture);
		}
	}
}
