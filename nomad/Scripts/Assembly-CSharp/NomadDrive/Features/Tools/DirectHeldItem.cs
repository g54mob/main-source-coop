using NomadDrive.Features.Inputs;
using NomadDrive.Features.Interaction;

namespace NomadDrive.Features.Tools
{
	public class DirectHeldItem : HeldItem
	{
		public HeldItemUseInputType useInputType;

		public virtual string UseActionPromptId => null;

		public virtual void OnUseButton()
		{
		}

		public virtual void OnUseButtonDown()
		{
		}

		public virtual void OnUseButtonUp()
		{
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
