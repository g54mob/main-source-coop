using UnityEngine;

namespace Mimicraft.Dev
{
	public static class DevCameraPathPlayer
	{
		private static float nextSampleTime;

		public static bool Recording { get; private set; }

		public static bool Playing { get; private set; }

		public static DevCameraPath Current { get; private set; }

		public static float Time { get; private set; }

		public static bool Loop { get; set; } = true;

		public static float Speed { get; set; } = 1f;

		public static void BeginRecording(string name, float tickRate)
		{
			Playing = false;
			Recording = true;
			Time = 0f;
			nextSampleTime = 0f;
			Current = new DevCameraPath
			{
				Name = DevRecording.SanitizeName(name),
				TickRate = Mathf.Clamp(tickRate, 15f, 120f)
			};
		}

		public static string EndRecording()
		{
			Recording = false;
			if (Current == null || Current.Keys.Count < 2)
			{
				return "";
			}
			string text = DevCameraPath.PathFor(Current.Name);
			Current.Save(text);
			return text;
		}

		public static bool BeginPlayback(DevCameraPath path)
		{
			if (path == null || path.Keys.Count < 2)
			{
				return false;
			}
			Recording = false;
			Current = path;
			Playing = true;
			Time = 0f;
			return true;
		}

		public static void Stop()
		{
			Recording = false;
			Playing = false;
		}

		public static bool Tick(Transform camera, Camera lens, float deltaTime)
		{
			if (Recording)
			{
				Sample(camera, lens, deltaTime);
				return false;
			}
			if (!Playing || Current == null)
			{
				return false;
			}
			Time += deltaTime * Mathf.Max(0.01f, Speed);
			if (Time > Current.Duration)
			{
				if (!Loop)
				{
					Time = Current.Duration;
					Playing = false;
				}
				else
				{
					Time = 0f;
				}
			}
			DevCameraPath.Key key = Current.Sample(Time);
			camera.SetPositionAndRotation(key.Position, key.Rotation);
			if (lens != null && key.FieldOfView > 1f)
			{
				lens.fieldOfView = key.FieldOfView;
			}
			return true;
		}

		private static void Sample(Transform camera, Camera lens, float deltaTime)
		{
			Time += deltaTime;
			if (!(Time < nextSampleTime))
			{
				nextSampleTime = Time + 1f / Current.TickRate;
				Current.Keys.Add(new DevCameraPath.Key
				{
					Position = camera.position,
					Rotation = camera.rotation,
					FieldOfView = ((lens != null) ? lens.fieldOfView : 60f)
				});
			}
		}
	}
}
