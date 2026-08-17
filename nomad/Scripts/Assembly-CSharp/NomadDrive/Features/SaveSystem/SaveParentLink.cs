namespace NomadDrive.Features.SaveSystem
{
	public struct SaveParentLink
	{
		public string ParentGuid;

		public SaveLinkKind LinkKind;

		public byte SubIndex;

		public static SaveParentLink None => new SaveParentLink
		{
			ParentGuid = string.Empty,
			LinkKind = SaveLinkKind.None,
			SubIndex = 0
		};

		public bool HasParent
		{
			get
			{
				if (LinkKind != SaveLinkKind.None)
				{
					return !string.IsNullOrEmpty(ParentGuid);
				}
				return false;
			}
		}
	}
}
