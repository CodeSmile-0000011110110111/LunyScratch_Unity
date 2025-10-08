using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Global singleton runtime for executing Scratch blocks without GameObject context.
	/// Automatically created and managed by Unity.
	/// </summary>
	[DefaultExecutionOrder(Int16.MinValue)]
	[AddComponentMenu("GameObject/")] // Do not list in "Add Component" menu
	[DisallowMultipleComponent]
	internal sealed class ScratchRuntime : ScratchBehaviour, IEngineRuntime
	{
		private static ScratchRuntime s_Instance;
		private static Boolean s_Initialized;

		public static ScratchRuntime Instance => s_Instance;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			if (s_Initialized)
				return;

			s_Initialized = true;

			var go = new GameObject(nameof(ScratchRuntime));
			s_Instance = go.AddComponent<ScratchRuntime>();
			DontDestroyOnLoad(go);

			GameEngine.Initialize(s_Instance, new UnityActions());
		}

		protected override void OnBehaviourDestroy()
		{
			s_Instance = null;
			s_Initialized = false;
		}
	}
}
