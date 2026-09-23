using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mimicraft.Gameplay
{
	[DisallowMultipleComponent]
	public class MapModeObject : MonoBehaviour
	{
		[Tooltip("Bu obje hangi modlarda AÇIK olsun. Birden fazlası seçilebilir. Hiçbiri seçili değilse obje hiçbir modda görünmez - bir şeyi geçici olarak devre dışı bırakmanın yolu bu. Everything = her modda açık, yani bu bileşen hiçbir şey yapmaz.")]
		[SerializeField]
		private MapMarkerModes modes = (MapMarkerModes)(-1);

		[Tooltip("İşaretliyse obje seçili modlarda KAPALI olur - kural tersine döner. 'Gartic hariç her yerde olsun' demek, üç modu tek tek seçmekten kısa ve bir mod eklendiğinde kendiliğinden doğru kalır.")]
		[SerializeField]
		private bool invert;

		public bool AppliesTo(MapMarkerModes mode)
		{
			bool flag = (modes & mode) != 0;
			if (!invert)
			{
				return flag;
			}
			return !flag;
		}

		public static int ApplyAll(GameObject root, MapMarkerModes mode)
		{
			if (root == null)
			{
				return 0;
			}
			int num = 0;
			MapModeObject[] componentsInChildren = root.GetComponentsInChildren<MapModeObject>(includeInactive: true);
			foreach (MapModeObject mapModeObject in componentsInChildren)
			{
				bool flag = mapModeObject.AppliesTo(mode);
				if (mapModeObject.gameObject.activeSelf != flag)
				{
					mapModeObject.gameObject.SetActive(flag);
					num++;
				}
			}
			return num;
		}

		public static int ApplyAll(Scene scene, MapMarkerModes mode)
		{
			if (!scene.IsValid() || !scene.isLoaded)
			{
				return 0;
			}
			int num = 0;
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			foreach (GameObject root in rootGameObjects)
			{
				num += ApplyAll(root, mode);
			}
			return num;
		}
	}
}
