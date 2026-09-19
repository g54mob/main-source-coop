using UnityHFSM.Inspection;

namespace UnityHFSM
{
	public interface IVisitableState
	{
		void AcceptVisitor(IStateVisitor visitor);
	}
}
