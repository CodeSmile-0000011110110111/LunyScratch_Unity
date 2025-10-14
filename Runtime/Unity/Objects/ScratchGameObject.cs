using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class ScratchGameObject : IEngineObject
	{
		private readonly GameObject _gameObject;

		public static implicit operator ScratchGameObject(GameObject engineObject) => new(engineObject);

		public ScratchGameObject(GameObject gameObject) => _gameObject = gameObject;

		public void SetEnabled(Boolean enabled) => _gameObject.SetActive(enabled);

		// public void ScheduleDestroy(Double delayInSeconds = 0.0)
		// {
		// 	if (delayInSeconds > 0.0)
		// 		Object.Destroy(_gameObject, (Single)delayInSeconds);
		// 	else
		// 		Object.Destroy(_gameObject);
		// }
	}
}
