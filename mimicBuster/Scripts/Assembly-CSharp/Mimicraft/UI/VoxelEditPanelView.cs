using Mimicraft.Gameplay;
using Mimicraft.Networking;
using Mimicraft.VoxelEditor;
using Unity.Netcode;
using UnityEngine;

namespace Mimicraft.UI
{
	public class VoxelEditPanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] panels;

		private PlayerVoxelBody localVoxelBody;

		private PlayerEditSession localEditSession;

		public static VoxelEditPanelView Create(Transform parent, GameObject[] panels)
		{
			GameObject obj = new GameObject("VoxelEditPanel");
			obj.transform.SetParent(parent, worldPositionStays: false);
			VoxelEditPanelView voxelEditPanelView = obj.AddComponent<VoxelEditPanelView>();
			voxelEditPanelView.panels = panels;
			return voxelEditPanelView;
		}

		public void SetPanels(GameObject[] panels)
		{
			this.panels = panels;
		}

		private void Update()
		{
			if (localEditSession == null && NetworkManager.Singleton != null && NetworkManager.Singleton.LocalClient != null && NetworkManager.Singleton.LocalClient.PlayerObject != null)
			{
				NetworkObject playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
				localVoxelBody = playerObject.GetComponent<PlayerVoxelBody>();
				localEditSession = playerObject.GetComponent<PlayerEditSession>();
			}
			bool flag = VoxelEditorSettings.MenuEditing || (localEditSession != null && localEditSession.IsEditing);
			if (localVoxelBody != null && !VoxelEditorSettings.MenuEditing)
			{
				localVoxelBody.EditorController.enabled = flag;
			}
			if (panels == null)
			{
				return;
			}
			GameObject[] array = panels;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null && gameObject.activeSelf != flag)
				{
					gameObject.SetActive(flag);
				}
			}
		}
	}
}
