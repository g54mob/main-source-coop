using NomadDrive.Features.Attachables;

namespace NomadDrive.Features.Furnitures
{
	public class Seat : AttachableObject
	{
		private SittableSurface _sittable;

		protected override void Awake()
		{
			base.Awake();
			_sittable = GetComponentInChildren<SittableSurface>(includeInactive: true);
		}

		protected override bool CanDetach()
		{
			if (base.CanDetach())
			{
				if (!(_sittable == null))
				{
					return !_sittable.IsOccupied;
				}
				return true;
			}
			return false;
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
