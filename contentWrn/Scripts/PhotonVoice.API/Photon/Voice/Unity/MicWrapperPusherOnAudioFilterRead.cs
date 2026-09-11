using System;
using UnityEngine;

namespace Photon.Voice.Unity
{
	internal class MicWrapperPusherOnAudioFilterRead : MonoBehaviour
	{
		private float[] frame2 = new float[0];

		public event Action<float[], int> OnAudioFrame;

		private void OnAudioFilterRead(float[] frame, int channels)
		{
			if (this.OnAudioFrame != null)
			{
				if (frame2.Length != frame.Length)
				{
					frame2 = new float[frame.Length];
				}
				Array.Copy(frame, frame2, frame.Length);
				this.OnAudioFrame(frame2, channels);
			}
			Array.Clear(frame, 0, frame.Length);
		}
	}
}
