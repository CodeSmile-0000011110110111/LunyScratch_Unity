using System;
using UnityEngine;

namespace LunyScratch
{
	internal sealed class ScratchRigidbody : IRigidbody
	{
		private readonly Rigidbody _rigidbody;

		public IVector3 LinearVelocity => new ScratchVector3(_rigidbody.linearVelocity);
		public IVector3 AngularVelocity => new ScratchVector3(_rigidbody.angularVelocity);
		public IVector3 Position => new ScratchVector3(_rigidbody.position);
		public IVector3 Forward => new ScratchVector3(_rigidbody.transform.forward);

		public ScratchRigidbody(Rigidbody rigidbody) => _rigidbody = rigidbody;

		public void SetLinearVelocity(Single x, Single y, Single z) => _rigidbody.linearVelocity = new Vector3(x, y, z);

		public void SetAngularVelocity(Single x, Single y, Single z) => _rigidbody.angularVelocity = new Vector3(x, y, z);

		public void SetPosition(Single x, Single y, Single z) => _rigidbody.MovePosition(new Vector3(x, y, z));
	}
}
