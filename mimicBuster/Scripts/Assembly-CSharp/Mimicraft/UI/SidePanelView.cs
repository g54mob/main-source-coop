using Mimicraft.VoxelEditor;
using UnityEngine;

namespace Mimicraft.UI
{
	public class SidePanelView : MonoBehaviour
	{
		[SerializeField]
		private ExtrudeStatusView extrudeStatus;

		[SerializeField]
		private PaintPanelView paintPanel;

		[SerializeField]
		private TransformPanelView transformPanel;

		public static SidePanelView Create(Transform parent)
		{
			RectTransform rectTransform = UIFactory.CreateRect(parent, "SidePanel");
			rectTransform.anchorMin = new Vector2(1f, 0.5f);
			rectTransform.anchorMax = new Vector2(1f, 0.5f);
			rectTransform.pivot = new Vector2(1f, 0.5f);
			rectTransform.anchoredPosition = new Vector2(-16f, 0f);
			ExtrudeStatusView extrudeStatusView = ExtrudeStatusView.Create(rectTransform);
			PaintPanelView paintPanelView = PaintPanelView.Create(rectTransform);
			TransformPanelView transformPanelView = TransformPanelView.Create(rectTransform);
			SidePanelView sidePanelView = rectTransform.gameObject.AddComponent<SidePanelView>();
			sidePanelView.extrudeStatus = extrudeStatusView;
			sidePanelView.paintPanel = paintPanelView;
			sidePanelView.transformPanel = transformPanelView;
			return sidePanelView;
		}

		public void EnsureLatestControls()
		{
			if (transformPanel != null)
			{
				transformPanel.EnsureModeButton();
			}
		}

		private void Start()
		{
			if (extrudeStatus != null)
			{
				extrudeStatus.ReportModeOptions();
			}
			if (paintPanel != null)
			{
				paintPanel.ReportModeOptions();
			}
			if (transformPanel != null)
			{
				transformPanel.ReportModeOptions();
			}
		}

		private void OnDestroy()
		{
			EditorModeOptions.ResetAll();
		}

		private void Update()
		{
			VoxelEditorController focused = VoxelFocusManager.GetFocused(VoxelEditorSettings.ActiveCamera);
			bool active = extrudeStatus.Bind(focused);
			extrudeStatus.gameObject.SetActive(active);
			bool flag = focused != null && focused.State == EditorState.Paint;
			if (flag)
			{
				paintPanel.Bind(focused);
			}
			paintPanel.gameObject.SetActive(flag);
			bool flag2 = focused != null && focused.State == EditorState.Transform;
			if (flag2)
			{
				transformPanel.Bind(focused);
			}
			transformPanel.gameObject.SetActive(flag2);
		}
	}
}
