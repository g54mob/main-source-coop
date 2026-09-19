using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenuModule.Scripts.Views.Chapters
{
	public class ChapterOptionPresenter : PresenterBehaviour<ChapterOptionViewBase>
	{
		private int _chapterIndex;

		private ISelectableWithNavigationCallbacks _selectableCallbacks;

		public Selectable Selectable => base.View.ClickButton;

		public event Action<int> OnClicked;

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

		public void Setup(int chapterIndex, int levelsCount)
		{
			_chapterIndex = chapterIndex;
			base.View.SetChapterNumber(chapterIndex + 1);
			base.View.SetLevelsCount(levelsCount);
		}

		public void SetParent(Transform parent)
		{
			base.View.transform.SetParent(parent, worldPositionStays: false);
		}

		public void DestroyView()
		{
			UnityEngine.Object.Destroy(base.View.gameObject);
		}

		public void SetChapterData(ChapterPreviewData chapterPreviewData)
		{
			base.View.SetChapterData(chapterPreviewData);
		}

		public void SetProgress(int levelsCount, int reachedSubLevelCount, bool isChapterPassed)
		{
			base.View.SetProgress(levelsCount, reachedSubLevelCount, isChapterPassed);
		}

		public void SetSelected(bool isSelected)
		{
			base.View.SetSelected(isSelected);
		}

		public void SetLocked(bool isLocked)
		{
			base.View.SetLocked(isLocked);
		}

		private void OnButtonClicked()
		{
			this.OnClicked?.Invoke(_chapterIndex);
		}

		private void OnSelected(ISelectableWithNavigationCallbacks _)
		{
			ExecuteEvents.Execute(base.View.ClickButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}
}
