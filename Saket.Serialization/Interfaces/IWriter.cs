using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Saket.Serialization
{
    public interface IWriter
    {
        public int Position { get; set; }

        public void Write<T>(in T value) where T : unmanaged;
        public void Write<T>(in T[] value) where T : unmanaged 
        { 
            Write(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                Write(value[i]);
            }
        }

        public void WriteSerde<T>(in T value) where T : ISerde, new();
        public void WriteSerde<T>(in T[] value) where T : ISerde, new()
        {
            Write(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                WriteSerde(value[i]);
            }
        }



        // ---- String Serialization ----
        public void Write(string s);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Write(void* value, int length);

    }
}
