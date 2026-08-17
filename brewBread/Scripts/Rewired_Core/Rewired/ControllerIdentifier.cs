using System;

namespace Rewired
{
	public struct ControllerIdentifier
	{
		private int UceKlChkUHGjQaNwpAzeqaieFMpt;

		private ControllerType KHkewZTAeuvHVDDJaWvnQRRxxDiF;

		private Guid fMxZVPLmyEupjctdQIaGgbDJdlvHA;

		private string xBrcfixGFlRWLdALITGLqMbVvAXW;

		private Guid guGxXkQdSCnhDedSVaPzhraGphmHA;

		public int controllerId
		{
			get
			{
				return UceKlChkUHGjQaNwpAzeqaieFMpt;
			}
			set
			{
				UceKlChkUHGjQaNwpAzeqaieFMpt = value;
			}
		}

		public ControllerType controllerType
		{
			get
			{
				return KHkewZTAeuvHVDDJaWvnQRRxxDiF;
			}
			set
			{
				KHkewZTAeuvHVDDJaWvnQRRxxDiF = value;
			}
		}

		public Guid hardwareTypeGuid
		{
			get
			{
				return fMxZVPLmyEupjctdQIaGgbDJdlvHA;
			}
			set
			{
				fMxZVPLmyEupjctdQIaGgbDJdlvHA = value;
			}
		}

		public string hardwareIdentifier
		{
			get
			{
				return xBrcfixGFlRWLdALITGLqMbVvAXW;
			}
			set
			{
				xBrcfixGFlRWLdALITGLqMbVvAXW = value;
			}
		}

		public Guid deviceInstanceGuid
		{
			get
			{
				return guGxXkQdSCnhDedSVaPzhraGphmHA;
			}
			set
			{
				guGxXkQdSCnhDedSVaPzhraGphmHA = value;
			}
		}

		public static ControllerIdentifier Blank => new ControllerIdentifier
		{
			UceKlChkUHGjQaNwpAzeqaieFMpt = -1
		};

		internal ControllerIdentifier(Controller P_0)
		{
			UceKlChkUHGjQaNwpAzeqaieFMpt = P_0.id;
			KHkewZTAeuvHVDDJaWvnQRRxxDiF = P_0.type;
			fMxZVPLmyEupjctdQIaGgbDJdlvHA = P_0.fMxZVPLmyEupjctdQIaGgbDJdlvHA;
			xBrcfixGFlRWLdALITGLqMbVvAXW = P_0.hardwareIdentifier;
			guGxXkQdSCnhDedSVaPzhraGphmHA = P_0.deviceInstanceGuid;
		}
	}
}
