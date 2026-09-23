using System;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	[Serializable]
	public class EditorGizmoSizes
	{
		[Tooltip("Genel gizmo büyüklüğü. VoxelSize'dan BAĞIMSIZ - 1 yazmak voxel boyu ne olursa olsun aynı büyüklüğü verir.")]
		[Min(0.01f)]
		public float gizmoSize = 1f;

		[Tooltip("Extrude okunun uzunluk çarpanı. Ayrı tutuluyor çünkü o okun boyu modelin kutusuna değil çekilen adım sayısına bağlı.")]
		[Min(0.01f)]
		public float extrudeGizmoSize = 1f;

		[Tooltip("Ok gövdesi/başı kalınlığı, uzunluktan bağımsız.")]
		[Min(0.01f)]
		public float gizmoThickness = 1f;

		[Tooltip("Merkezdeki sarı işaretçi küresinin büyüklüğü. Diğer gizmo'lardan ayrı: dünya çapı bir Modelci modeline göre yazılmış, küçük bir modelde çok daha küçük olması gerekiyor.")]
		[Min(0.01f)]
		public float centerMarkerSize = 1f;

		public void ApplyTo(VoxelEditorController controller)
		{
			if (!(controller == null))
			{
				controller.GizmoSize = gizmoSize;
				controller.ExtrudeGizmoSize = extrudeGizmoSize;
				controller.GizmoThickness = gizmoThickness;
				controller.CenterMarkerSize = centerMarkerSize;
			}
		}
	}
}
