using Fusion.Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Features.DebugModule.Scripts
{
	public class RegionVersionOverlay : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _regionText;

		private void Update()
		{
			string fixedRegion = PhotonAppSettings.Global.AppSettings.FixedRegion;
			_regionText.text = "Region: " + fixedRegion;
		}
	}
}
