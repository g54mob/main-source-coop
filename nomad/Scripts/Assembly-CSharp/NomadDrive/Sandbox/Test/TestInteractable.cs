using NomadDrive.Features.Interaction;

namespace NomadDrive.Sandbox.Test
{
	public class TestInteractable : Interactable
	{
		protected override void Awake()
		{
			base.Awake();
			CreateHoldInteraction(TestHoldInteractionAction, "Test", 1.5f);
		}

		private void TestHoldInteractionAction()
		{
			RemoveHoldInteraction();
			CreateBasicInteraction(TestBasicInteractionAction, "Test");
		}

		private void TestBasicInteractionAction()
		{
			RemoveBasicInteraction();
			CreateHoldInteraction(TestHoldInteractionAction, "Test", 1.5f);
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
