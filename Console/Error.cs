// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using SR = iobloc.ConsoleTest.Properties.Resources;

namespace iobloc.Native
{
    internal static class Error
    {
        internal static System.Exception GetFileNotOpen()
        {
            return new System.ObjectDisposedException(null, SR.ObjectDisposed_FileClosed);
        }

        internal static System.Exception GetReadNotSupported()
        {
            return new System.NotSupportedException(SR.NotSupported_UnreadableStream);
        }

        internal static System.Exception GetSeekNotSupported()
        {
            return new System.NotSupportedException(SR.NotSupported_UnseekableStream);
        }

        internal static System.Exception GetWriteNotSupported()
        {
            return new System.NotSupportedException(SR.NotSupported_UnwritableStream);
        }
    }
}
