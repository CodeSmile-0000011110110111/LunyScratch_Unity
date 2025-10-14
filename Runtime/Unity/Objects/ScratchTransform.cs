using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class ScratchTransform : ITransform
	{
		private readonly Transform _transform;

		public IVector3 Position => new ScratchVector3(_transform.position);
		public IVector3 Forward => new ScratchVector3(_transform.forward);

		internal Transform Transform => _transform;

		public ScratchTransform(Transform transform) => _transform = transform;

		public void SetPosition(Single x, Single y, Single z) => _transform.position = new Vector3(x, y, z);
	}
}
