using UnityEngine;

namespace LunyScratch
{
	public static partial class UnityBlocks
	{
		public static IScratchBlock Enable(Object obj) => Blocks.Enable(new UnityEngineObject(obj));
		public static IScratchBlock Disable(Object obj) => Blocks.Disable(new UnityEngineObject(obj));
	}
}
