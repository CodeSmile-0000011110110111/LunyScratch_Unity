namespace LunyScratch
{
	internal sealed class ScratchRuntimeContext : ScratchBehaviourContext
	{
		public ScratchRuntimeContext(ScratchBehaviour owner)
			: base(owner) => _isRuntimeContext = true;
	}
}
