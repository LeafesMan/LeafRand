using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LeafRand.Collections
{
    /// <summary>
    /// This class is just a work around to get Nonallocated access to a lists backing array.<br></br>
    /// When Unity finishes the process of switching from mono to CoreClr we can switch to CollectionsMarshal.AsSpan() and drop this hack-job class
    /// </summary>
    public static class ListAccessor<T>
    {
        // Fields order+types match List<T> private layout
        private class ListMirror
        {
            public T[] _items;
            public int _size;
            public int _version;
        }

        public static Span<T> AsSpan(List<T> list)
        {
            var mirror = Unsafe.As<ListMirror>(list);
            return mirror._items.AsSpan(0, mirror._size);
        }
    }
}