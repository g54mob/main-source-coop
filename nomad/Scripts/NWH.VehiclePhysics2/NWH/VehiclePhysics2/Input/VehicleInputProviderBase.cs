using NWH.Common.Input;

namespace NWH.VehiclePhysics2.Input
{
	public class VehicleInputProviderBase : InputProvider
	{
		public virtual float Throttle()
		{
			return 0f;
		}

		public virtual float Brakes()
		{
			return 0f;
		}

		public virtual float Steering()
		{
			return 0f;
		}

		public virtual float Handbrake()
		{
			return 0f;
		}

		public virtual float Clutch()
		{
			return 0f;
		}

		public virtual bool EngineStartStop()
		{
			return false;
		}

		public virtual bool ExtraLights()
		{
			return false;
		}

		public virtual bool HighBeamLights()
		{
			return false;
		}

		public virtual bool HazardLights()
		{
			return false;
		}

		public virtual bool Horn()
		{
			return false;
		}

		public virtual bool LeftBlinker()
		{
			return false;
		}

		public virtual bool LowBeamLights()
		{
			return false;
		}

		public virtual bool RightBlinker()
		{
			return false;
		}

		public virtual bool ShiftDown()
		{
			return false;
		}

		public virtual int ShiftInto()
		{
			return -999;
		}

		public virtual bool ShiftUp()
		{
			return false;
		}

		public virtual bool TrailerAttachDetach()
		{
			return false;
		}

		public virtual bool FlipOver()
		{
			return false;
		}

		public virtual bool Boost()
		{
			return false;
		}

		public virtual bool CruiseControl()
		{
			return false;
		}
	}
}
