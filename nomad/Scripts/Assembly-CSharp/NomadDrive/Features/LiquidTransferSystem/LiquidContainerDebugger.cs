using UnityEngine;

namespace NomadDrive.Features.LiquidTransferSystem
{
	public class LiquidContainerDebugger : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private LiquidContainerComponent container;

		public LiquidType debugLiquidType;

		public float debugAmount = 50f;

		public float transferAmount = 10f;

		private LiquidContainerState CurrentState
		{
			get
			{
				if (!(container != null))
				{
					return LiquidContainerState.Empty;
				}
				return container.State;
			}
		}

		private LiquidType DisplayLiquidType
		{
			get
			{
				if (!(container != null))
				{
					return LiquidType.Empty;
				}
				return container.CurrentLiquidType;
			}
		}

		private float FillPercentage
		{
			get
			{
				if (!(container != null))
				{
					return 0f;
				}
				return container.FillRatio * 100f;
			}
		}

		private string AmountDisplay
		{
			get
			{
				if (!(container != null))
				{
					return "N/A";
				}
				return $"{container.CurrentAmount:F1} / {container.Capacity:F1}";
			}
		}

		private float MaxCapacity
		{
			get
			{
				if (!(container != null))
				{
					return 100f;
				}
				return container.Capacity;
			}
		}

		private void ApplyLiquidType()
		{
			if (ValidateContainer())
			{
				container.SetLiquidType(debugLiquidType);
			}
		}

		private void ApplyAmount()
		{
			if (ValidateContainer())
			{
				container.SetAmount(debugAmount);
			}
		}

		private void ApplyBoth()
		{
			if (ValidateContainer())
			{
				container.SetLiquidType(debugLiquidType);
				container.SetAmount(debugAmount);
			}
		}

		private void Fill25()
		{
			SetFillPercentage(0.25f);
		}

		private void Fill50()
		{
			SetFillPercentage(0.5f);
		}

		private void Fill75()
		{
			SetFillPercentage(0.75f);
		}

		private void Fill100()
		{
			SetFillPercentage(1f);
		}

		private void EmptyContainer()
		{
			if (ValidateContainer())
			{
				container.SetAmount(0f);
				container.SetLiquidType(LiquidType.Empty);
			}
		}

		private void FillMax()
		{
			if (ValidateContainer())
			{
				container.SetAmount(container.Capacity);
			}
		}

		private void SetFillPercentage(float percentage)
		{
			if (ValidateContainer())
			{
				container.SetAmount(container.Capacity * percentage);
			}
		}

		private void SimulateDrain()
		{
			if (ValidateContainer())
			{
				container.Drain(transferAmount);
			}
		}

		private void SimulateFill()
		{
			if (ValidateContainer())
			{
				container.Fill(transferAmount);
			}
		}

		private void LogFullState()
		{
			_ = container == null;
		}

		private void RefreshReference()
		{
			container = GetComponent<LiquidContainerComponent>();
		}

		private Color GetStateColor()
		{
			if (container == null)
			{
				return Color.gray;
			}
			return container.State switch
			{
				LiquidContainerState.Empty => new Color(1f, 0.5f, 0.5f), 
				LiquidContainerState.Full => new Color(0.5f, 1f, 0.5f), 
				LiquidContainerState.HasLiquid => new Color(0.5f, 0.8f, 1f), 
				_ => Color.white, 
			};
		}

		private Color GetFillBarColor()
		{
			if (container == null)
			{
				return Color.gray;
			}
			float fillRatio = container.FillRatio;
			if (fillRatio < 0.25f)
			{
				return new Color(1f, 0.4f, 0.4f);
			}
			if (fillRatio < 0.5f)
			{
				return new Color(1f, 0.7f, 0.4f);
			}
			if (fillRatio < 0.75f)
			{
				return new Color(0.7f, 0.9f, 0.5f);
			}
			return new Color(0.4f, 0.9f, 0.4f);
		}

		private Color GetFillBarBackgroundColor()
		{
			return new Color(0.2f, 0.2f, 0.2f);
		}

		private bool ValidateContainer()
		{
			if (container == null)
			{
				return false;
			}
			return true;
		}

		private void OnValidate()
		{
			if (container == null)
			{
				container = GetComponent<LiquidContainerComponent>();
			}
		}

		private void Reset()
		{
			container = GetComponent<LiquidContainerComponent>();
		}
	}
}
