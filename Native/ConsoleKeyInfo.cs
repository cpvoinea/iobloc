// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using SR = iobloc.Properties.Resources;

namespace iobloc.Native
{
    public readonly struct ConsoleKeyInfo
    {
        public ConsoleKeyInfo(char keyChar, ConsoleKey key, bool shift, bool alt, bool control)
        {
            // Limit ConsoleKey values to 0 to 255, but don't check whether the
            // key is a valid value in our ConsoleKey enum.  There are a few
            // values in that enum that we didn't define, and reserved keys
            // that might start showing up on keyboards in a few years.
            if (((int)key) < 0 || ((int)key) > 255)
            {
                throw new System.ArgumentOutOfRangeException(nameof(key), SR.ArgumentOutOfRange_ConsoleKey);
            }

            KeyChar = keyChar;
            Key = key;
            Modifiers = 0;
            if (shift)
                Modifiers |= ConsoleModifiers.Shift;
            if (alt)
                Modifiers |= ConsoleModifiers.Alt;
            if (control)
                Modifiers |= ConsoleModifiers.Control;
        }

        public char KeyChar { get; }

        public ConsoleKey Key { get; }

        public ConsoleModifiers Modifiers { get; }

        public override bool Equals(object? value)
        {
            return value is ConsoleKeyInfo info && Equals(info);
        }

        public bool Equals(ConsoleKeyInfo obj)
        {
            return obj.KeyChar == KeyChar && obj.Key == Key && obj.Modifiers == Modifiers;
        }

        public static bool operator ==(ConsoleKeyInfo a, ConsoleKeyInfo b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ConsoleKeyInfo a, ConsoleKeyInfo b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            // For all normal cases we can fit all bits losslessly into the hash code:
            // _keyChar could be any 16-bit value (though is most commonly ASCII). Use all 16 bits without conflict.
            // _key is 32-bit, but the ctor throws for anything over 255. Use those 8 bits without conflict.
            // _mods only has enum defined values for 1,2,4: 3 bits. Use the remaining 8 bits.
            return KeyChar | ((int)Key << 16) | ((int)Modifiers << 24);
        }
    }
}
