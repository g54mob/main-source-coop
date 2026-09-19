using System.Collections.Generic;
using System.Linq;
using Fusion;
using Obi;
using PlayerCustomization;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	[NetworkBehaviourWeaved(0)]
	public class StaticColorChangerByPlayerRef : NetworkBehaviour
	{
		private static readonly int ColorProperty = Shader.PropertyToID("_Color");

		private static readonly int UpperColorProperty = Shader.PropertyToID("_UpperColor");

		private static readonly int BottomColorProperty = Shader.PropertyToID("_BottomColor");

		private static readonly int ObiBaseColorProperty = Shader.PropertyToID("_BaseColor");

		[SerializeField]
		private List<Renderer> _renderers;

		[SerializeField]
		private List<Renderer> _universalRenderers;

		[SerializeField]
		private List<Renderer> _maskedRenderers;

		[SerializeField]
		private List<Renderer> _variableColorRenderers;

		[SerializeField]
		private List<ObiRopeExtrudedRenderer> _obiRopeExtrudedRenderers;

		protected PlayerCustomizationModel PlayerCustomizationModel;

		private float _flushBlend;

		private Color _flushTargetColor = Color.white;

		[Inject]
		public void InjectDependencies(PlayerCustomizationModel playerCustomizationModel)
		{
			PlayerCustomizationModel = playerCustomizationModel;
		}

		public override void Spawned()
		{
			base.Spawned();
			AdjustColorByCustomization();
			PlayerCustomizationModel.OnSlotsChanged += AdjustColorByCustomization;
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);
			PlayerCustomizationModel.OnSlotsChanged -= AdjustColorByCustomization;
			_flushBlend = 0f;
		}

		public void SetFlushBlend(Color flushColor, float blend01)
		{
			_flushTargetColor = flushColor;
			_flushBlend = Mathf.Clamp01(blend01);
			AdjustColorByCustomization();
		}

		public void ClearFlushBlend()
		{
			if (!(_flushBlend <= 0f))
			{
				_flushBlend = 0f;
				AdjustColorByCustomization();
			}
		}

		private void AssignColor(List<Renderer> playerRenderers, Color color)
		{
			foreach (Renderer playerRenderer in playerRenderers)
			{
				if (!(playerRenderer == null) && !(playerRenderer.material == null))
				{
					Material material = new Material(playerRenderer.material);
					material.color = color;
					playerRenderer.material = material;
				}
			}
		}

		private void AssignShaderColor(List<Renderer> playerRenderers, Color color, int shaderColorProperty)
		{
			foreach (Renderer playerRenderer in playerRenderers)
			{
				Material material = new Material(playerRenderer.material);
				material.SetColor(shaderColorProperty, color);
				playerRenderer.material = material;
			}
		}

		private void AssignUniversalShaderColor(List<Renderer> playerRenderers, Color upperColor, Color bottomColor)
		{
			foreach (Renderer playerRenderer in playerRenderers)
			{
				Material material = new Material(playerRenderer.material);
				material.SetColor(UpperColorProperty, upperColor);
				material.SetColor(BottomColorProperty, bottomColor);
				playerRenderer.material = material;
			}
		}

		private void AssignObiShaderColor(List<ObiRopeExtrudedRenderer> obiRenderers, Color color, int shaderColorProperty)
		{
			foreach (ObiRopeExtrudedRenderer obiRenderer in obiRenderers)
			{
				Material material = new Material(obiRenderer.material);
				material.SetColor(shaderColorProperty, color);
				obiRenderer.material = material;
				obiRenderer.OnValidate();
			}
		}

		protected void AdjustColorByCustomization()
		{
			int searchedPlayerId = base.Object.InputAuthority.PlayerId;
			PlayerCustomizationSlotData playerCustomizationSlotData = PlayerCustomizationModel.Slots.FirstOrDefault((PlayerCustomizationSlotData x) => x.PlayerId == searchedPlayerId);
			if (playerCustomizationSlotData != null)
			{
				Color color = MixWithFlush(playerCustomizationSlotData.PrimaryColor);
				Color color2 = MixWithFlush(playerCustomizationSlotData.VariableColor);
				AssignColor(_renderers, color);
				AssignShaderColor(_maskedRenderers, color, ColorProperty);
				AssignShaderColor(_variableColorRenderers, color2, ColorProperty);
				AssignUniversalShaderColor(_universalRenderers, color, color2);
				AssignObiShaderColor(_obiRopeExtrudedRenderers, color, ObiBaseColorProperty);
			}
		}

		private Color MixWithFlush(Color baseColor)
		{
			if (_flushBlend <= 0f)
			{
				return baseColor;
			}
			return Color.Lerp(baseColor, _flushTargetColor, _flushBlend);
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
		}
	}
}
