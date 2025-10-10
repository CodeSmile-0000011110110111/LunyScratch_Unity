using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class UnityTransform : ITransform
	{
		private readonly Transform _transform;

		public IVector3 Position => new UnityVector3(_transform.position);
		public IVector3 Forward => new UnityVector3(_transform.forward);

		internal Transform Transform => _transform;

		public UnityTransform(Transform transform) => _transform = transform;

		public void SetPosition(Single x, Single y, Single z) => _transform.position = new Vector3(x, y, z);
	}
}
