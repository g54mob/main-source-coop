using System.Collections.Generic;
using Obi;
using UnityEngine;

namespace Features.BridgeModule.Scripts
{
	public class BridgeSectionComponent : MonoBehaviour
	{
		public List<ObiRope> Ropes;

		public List<ObiParticleAttachment> StartAttachments;

		public List<ObiParticleAttachment> EndAttachments;
	}
}
