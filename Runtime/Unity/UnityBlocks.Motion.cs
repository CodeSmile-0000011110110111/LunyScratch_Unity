using System;
using UnityEngine;

namespace LunyScratch
{
	public static partial class UnityBlocks
	{
		public static IScratchBlock MoveForward(Rigidbody rigidbody, Single speed) =>
			Blocks.MoveForward(new UnityRigidbody(rigidbody), speed);

		public static IScratchBlock MoveBackward(Rigidbody rigidbody, Single speed) =>
			Blocks.MoveBackward(new UnityRigidbody(rigidbody), speed);

		public static IScratchBlock StopMoving(Rigidbody rigidbody) =>
			Blocks.StopMoving(new UnityRigidbody(rigidbody));

		public static IScratchBlock SlowDownMoving(Rigidbody rigidbody, Single factor) =>
			Blocks.SlowDownMoving(new UnityRigidbody(rigidbody), factor);

		public static IScratchBlock TurnLeft(Rigidbody rigidbody, Single degreesPerSecond) =>
			Blocks.TurnLeft(new UnityRigidbody(rigidbody), degreesPerSecond);

		public static IScratchBlock TurnRight(Rigidbody rigidbody, Single degreesPerSecond) =>
			Blocks.TurnRight(new UnityRigidbody(rigidbody), degreesPerSecond);

		public static IScratchBlock StopTurning(Rigidbody rigidbody) =>
			Blocks.StopTurning(new UnityRigidbody(rigidbody));

		public static IScratchBlock SlowDownTurning(Rigidbody rigidbody, Single factor) =>
			Blocks.SlowDownTurning(new UnityRigidbody(rigidbody), factor);
	}
}
