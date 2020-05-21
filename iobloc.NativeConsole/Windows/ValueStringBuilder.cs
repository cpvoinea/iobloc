// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace iobloc.NativeConsole.Windows
{
    internal ref struct ValueStringBuilder
    {
        private char[]? _arrayToReturnToPool;
        private int _pos;

        public ValueStringBuilder(System.Span<char> initialBuffer)
        {
            _arrayToReturnToPool = null;
            RawChars = initialBuffer;
            _pos = 0;
        }

        public int Length
        {
            get => _pos;
            set
            {
                Debug.Assert(value >= 0);
                Debug.Assert(value <= RawChars.Length);
                _pos = value;
            }
        }

        public int Capacity => RawChars.Length;

        public void EnsureCapacity(int capacity)
        {
            if (capacity > RawChars.Length)
                Grow(capacity - _pos);
        }

        /// <summary>
        /// Get a pinnable reference to the builder.
        /// Does not ensure there is a null char after <see cref="Length"/>
        /// This overload is pattern matched in the C# 7.3+ compiler so you can omit
        /// the explicit method call, and write eg "fixed (char* c = builder)"
        /// </summary>
        public ref char GetPinnableReference()
        {
            return ref MemoryMarshal.GetReference(RawChars);
        }

        public override string ToString()
        {
            string s = RawChars.Slice(0, _pos).ToString();
            Dispose();
            return s;
        }

        /// <summary>Returns the underlying storage of the builder.</summary>
        public System.Span<char> RawChars { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            int pos = _pos;
            if ((uint)pos < (uint)RawChars.Length)
            {
                RawChars[pos] = c;
                _pos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        /// <summary>
        /// Resize the internal buffer either by doubling current buffer size or
        /// by adding <paramref name="additionalCapacityBeyondPos"/> to
        /// <see cref="_pos"/> whichever is greater.
        /// </summary>
        /// <param name="additionalCapacityBeyondPos">
        /// Number of chars requested beyond current position.
        /// </param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            Debug.Assert(additionalCapacityBeyondPos > 0);
            Debug.Assert(_pos > RawChars.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

            char[] poolArray = ArrayPool<char>.Shared.Rent(System.Math.Max(_pos + additionalCapacityBeyondPos, RawChars.Length * 2));

            RawChars.Slice(0, _pos).CopyTo(poolArray);

            char[]? toReturn = _arrayToReturnToPool;
            RawChars = _arrayToReturnToPool = poolArray;
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            char[]? toReturn = _arrayToReturnToPool;
            this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }
    }
}