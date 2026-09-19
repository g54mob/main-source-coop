using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	[ScriptHelp(BackColor = ScriptHeaderBackColor.Orange)]
	public class FusionStatisticsRoot : FusionMonoBehaviour
	{
		public static List<FusionStatisticsRoot> Roots = new List<FusionStatisticsRoot>();

		[SerializeField]
		private MultipleOptionsPanel _multipleOptionsPrefab;

		[SerializeField]
		private Text _peerText;

		[SerializeField]
		private Button _collapseButton;

		[SerializeField]
		private Button _multiPeerButton;

		[SerializeField]
		private Button _anchorButton;

		[SerializeField]
		private RectTransform _sideBar;

		[SerializeField]
		private Dropdown _pagesDropdown;

		public RectTransform PagesContent;

		private FusionStatistics _statistics;

		private MultipleOptionsPanel _peerOptionsInstance;

		private bool _collapsed;

		private RectTransform _rectTransform;

		private Vector2 _originAnchoredPosition;

		private static FusionStatisticsConfig.Side _anchorSide = FusionStatisticsConfig.Side.Right;

		public static FusionStatisticsRoot ActiveRoot { get; private set; }

		public FusionStatistics Statistics => _statistics;

		public bool IsVisible => !_collapsed;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticFields()
		{
			_anchorSide = FusionStatisticsConfig.Side.Right;
			Roots = new List<FusionStatisticsRoot>();
			ActiveRoot = null;
		}

		private void Start()
		{
			EnsureCorrectAnchor();
		}

		public void SetupStatistics(FusionStatistics statistics)
		{
			_statistics = statistics;
			_peerText.text = _statistics.Runner.LocalPlayer.ToString();
			_collapsed = false;
			GetComponentInParent<CanvasScaler>();
			_rectTransform = (RectTransform)base.transform;
			_originAnchoredPosition = _rectTransform.anchoredPosition;
		}

		public void SetupPagesDropdown()
		{
			_pagesDropdown.ClearOptions();
			foreach (FusionStatisticsPage page in _statistics.Pages)
			{
				_pagesDropdown.options.Add(new Dropdown.OptionData
				{
					text = page.PageName
				});
			}
			_pagesDropdown.onValueChanged.AddListener(OnDropdownChanged);
			void OnDropdownChanged(int selected)
			{
				_statistics.ChangePage(selected);
			}
		}

		public void ToggleMultiPeerPanel()
		{
			if ((bool)_statistics && !_peerOptionsInstance && !_collapsed)
			{
				FusionStatisticsRoot[] componentsInChildren = base.transform.parent.GetComponentsInChildren<FusionStatisticsRoot>(includeInactive: true);
				MultipleOptionsPanel multipleOptionsPanel = Object.Instantiate(_multipleOptionsPrefab, base.transform.parent);
				multipleOptionsPanel.Setup("Select Peer", componentsInChildren, (FusionStatisticsRoot root) => root.Statistics?.Runner.LocalPlayer.ToString(), delegate(FusionStatisticsRoot root)
				{
					base.gameObject.SetActive(value: false);
					SetActiveRoot(root);
				});
				_peerOptionsInstance = multipleOptionsPanel;
			}
		}

		internal static void SetActiveRoot(FusionStatisticsRoot root)
		{
			root.gameObject.SetActive(value: true);
			ActiveRoot = root;
		}

		public void ToggleCollapse()
		{
			_collapsed = !_collapsed;
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 vector = ((_anchorSide == FusionStatisticsConfig.Side.Right) ? Vector2.right : Vector2.left);
			Vector2 target = (_collapsed ? (_originAnchoredPosition + vector * rectTransform.rect.width) : _originAnchoredPosition);
			StartCoroutine(MoveToPosition(target, 0.2f));
			int num = ((_anchorSide == FusionStatisticsConfig.Side.Right) ? (-90) : 90);
			_collapseButton.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, _collapsed ? num : (-num));
			_anchorButton.interactable = !_collapsed;
			_multiPeerButton.interactable = !_collapsed;
		}

		public void ToggleAnchorSide()
		{
			_anchorSide = ((_anchorSide == FusionStatisticsConfig.Side.Right) ? FusionStatisticsConfig.Side.Left : FusionStatisticsConfig.Side.Right);
			FusionStatisticsRoot[] componentsInChildren = base.transform.parent.GetComponentsInChildren<FusionStatisticsRoot>(includeInactive: true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].EnsureCorrectAnchor();
			}
		}

		internal void EnsureCorrectAnchor()
		{
			FusionStatisticsConfig.Side anchorSide = _anchorSide;
			Vector2 anchorMin = _sideBar.anchorMin;
			Vector2 anchorMax = _sideBar.anchorMax;
			int num = ((anchorSide != FusionStatisticsConfig.Side.Right) ? 1 : 0);
			anchorMin.x = num;
			anchorMax.x = num;
			_sideBar.anchorMin = anchorMin;
			_sideBar.anchorMax = anchorMax;
			Vector2 pivot = _sideBar.pivot;
			pivot.x = ((anchorSide == FusionStatisticsConfig.Side.Right) ? 1 : 0);
			_sideBar.pivot = pivot;
			_sideBar.anchoredPosition = Vector3.zero;
			Vector2 anchorMin2 = _rectTransform.anchorMin;
			Vector2 anchorMax2 = _rectTransform.anchorMax;
			anchorMin2.x = ((anchorSide == FusionStatisticsConfig.Side.Right) ? 0.75f : 0f);
			anchorMax2.x = ((anchorSide == FusionStatisticsConfig.Side.Right) ? 1f : 0.25f);
			_rectTransform.anchorMin = anchorMin2;
			_rectTransform.anchorMax = anchorMax2;
			Quaternion rotation = _collapseButton.transform.GetChild(0).rotation;
			rotation.z *= -1f;
			_collapseButton.transform.GetChild(0).rotation = rotation;
		}

		private void Update()
		{
			_multiPeerButton.gameObject.SetActive(Roots.Count > 1);
		}

		private IEnumerator MoveToPosition(Vector2 target, float duration)
		{
			float time = 0f;
			Vector2 startPosition = _rectTransform.anchoredPosition;
			while (time < duration)
			{
				_rectTransform.anchoredPosition = Vector2.Lerp(startPosition, target, time / duration);
				time += Time.deltaTime;
				yield return null;
			}
			_rectTransform.anchoredPosition = target;
		}
	}
}
