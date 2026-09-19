using UnityEngine;

namespace Obi
{
	public class ObiEditorSettings : ScriptableObject
	{
		public const string m_ObiEditorSettingsPath = "Assets/ObiEditorSettings.asset";

		[SerializeField]
		private Color m_ParticleBrush;

		[SerializeField]
		private Color m_BrushWireframe;

		[SerializeField]
		private Color m_Particle;

		[SerializeField]
		private Color m_SelectedParticle;

		[SerializeField]
		private Color m_ActiveParticle;

		[SerializeField]
		private Gradient m_PropertyGradient;

		[SerializeField]
		private bool m_ParticlePicking;

		public Color brushColor => m_ParticleBrush;

		public Color brushWireframeColor => m_BrushWireframe;

		public Color particleColor => m_Particle;

		public Color selectedParticleColor => m_SelectedParticle;

		public Color activeParticleColor => m_ActiveParticle;

		public Gradient propertyGradient => m_PropertyGradient;

		public bool sceneViewParticlePicking => m_ParticlePicking;
	}
}
