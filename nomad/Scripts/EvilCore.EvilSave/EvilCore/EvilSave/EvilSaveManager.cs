using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.Managers;
using UnityEngine;

namespace EvilCore.EvilSave
{
	public class EvilSaveManager : MonoBehaviour, IEvilSaveManager, ISaveManager
	{
		private float _autoSaveTimer;

		private bool _autoSaveInProgress;

		private float _sessionStartTime;

		private readonly List<SaveableEntity> _registeredEntities = new List<SaveableEntity>();

		public Func<bool> AutoSaveGate { get; set; }

		public string ActiveSlot
		{
			get
			{
				return EvilSave.ActiveSlot;
			}
			set
			{
				EvilSave.ActiveSlot = value;
			}
		}

		public bool IsLoading { get; private set; }

		public bool IsSaving { get; private set; }

		public event Action OnBeforeSave;

		public event Action OnAfterSave;

		public event Action OnBeforeLoad;

		public event Action OnAfterLoad;

		private void Awake()
		{
			_sessionStartTime = Time.unscaledTime;
		}

		private void Update()
		{
			if (!EvilSaveSettings.Instance.EnableAutoSave)
			{
				return;
			}
			if (AutoSaveGate != null && !AutoSaveGate())
			{
				_autoSaveTimer = 0f;
			}
			else if (!_autoSaveInProgress)
			{
				_autoSaveTimer += Time.unscaledDeltaTime;
				if (!(_autoSaveTimer < EvilSaveSettings.Instance.AutoSaveIntervalSeconds))
				{
					_autoSaveTimer = 0f;
					AutoSaveAsync().Forget();
				}
			}
		}

		public void RegisterSaveable(SaveableEntity entity)
		{
			if (!_registeredEntities.Contains(entity))
			{
				_registeredEntities.Add(entity);
			}
		}

		public void UnregisterSaveable(SaveableEntity entity)
		{
			_registeredEntities.Remove(entity);
		}

		public void SaveGame()
		{
			this.OnBeforeSave?.Invoke();
			GatherAllSaveables();
			EvilSave.SaveToDisk();
			CreateBackupIfEnabled();
			UpdatePlaytime();
			this.OnAfterSave?.Invoke();
		}

		public void LoadGame()
		{
			IsLoading = true;
			this.OnBeforeLoad?.Invoke();
			EvilSave.LoadFromDisk();
			ApplyAllSaveables();
			IsLoading = false;
			this.OnAfterLoad?.Invoke();
		}

		public async UniTask SaveGameAsync()
		{
			IsSaving = true;
			this.OnBeforeSave?.Invoke();
			GatherAllSaveables();
			await EvilSave.SaveToDiskAsync();
			CreateBackupIfEnabled();
			UpdatePlaytime();
			IsSaving = false;
			this.OnAfterSave?.Invoke();
		}

		public async UniTask LoadGameAsync()
		{
			IsLoading = true;
			this.OnBeforeLoad?.Invoke();
			await EvilSave.LoadFromDiskAsync();
			ApplyAllSaveables();
			IsLoading = false;
			this.OnAfterLoad?.Invoke();
		}

		public void ApplyLoadedSaveables()
		{
			ApplyAllSaveables();
		}

		void ISaveManager.StartGame()
		{
			if (EvilSave.SlotExists(ActiveSlot))
			{
				LoadGame();
			}
			else
			{
				EvilSave.CreateSlot(ActiveSlot);
			}
		}

		void ISaveManager.SaveGame()
		{
			SaveGame();
		}

		private async UniTaskVoid AutoSaveAsync()
		{
			_autoSaveInProgress = true;
			try
			{
				this.OnBeforeSave?.Invoke();
				GatherAllSaveables();
				await EvilSave.SaveToDiskAsync();
				UpdatePlaytime();
				this.OnAfterSave?.Invoke();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[EvilSaveManager] Auto-save failed: " + ex.Message, "AutoSaveAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\EvilSave\\Scripts\\Core\\EvilSaveManager.cs", 154);
			}
			finally
			{
				_autoSaveInProgress = false;
			}
		}

		private void GatherAllSaveables()
		{
			for (int num = _registeredEntities.Count - 1; num >= 0; num--)
			{
				if (_registeredEntities[num] == null)
				{
					_registeredEntities.RemoveAt(num);
				}
				else
				{
					_registeredEntities[num].CaptureState();
				}
			}
		}

		private void ApplyAllSaveables()
		{
			for (int num = _registeredEntities.Count - 1; num >= 0; num--)
			{
				if (_registeredEntities[num] == null)
				{
					_registeredEntities.RemoveAt(num);
				}
				else
				{
					_registeredEntities[num].RestoreState();
				}
			}
		}

		private void CreateBackupIfEnabled()
		{
			if (EvilSaveSettings.Instance.EnableBackups)
			{
				EvilSave.CreateBackup();
			}
		}

		private void UpdatePlaytime()
		{
			SaveSlotMetadata slotMetadata = EvilSave.GetSlotMetadata(ActiveSlot);
			if (slotMetadata != null)
			{
				slotMetadata.playtimeSeconds += Time.unscaledTime - _sessionStartTime;
				_sessionStartTime = Time.unscaledTime;
				string metaPath = EvilSave.GetMetaPath(ActiveSlot);
				EvilSave.Storage.Write(metaPath, Encoding.UTF8.GetBytes(slotMetadata.ToJson()));
			}
		}
	}
}
