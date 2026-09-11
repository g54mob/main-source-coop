using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CurvedUI
{
	[ExecuteInEditMode]
	public class CurvedUITMPSubmesh : MonoBehaviour
	{
		private VertexHelper _vh;

		private Mesh _straightMesh;

		private Mesh _curvedMesh;

		private CurvedUIVertexEffect _crvdVe;

		private TMP_SubMeshUI _tmPsub;

		private TextMeshProUGUI _tmPtext;

		public void UpdateSubmesh(bool tesselate, bool curve)
		{
			if (_tmPsub == null)
			{
				_tmPsub = base.gameObject.GetComponent<TMP_SubMeshUI>();
			}
			if (_tmPsub == null)
			{
				return;
			}
			if (_tmPtext == null)
			{
				_tmPtext = GetComponentInParent<TextMeshProUGUI>();
			}
			if (_crvdVe == null)
			{
				_crvdVe = base.gameObject.AddComponentIfMissing<CurvedUIVertexEffect>();
			}
			if (!(_tmPsub.materialForRendering == null) || !(_tmPsub.fallbackMaterial == null))
			{
				if (tesselate || _straightMesh == null || _vh == null || !Application.isPlaying)
				{
					_vh = new VertexHelper(_tmPsub.mesh);
					_straightMesh = new Mesh();
					_vh.FillMesh(_straightMesh);
					curve = true;
				}
				if (curve)
				{
					_vh = new VertexHelper(_straightMesh);
					_crvdVe.ModifyMesh(_vh);
					_curvedMesh = new Mesh();
					_vh.FillMesh(_curvedMesh);
					_crvdVe.CurvingRequired = true;
					_crvdVe.TesselationRequired = true;
				}
				_tmPsub.canvasRenderer.SetMesh(_curvedMesh);
				if (_tmPtext != null && _tmPtext.textInfo.materialCount < 2)
				{
					_tmPsub.enabled = false;
					_tmPsub.enabled = true;
				}
			}
		}
	}
}
