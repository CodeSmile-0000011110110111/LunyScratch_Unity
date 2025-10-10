using System;
using UnityEngine;

namespace LunyScratch
{
	internal struct UnityVector3 : IVector3
	{
		private Vector3 _value;

		public Single X { get => _value.x; set => _value.x = value; }
		public Single Y { get => _value.y; set => _value.y = value; }
		public Single Z { get => _value.z; set => _value.z = value; }

		public UnityVector3(Vector3 value) => _value = value;

		public Vector3 ToUnity() => _value;

		public static implicit operator UnityVector3(Vector3 v) => new(v);
		public static implicit operator Vector3(UnityVector3 v) => v._value;
	}
}
