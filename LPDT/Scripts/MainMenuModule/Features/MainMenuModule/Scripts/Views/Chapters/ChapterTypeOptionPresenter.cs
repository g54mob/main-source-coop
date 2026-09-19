using System;
using Features.LevelModule.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class ChapterTypeOptionPresenter : PresenterBehaviour<ChapterTypeOptionViewBase>
	{
		private ISelectableWithNavigationCallbacks _selectableCallbacks;

		private ChapterType _chapterType;

		public ChapterType ChapterType => _chapterType;

		public Selectable Selectable => base.View.ClickButton;

		public event Action<ChapterType> OnClicked;

		protected override void OnViewSet()
		{
			base.View.ClickButton.onClick.AddListener(OnButtonClicked);
			if (base.View.ClickButton is ISelectableWithNavigationCallbacks selectableCallbacks)
			{
				_selectableCallbacks = selectableCallbacks;
				_selectableCallbacks.OnSelectEvent += OnSelected;
			}
		}

		protected override void OnDisposed()
		{
			base.View.ClickButton.onClick.RemoveListener(OnButtonClicked);
			if (_selectableCallbacks != null)
			{
				_selectableCallbacks.OnSelectEvent -= OnSelected;
			}
		}

		public void Setup(ChapterType chapterType, LocalizationKey nameLocalizationKey)
		{
			_chapterType = chapterType;
			base.View.SetNameLocalizationKey(nameLocalizationKey);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent, worldPositionStays: false);
		}

		public void DestroyView()
		{
			UnityEngine.Object.Destroy(base.View.gameObject);
		}

		public void SetSelected(bool isSelected)
		{
			base.View.SetSelected(isSelected);
		}

		private void OnButtonClicked()
		{
			this.OnClicked?.Invoke(_chapterType);
		}

		private void OnSelected(ISelectableWithNavigationCallbacks _)
		{
			ExecuteEvents.Execute(base.View.ClickButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}
}
