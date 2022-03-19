using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LibMtpSharp.Lists
{
    internal abstract class UnmanagedList<T> : IEnumerable<T>, IDisposable
    {
        private readonly IntPtr _listPtr;

        public UnmanagedList(IntPtr listPtr)
        {
            _listPtr = listPtr;
        }

        protected abstract IntPtr GetPointerToNextItem(ref T item);
        protected abstract void FreeItem(IntPtr item);
        
        public IEnumerator<T> GetEnumerator()
        {
            var currentItem = _listPtr;
            while (currentItem != IntPtr.Zero)
            {
                var currentItemStruct = Marshal.PtrToStructure<T>(currentItem);
                yield return currentItemStruct;
                currentItem = GetPointerToNextItem(ref currentItemStruct);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ReleaseUnmanagedResources()
        {
            var currentItem = _listPtr;
            while (currentItem != IntPtr.Zero)
            {
                var currentItemStruct = Marshal.PtrToStructure<T>(currentItem);
                FreeItem(currentItem);
                currentItem = GetPointerToNextItem(ref currentItemStruct);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }
        
        ~UnmanagedList()
        {
            ReleaseUnmanagedResources();
        }
    }
}