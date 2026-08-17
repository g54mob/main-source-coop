using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EvilCore.UI.MainMenu.Panels
{
	public class SaveSlotEntryUI : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI worldNameText;

		[SerializeField]
		private TextMeshProUGUI lastSavedText;

		[SerializeField]
		private TextMeshProUGUI playtimeText;

		[SerializeField]
		private RawImage thumbnailImage;

		[SerializeField]
		private GameObject thumbnailPlaceholder;

		[SerializeField]
		private Button continueButton;

		[SerializeField]
		private Button deleteButton;

		private SaveSlotInfo _slot;

		private Action<SaveSlotInfo> _onContinue;

		private Action<SaveSlotInfo> _onDelete;

		private Texture2D _thumbnailTexture;

		public void Setup(SaveSlotInfo slot, string dateText, string playtimeText, Action<SaveSlotInfo> onContinue, Action<SaveSlotInfo> onDelete)
		{
			_slot = slot;
			_onContinue = onContinue;
			_onDelete = onDelete;
			if (worldNameText != null)
			{
				worldNameText.text = slot.DisplayName;
			}
			if (lastSavedText != null)
			{
				lastSavedText.text = dateText;
			}
			if (this.playtimeText != null)
			{
				this.playtimeText.text = playtimeText;
			}
			LoadThumbnail(slot.ThumbnailPath);
			continueButton?.onClick.AddListener(delegate
			{
				_onContinue?.Invoke(_slot);
			});
			deleteButton?.onClick.AddListener(delegate
			{
				_onDelete?.Invoke(_slot);
			});
		}

		private void LoadThumbnail(string path)
		{
			bool flag = !string.IsNullOrEmpty(path) && File.Exists(path);
			if (thumbnailImage != null)
			{
				thumbnailImage.gameObject.SetActive(flag);
			}
			if (thumbnailPlaceholder != null)
			{
				thumbnailPlaceholder.SetActive(!flag);
			}
			if (!flag || thumbnailImage == null)
			{
				return;
			}
			try
			{
				byte[] data = File.ReadAllBytes(path);
				_thumbnailTexture = new Texture2D(2, 2, TextureFormat.RGB24, mipChain: false);
				if (_thumbnailTexture.LoadImage(data))
				{
					thumbnailImage.texture = _thumbnailTexture;
				}
			}
			catch
			{
				thumbnailImage.gameObject.SetActive(value: false);
				if (thumbnailPlaceholder != null)
				{
					thumbnailPlaceholder.SetActive(value: true);
				}
			}
		}

		private void OnDestroy()
		{
			continueButton?.onClick.RemoveAllListeners();
			deleteButton?.onClick.RemoveAllListeners();
			if (_thumbnailTexture != null)
			{
				UnityEngine.Object.Destroy(_thumbnailTexture);
			}
		}
	}
}
