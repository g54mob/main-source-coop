using System.Collections.Generic;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.Extensions;
using EvilCore.Localization;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Inputs;
using PrimeTween;
using TMPro;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Objectives
{
	public class ObjectivesPanel : GameCanvasGroup
	{
		[Header("List")]
		[SerializeField]
		private RectTransform itemsContainer;

		[SerializeField]
		private ObjectiveItemUI itemPrefab;

		[SerializeField]
		private CanvasGroup itemsContainerGroup;

		[Header("Exit Animation")]
		[SerializeField]
		private float exitFadeDuration = 0.4f;

		[Header("Paging")]
		[SerializeField]
		[Min(1f)]
		private int itemsPerPage = 2;

		[SerializeField]
		private TMP_Text pageIndicatorText;

		[SerializeField]
		private RectTransform pageIndicatorRoot;

		[Header("Page Transition")]
		[SerializeField]
		private float pageTransitionDuration = 0.22f;

		[SerializeField]
		private float pageSlideOffset = 60f;

		[SerializeField]
		private Ease pageTransitionEase = Ease.OutCubic;

		[SerializeField]
		private float pageIndicatorPunchScale = 1.2f;

		[SerializeField]
		private float pageIndicatorPunchDuration = 0.25f;

		[SerializeField]
		private Ease pageIndicatorPunchEase = Ease.OutBack;

		[Header("Lock")]
		[SerializeField]
		private GameObject lockIcon;

		[SerializeField]
		[Min(0f)]
		private float lockRefreshDelay = 0.3f;

		[Header("Input Prompts")]
		[SerializeField]
		private GameObject previousPagePromptRoot;

		[SerializeField]
		private GameObject nextPagePromptRoot;

		[SerializeField]
		private TMP_Text lockPromptLabel;

		[Header("Audio")]
		[SerializeField]
		private SoundID pageChangeSound;

		[SerializeField]
		private SoundID lockToggleSound;

		[SerializeField]
		private SoundID lockNavigationBlockedSound;

		[Inject]
		private IAudioManager _audioManager;

		[Inject]
		private ILocalizationService _localizationService;

		private readonly Dictionary<string, ObjectiveItemUI> _items = new Dictionary<string, ObjectiveItemUI>();

		private readonly List<string> _orderedIds = new List<string>();

		private readonly HashSet<string> _lockedObjectiveIds = new HashSet<string>();

		private bool _userToggledOff;

		private bool _isLocked;

		private int _currentPage;

		private Vector2 _restingPosition;

		private Vector3 _pageIndicatorRestingScale = Vector3.one;

		private Sequence _pageSequence;

		private Sequence _indicatorPunchSequence;

		private int TotalPages => Mathf.Max(1, Mathf.CeilToInt((float)_items.Count / (float)itemsPerPage));

		public bool IsHidden => _userToggledOff;

		public bool IsLocked => _isLocked;

		public bool IsObjectiveAudible(string objectiveId)
		{
			if (string.IsNullOrEmpty(objectiveId))
			{
				return true;
			}
			if (!_isLocked)
			{
				return true;
			}
			return _lockedObjectiveIds.Contains(objectiveId);
		}

		private void Awake()
		{
			Initialize();
			if (itemsContainer != null)
			{
				_restingPosition = itemsContainer.anchoredPosition;
				if (itemsContainerGroup == null)
				{
					itemsContainerGroup = itemsContainer.GetOrAddComponent<CanvasGroup>();
				}
			}
			if (pageIndicatorRoot != null)
			{
				_pageIndicatorRestingScale = pageIndicatorRoot.localScale;
			}
			UpdateLockIcon();
			UpdateInputPrompts();
		}

		private void OnEnable()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged += UpdateInputPrompts;
			}
		}

		private void OnDisable()
		{
			if (_localizationService != null)
			{
				_localizationService.OnLocaleChanged -= UpdateInputPrompts;
			}
		}

		private void Update()
		{
			HandleToggleInput();
			HandleLockInput();
			HandlePageInput();
		}

		private void HandleToggleInput()
		{
			if (BaseInputs.IsToggleObjectivesPanelButtonDown() && _items.Count != 0)
			{
				if (_userToggledOff)
				{
					_userToggledOff = false;
					base.Show(interactable: true, blockRaycast: true);
				}
				else
				{
					_userToggledOff = true;
					Hide();
				}
			}
		}

		private void HandleLockInput()
		{
			if (BaseInputs.IsObjectivesPanelLockButtonDown() && _items.Count != 0 && !_userToggledOff)
			{
				if (_isLocked)
				{
					_isLocked = false;
					_lockedObjectiveIds.Clear();
				}
				else
				{
					CaptureLockSnapshot();
					_isLocked = true;
				}
				UpdateLockIcon();
				UpdateInputPrompts();
				_audioManager?.PlayOneShotUI(lockToggleSound);
			}
		}

		private void CaptureLockSnapshot()
		{
			_lockedObjectiveIds.Clear();
			int num = _currentPage * itemsPerPage;
			int num2 = Mathf.Min(num + itemsPerPage, _orderedIds.Count);
			for (int i = num; i < num2; i++)
			{
				_lockedObjectiveIds.Add(_orderedIds[i]);
			}
		}

		private void HandlePageInput()
		{
			if (_items.Count == 0 || _userToggledOff || _pageSequence.isAlive)
			{
				return;
			}
			bool flag = BaseInputs.IsObjectivesPanelPreviousPageButtonDown();
			bool flag2 = BaseInputs.IsObjectivesPanelNextPageButtonDown();
			if (flag || flag2)
			{
				if (_isLocked)
				{
					_audioManager?.PlayOneShotUI(lockNavigationBlockedSound);
				}
				else if (flag)
				{
					GoToPage(_currentPage - 1, animated: true);
				}
				else
				{
					GoToPage(_currentPage + 1, animated: true);
				}
			}
		}

		public override void Show(bool interactable, bool blockRaycast)
		{
			if (_items.Count != 0)
			{
				base.Show(interactable, blockRaycast);
				ApplyPageVisibility(_currentPage);
				UpdatePageIndicator();
			}
		}

		public void AddObjective(ObjectiveDefinition definition, ObjectiveTrackerState state)
		{
			if (definition == null || itemsContainer == null || itemPrefab == null || string.IsNullOrEmpty(definition.ObjectiveId) || _items.ContainsKey(definition.ObjectiveId))
			{
				return;
			}
			ObjectiveItemUI objectiveItemUI = Object.Instantiate(itemPrefab, itemsContainer);
			objectiveItemUI.gameObject.InjectGameObject();
			_items[definition.ObjectiveId] = objectiveItemUI;
			if (_isLocked)
			{
				_orderedIds.Add(definition.ObjectiveId);
				objectiveItemUI.transform.SetAsLastSibling();
				objectiveItemUI.Build(definition, state);
				ApplyPageVisibility(_currentPage);
				UpdatePageIndicator();
				return;
			}
			objectiveItemUI.transform.SetAsFirstSibling();
			_orderedIds.Insert(0, definition.ObjectiveId);
			objectiveItemUI.Build(definition, state);
			_userToggledOff = false;
			Show(interactable: true, blockRaycast: true);
			bool flag = _currentPage == 0;
			bool flag2 = _items.Count > 1 && !flag;
			GoToPage(0, flag2);
			if (!flag2)
			{
				ApplyPageVisibility(_currentPage);
				UpdatePageIndicator();
			}
		}

		public void MarkStepComplete(string objectiveId, string stepId)
		{
			if (_items.TryGetValue(objectiveId, out var value))
			{
				value.MarkStepComplete(stepId);
				PromoteObjective(objectiveId);
			}
		}

		public void UpdateStepProgress(string objectiveId, string stepId, ObjectiveStepProgress progress, ObjectiveStepDefinition step)
		{
			if (_items.TryGetValue(objectiveId, out var value))
			{
				value.UpdateStepProgress(stepId, progress, step);
				PromoteObjective(objectiveId);
			}
		}

		private void PromoteObjective(string objectiveId)
		{
			if (_isLocked || !_items.TryGetValue(objectiveId, out var value) || value == null || _orderedIds.Count == 0)
			{
				return;
			}
			int num = _orderedIds.IndexOf(objectiveId);
			if (num > 0)
			{
				_orderedIds.RemoveAt(num);
				_orderedIds.Insert(0, objectiveId);
				value.transform.SetAsFirstSibling();
				if (_userToggledOff)
				{
					_currentPage = 0;
				}
				else if (_currentPage != 0)
				{
					GoToPage(0, animated: true);
				}
				else
				{
					ApplyPageVisibility(_currentPage);
				}
			}
		}

		public void RemoveObjective(string objectiveId)
		{
			if (!_items.TryGetValue(objectiveId, out var item))
			{
				return;
			}
			_items.Remove(objectiveId);
			_orderedIds.Remove(objectiveId);
			bool wasLocked = _isLocked && _lockedObjectiveIds.Remove(objectiveId);
			if (item == null)
			{
				HandlePostRemovalLayout();
				if (wasLocked)
				{
					ScheduleLockRefresh();
				}
				return;
			}
			if (exitFadeDuration <= 0f)
			{
				Object.Destroy(item.gameObject);
				HandlePostRemovalLayout();
				if (wasLocked)
				{
					ScheduleLockRefresh();
				}
				return;
			}
			item.PlayCloseAnimation(delegate
			{
				if (item != null)
				{
					Object.Destroy(item.gameObject);
				}
				HandlePostRemovalLayout();
				if (wasLocked)
				{
					ScheduleLockRefresh();
				}
			});
		}

		private void ScheduleLockRefresh()
		{
			if (_items.Count != 0)
			{
				_isLocked = false;
				_lockedObjectiveIds.Clear();
				UpdateLockIcon();
				UpdateInputPrompts();
				_audioManager?.PlayOneShotUI(lockToggleSound);
				Tween.Delay(this, lockRefreshDelay, delegate(ObjectivesPanel target)
				{
					target.ReapplyLockSnapshot();
				});
			}
		}

		private void ReapplyLockSnapshot()
		{
			if (_items.Count != 0)
			{
				CaptureLockSnapshot();
				_isLocked = true;
				UpdateLockIcon();
				UpdateInputPrompts();
				_audioManager?.PlayOneShotUI(lockToggleSound);
			}
		}

		public void ClearAll()
		{
			foreach (ObjectiveItemUI value in _items.Values)
			{
				if (value != null)
				{
					Object.Destroy(value.gameObject);
				}
			}
			_items.Clear();
			_orderedIds.Clear();
			_lockedObjectiveIds.Clear();
			_currentPage = 0;
			_userToggledOff = false;
			_isLocked = false;
			if (_pageSequence.isAlive)
			{
				_pageSequence.Stop();
			}
			if (_indicatorPunchSequence.isAlive)
			{
				_indicatorPunchSequence.Stop();
			}
			UpdateLockIcon();
			UpdatePageIndicator();
			Hide();
		}

		private void HandlePostRemovalLayout()
		{
			if (_items.Count == 0)
			{
				HideIfEmpty();
				return;
			}
			int num = TotalPages - 1;
			if (_currentPage > num)
			{
				GoToPage(num, animated: true);
				return;
			}
			ApplyPageVisibility(_currentPage);
			UpdatePageIndicator();
		}

		private void HideIfEmpty()
		{
			if (_items.Count == 0)
			{
				_userToggledOff = false;
				_currentPage = 0;
				UpdatePageIndicator();
				Hide();
			}
		}

		private void GoToPage(int targetPage, bool animated)
		{
			int num = Mathf.Clamp(targetPage, 0, TotalPages - 1);
			if (num == _currentPage)
			{
				ApplyPageVisibility(_currentPage);
				UpdatePageIndicator();
				return;
			}
			int num2 = ((num > _currentPage) ? 1 : (-1));
			_currentPage = num;
			if (_pageSequence.isAlive)
			{
				_pageSequence.Complete();
			}
			if (!animated || itemsContainer == null || itemsContainerGroup == null)
			{
				if (itemsContainer != null)
				{
					itemsContainer.anchoredPosition = _restingPosition;
				}
				if (itemsContainerGroup != null)
				{
					itemsContainerGroup.alpha = 1f;
				}
				ApplyPageVisibility(_currentPage);
				UpdatePageIndicator();
				PlayPageChangeSound();
				PunchPageIndicator();
				return;
			}
			float duration = pageTransitionDuration * 0.45f;
			float duration2 = pageTransitionDuration * 0.55f;
			Vector2 endValue = _restingPosition + new Vector2((float)(-num2) * pageSlideOffset, 0f);
			Vector2 inStart = _restingPosition + new Vector2((float)num2 * pageSlideOffset, 0f);
			PlayPageChangeSound();
			_pageSequence = Sequence.Create().Chain(Tween.UIAnchoredPosition(itemsContainer, endValue, duration, pageTransitionEase)).Group(Tween.Alpha(itemsContainerGroup, 0f, duration, pageTransitionEase))
				.ChainCallback(delegate
				{
					ApplyPageVisibility(_currentPage);
					if (itemsContainer != null)
					{
						itemsContainer.anchoredPosition = inStart;
					}
					UpdatePageIndicator();
					PunchPageIndicator();
				})
				.Chain(Tween.UIAnchoredPosition(itemsContainer, _restingPosition, duration2, pageTransitionEase))
				.Group(Tween.Alpha(itemsContainerGroup, 1f, duration2, pageTransitionEase));
		}

		private void ApplyPageVisibility(int page)
		{
			if (_orderedIds.Count == 0)
			{
				return;
			}
			int num = page * itemsPerPage;
			int num2 = num + itemsPerPage;
			for (int i = 0; i < _orderedIds.Count; i++)
			{
				if (_items.TryGetValue(_orderedIds[i], out var value) && !(value == null))
				{
					bool flag = i >= num && i < num2;
					if (value.gameObject.activeSelf != flag)
					{
						value.gameObject.SetActive(flag);
					}
				}
			}
		}

		private void UpdatePageIndicator()
		{
			if (pageIndicatorText != null)
			{
				if (_items.Count == 0)
				{
					pageIndicatorText.text = string.Empty;
				}
				else
				{
					pageIndicatorText.text = $"{_currentPage + 1}/{TotalPages}";
				}
			}
			UpdateInputPrompts();
		}

		private void UpdateInputPrompts()
		{
			int totalPages = TotalPages;
			if (previousPagePromptRoot != null)
			{
				bool flag = _items.Count > 0 && _currentPage > 0;
				if (previousPagePromptRoot.activeSelf != flag)
				{
					previousPagePromptRoot.SetActive(flag);
				}
			}
			if (nextPagePromptRoot != null)
			{
				bool flag2 = _items.Count > 0 && _currentPage < totalPages - 1;
				if (nextPagePromptRoot.activeSelf != flag2)
				{
					nextPagePromptRoot.SetActive(flag2);
				}
			}
			if (lockPromptLabel != null)
			{
				string key = (_isLocked ? "@interaction.unlock" : "@interaction.lock");
				string text = (_isLocked ? "Unlock" : "Lock");
				lockPromptLabel.text = ((_localizationService != null) ? _localizationService.Localize(key) : text);
			}
		}

		private void PunchPageIndicator()
		{
			if (!(pageIndicatorRoot == null))
			{
				if (_indicatorPunchSequence.isAlive)
				{
					_indicatorPunchSequence.Complete();
				}
				pageIndicatorRoot.localScale = _pageIndicatorRestingScale;
				float duration = pageIndicatorPunchDuration * 0.5f;
				Vector3 endValue = _pageIndicatorRestingScale * pageIndicatorPunchScale;
				_indicatorPunchSequence = Sequence.Create().Chain(Tween.Scale(pageIndicatorRoot, endValue, duration, pageIndicatorPunchEase)).Chain(Tween.Scale(pageIndicatorRoot, _pageIndicatorRestingScale, duration, Ease.OutCubic));
			}
		}

		private void PlayPageChangeSound()
		{
			if (!_userToggledOff)
			{
				_audioManager?.PlayOneShotUI(pageChangeSound);
			}
		}

		private void UpdateLockIcon()
		{
			if (lockIcon != null)
			{
				lockIcon.SetActive(_isLocked);
			}
		}
	}
}
