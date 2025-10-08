using System;
using UnityEngine;

namespace LunyScratch
{
	/// <summary>
	/// Base class for all Scratch-style behaviors.
	/// Provides local block execution context for the GameObject it's attached to.
	/// </summary>
	public abstract class ScratchBehaviour : MonoBehaviour, IScratchRunner
	{
		private readonly BlockRunner _runner = new();

		private void Update() => _runner.ProcessUpdate(Time.deltaTime);

		private void FixedUpdate() => _runner.ProcessPhysicsUpdate(Time.fixedDeltaTime);

		private void OnDestroy()
		{
			_runner.Dispose();
			OnBehaviourDestroy();
		}

		/// <summary>
		/// Override this instead of OnDestroy to handle cleanup in derived classes.
		/// </summary>
		protected virtual void OnBehaviourDestroy() {}

		// IScratchRunner implementation
		public void Run(params IScratchBlock[] blocks) => _runner.Run(blocks);

		public void RunPhysics(params IScratchBlock[] blocks) => _runner.RunPhysics(blocks);

		public void RepeatForever(params IScratchBlock[] blocks) => _runner.RepeatForever(blocks);

		public void RepeatForeverPhysics(params IScratchBlock[] blocks) => _runner.RepeatForeverPhysics(blocks);

		public void RepeatWhileTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
			_runner.RepeatWhileTrue(condition, blocks);

		public void RepeatUntilTrue(Func<Boolean> condition, params IScratchBlock[] blocks) =>
			_runner.RepeatUntilTrue(condition, blocks);
	}
}
