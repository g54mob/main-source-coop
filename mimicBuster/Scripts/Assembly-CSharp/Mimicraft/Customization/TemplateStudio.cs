using Mimicraft.VoxelEditor;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.Customization
{
	public class TemplateStudio : PortraitStudio, IPortraitStudio<TemplateModel>
	{
		[Tooltip("Parçaların kopyalanacağı örnek model - sahneye elle koyduğun, üzerinde VoxelModel olan bir obje. Malzemesi ve voxel boyu buradan geliyor, yani şablonların neye benzeyeceğine bu obje karar veriyor.\n\nİçindeki voxeller önemsiz; her çekimde temizlenip şablonunkilerle dolduruluyor. Kendisi hiç görünmüyor, sadece kopyalanıyor.")]
		[SerializeField]
		private VoxelModel prototype;

		[Tooltip("Üretilen parçaların altına dizileceği obje. Boş bırakılırsa prototipin üst objesi kullanılır - yani prototipi sahnede nereye koyduysan model orada kurulur.")]
		[SerializeField]
		private Transform stageRoot;

		[Tooltip("Açıksa parçalar Unlit malzemeyle çizilir - stüdyodaki ışıklar görsele hiç karışmaz, her şablon aynı düz renklerle çıkar.\n\nKapalıyken (varsayılan) Lit kullanılır ve sahneyi aydınlatman anlam kazanır. Hangisini seçersen seç, prototipin VoxelModel'inde o malzemenin dolu olması gerekiyor.")]
		[SerializeField]
		private bool unlit;

		private Transform dressed;

		public static TemplateStudio Instance { get; private set; }

		protected override Component Stage => dressed;

		protected override string MissingStageMessage => "Sahnede kurulmuş bir model yok - Prototype alanı boş olabilir.";

		private void WarnAboutMaterials()
		{
			if (prototype == null)
			{
				Debug.LogWarning("[TemplateStudio] '" + base.name + "': Prototype bos - sablonlarin gorseli cekilemez. Sahneye VoxelModel tasiyan bir obje koy ve bu alana bagla.", this);
			}
			else if (!((unlit ? prototype.UnlitMaterial : prototype.LitMaterial) != null))
			{
				Debug.LogWarning("[TemplateStudio] '" + base.name + "': Prototype'in VoxelModel'inde " + (unlit ? "Unlit Material" : "Lit Material") + " bos - gorseller pembe cikar. Malzemeyi VoxelModel bilesenine ata (renderer'a degil; onu SetUnlit yaziyor).", this);
			}
		}

		protected override void Awake()
		{
			if (stageRoot == null && prototype != null)
			{
				stageRoot = prototype.transform.parent;
			}
			base.Awake();
			if (prototype != null)
			{
				prototype.gameObject.SetActive(value: false);
			}
			WarnAboutMaterials();
			Instance = this;
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
			Clear();
		}

		public void Dress(TemplateModel data)
		{
			Clear();
			if (data == null || prototype == null)
			{
				return;
			}
			Transform parent = ((stageRoot != null) ? stageRoot : base.transform);
			dressed = new GameObject("Sablon").transform;
			dressed.SetParent(parent, worldPositionStays: false);
			foreach (TemplatePiece piece in data.Pieces)
			{
				VoxelModel voxelModel = Object.Instantiate(prototype, dressed);
				voxelModel.gameObject.SetActive(value: true);
				voxelModel.name = (string.IsNullOrWhiteSpace(piece.Name) ? "Parca" : piece.Name);
				voxelModel.transform.localPosition = piece.LocalPosition;
				voxelModel.transform.localRotation = piece.LocalRotation;
				TemplateStorage.ApplyPiece(piece, voxelModel.Grid);
				voxelModel.RebuildMesh(updateCollider: false);
				voxelModel.SetUnlit(unlit);
			}
		}

		private void Clear()
		{
			if (!(dressed == null))
			{
				Object.Destroy(dressed.gameObject);
				dressed = null;
			}
		}
	}
}
