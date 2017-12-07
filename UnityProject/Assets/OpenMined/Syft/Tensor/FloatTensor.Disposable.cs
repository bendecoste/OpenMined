using System;

namespace OpenMined.Syft.Tensor
{
	public partial class FloatTensor : IDisposable
	{
		// Do not decrement nCreated everytime we dispose an object to avoid id collisions.
		private static volatile int nDeleted = 0;
		private bool disposed = false;

		public static int DeletedObjectCount {
			get { return nDeleted; }
		}

		public void Dispose ()
		{ 
			Dispose (true);
			GC.SuppressFinalize (this);           
		}

		protected virtual void Dispose (bool disposing)
		{
			if (disposed)
				return; 

			if (disposing) {
				data = null;
				shape = null;
				strides = null;

				if (dataOnGpu) {
					EraseGpu ();
				}
			}

			disposed = true;
		}

		~FloatTensor ()
		{
			Dispose (false);
			System.Threading.Interlocked.Increment (ref nDeleted);
		}
	}
}
