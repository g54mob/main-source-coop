using UnityEngine;
using UnityEngine.Serialization;

namespace FMODUnityResonance
{
	[AddComponentMenu("ResonanceAudio/FmodResonanceAudioRoom")]
	public class FmodResonanceAudioRoom : MonoBehaviour
	{
		public enum SurfaceMaterial
		{
			Transparent = 0,
			AcousticCeilingTiles = 1,
			BrickBare = 2,
			BrickPainted = 3,
			ConcreteBlockCoarse = 4,
			ConcreteBlockPainted = 5,
			CurtainHeavy = 6,
			FiberglassInsulation = 7,
			GlassThin = 8,
			GlassThick = 9,
			Grass = 10,
			LinoleumOnConcrete = 11,
			Marble = 12,
			Metal = 13,
			ParquetOnConcrete = 14,
			PlasterRough = 15,
			PlasterSmooth = 16,
			PlywoodPanel = 17,
			PolishedConcreteOrTile = 18,
			Sheetrock = 19,
			WaterOrIceSurface = 20,
			WoodCeiling = 21,
			WoodPanel = 22
		}

		[FormerlySerializedAs("leftWall")]
		public SurfaceMaterial LeftWall = SurfaceMaterial.ConcreteBlockCoarse;

		[FormerlySerializedAs("rightWall")]
		public SurfaceMaterial RightWall = SurfaceMaterial.ConcreteBlockCoarse;

		[FormerlySerializedAs("floor")]
		public SurfaceMaterial Floor = SurfaceMaterial.ParquetOnConcrete;

		[FormerlySerializedAs("ceiling")]
		public SurfaceMaterial Ceiling = SurfaceMaterial.PlasterRough;

		[FormerlySerializedAs("backWall")]
		public SurfaceMaterial BackWall = SurfaceMaterial.ConcreteBlockCoarse;

		[FormerlySerializedAs("frontWall")]
		public SurfaceMaterial FrontWall = SurfaceMaterial.ConcreteBlockCoarse;

		[FormerlySerializedAs("reflectivity")]
		public float Reflectivity = 1f;

		[FormerlySerializedAs("reverbGainDb")]
		public float ReverbGainDb;

		[FormerlySerializedAs("reverbBrightness")]
		public float ReverbBrightness;

		[FormerlySerializedAs("reverbTime")]
		public float ReverbTime = 1f;

		[FormerlySerializedAs("size")]
		public Vector3 Size = Vector3.one;

		private void OnEnable()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
		}

		private void OnDisable()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, roomEnabled: false);
		}

		private void Update()
		{
			FmodResonanceAudio.UpdateAudioRoom(this, FmodResonanceAudio.IsListenerInsideRoom(this));
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Size);
		}
	}
}
