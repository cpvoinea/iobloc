// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SR = iobloc.ConsoleTest.Properties.Resources;

namespace iobloc.Native
{
    /* SyncTextReader intentionally locks on itself rather than a private lock object.
     * This is done to synchronize different console readers (https://github.com/dotnet/corefx/pull/2855).
     */
    internal sealed class SyncTextReader : TextReader
    {
        internal readonly TextReader _in;

        public static SyncTextReader GetSynchronizedTextReader(TextReader reader)
        {
            Debug.Assert(reader != null);
            return reader as SyncTextReader ??
                new SyncTextReader(reader);
        }

        internal SyncTextReader(TextReader t)
        {
            _in = t;
        }
    }
}
