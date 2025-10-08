using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class UnityGameObject : IEngineObject
	{
		private readonly GameObject _gameObject;

		public static implicit operator UnityGameObject(GameObject engineObject) => new(engineObject);

		public UnityGameObject(GameObject gameObject) => _gameObject = gameObject;

		public void SetEnabled(Boolean enabled) => _gameObject.SetActive(enabled);
	}
}
