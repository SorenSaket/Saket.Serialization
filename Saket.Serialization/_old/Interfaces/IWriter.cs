using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    public interface IWriter
    {
        public void Write<T>(in T value) where T : unmanaged;
        public void Write<T>(in T[] value) where T : unmanaged;
        public void Write<T>(in ArraySegment<T> value) where T : unmanaged;

        
        // ---- Serializable Serialization ----
        public void WriteSerializable<T>(in T value) where T : ISerializable;
        public void WriteSerializable<T>(in T[] value) where T : ISerializable;


        // ---- String Serialization ----
        public void Write(string s, bool oneByteChars = false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Write(void* value, int length);

    }
}
