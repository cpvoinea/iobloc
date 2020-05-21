// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#nullable enable

namespace iobloc.Native
{

    public delegate void ConsoleCancelEventHandler(object? sender, ConsoleCancelEventArgs e);

    public sealed class ConsoleCancelEventArgs : System.EventArgs
    {
        internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
        {
            SpecialKey = type;
        }

        // Whether to cancel the break event.  By setting this to true, the
        // Control-C will not kill the process.
        public bool Cancel
        {
            get; set;
        }

        public ConsoleSpecialKey SpecialKey { get; }
    }
}
