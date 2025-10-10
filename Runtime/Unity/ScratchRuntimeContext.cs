using UnityEngine;

namespace LunyScratch
{
	internal sealed class UnityRuntimeContext : ScratchBehaviourContext
	{
		public UnityRuntimeContext(MonoBehaviour owner)
			: base(owner)
		{
			_isRuntimeContext = true;
		}
	}
}
