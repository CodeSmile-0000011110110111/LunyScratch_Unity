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
	internal sealed class ScratchRuntime : MonoBehaviour, IEngineRuntime, IScratchRunner
	{
		private static ScratchRuntime s_Instance;
		private static Boolean s_Initialized;
		private BlockRunner _runner;

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

		// IScratchRunner implementation
		public void Run(params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.Sequence(blocks));

		public void RunPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(Blocks.Sequence(blocks));

		public void RepeatForever(params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.RepeatForever(blocks));

		public void RepeatForeverPhysics(params IScratchBlock[] blocks) => _runner.AddPhysicsBlock(Blocks.RepeatForever(blocks));

		// public void RepeatWhileTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(new RepeatWhileTrueBlock(condition, blocks));
		//
		// public void RepeatUntilTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(new RepeatUntilTrueBlock(condition, blocks));

		// public void When(Func<Boolean> condition, params IScratchBlock[] blocks) =>
		// 	_runner.AddBlock(Blocks.When(condition, blocks));

		public void When(EventBlock evt, params IScratchBlock[] blocks) => _runner.AddBlock(Blocks.When(evt, blocks));

		private void Awake() => _runner = new BlockRunner(GlobalScratchContext.Null);

		private void Update() => _runner.ProcessUpdate(Time.deltaTime);

		private void FixedUpdate() => _runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);

		private void OnDestroy()
		{
			_runner.Dispose();
			s_Instance = null;
			s_Initialized = false;
		}
	}
}
