﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SR = iobloc.ConsoleTest.Properties.Resources;

namespace iobloc.Native.Windows
{
    /// <summary>
    /// Provides static methods for converting from Win32 errors codes to exceptions, HRESULTS and error messages.
    /// </summary>
    internal static class Win32Marshal
    {
        /// <summary>
        /// Converts, resetting it, the last Win32 error into a corresponding <see cref="Exception"/> object, optionally
        /// including the specified path in the error message.
        /// </summary>
        internal static System.Exception GetExceptionForLastWin32Error(string? path = "")
            => GetExceptionForWin32Error(Marshal.GetLastWin32Error(), path);

        /// <summary>
        /// Converts the specified Win32 error into a corresponding <see cref="Exception"/> object, optionally
        /// including the specified path in the error message.
        /// </summary>
        internal static System.Exception GetExceptionForWin32Error(int errorCode, string? path = "")
        {
            // ERROR_SUCCESS gets thrown when another unexpected interop call was made before checking GetLastWin32Error().
            // Errors have to get retrieved as soon as possible after P/Invoking to avoid this.
            Debug.Assert(errorCode != Interop.Errors.ERROR_SUCCESS);

            switch (errorCode)
            {
                case Interop.Errors.ERROR_FILE_NOT_FOUND:
                    return new FileNotFoundException(
                        string.IsNullOrEmpty(path) ? SR.IO_FileNotFound : string.Format(SR.IO_FileNotFound_FileName, path), path);
                case Interop.Errors.ERROR_PATH_NOT_FOUND:
                    return new DirectoryNotFoundException(
                        string.IsNullOrEmpty(path) ? SR.IO_PathNotFound_NoPathName : string.Format(SR.IO_PathNotFound_Path, path));
                case Interop.Errors.ERROR_ACCESS_DENIED:
                    return new System.UnauthorizedAccessException(
                        string.IsNullOrEmpty(path) ? SR.UnauthorizedAccess_IODenied_NoPathName : string.Format(SR.UnauthorizedAccess_IODenied_Path, path));
                case Interop.Errors.ERROR_ALREADY_EXISTS:
                    if (string.IsNullOrEmpty(path))
                        goto default;
                    return new IOException(string.Format(SR.IO_AlreadyExists_Name, path), MakeHRFromErrorCode(errorCode));
                case Interop.Errors.ERROR_FILENAME_EXCED_RANGE:
                    return new PathTooLongException(
                        string.IsNullOrEmpty(path) ? SR.IO_PathTooLong : string.Format(SR.IO_PathTooLong_Path, path));
                case Interop.Errors.ERROR_SHARING_VIOLATION:
                    return new IOException(
                        string.IsNullOrEmpty(path) ? SR.IO_SharingViolation_NoFileName : string.Format(SR.IO_SharingViolation_File, path),
                        MakeHRFromErrorCode(errorCode));
                case Interop.Errors.ERROR_FILE_EXISTS:
                    if (string.IsNullOrEmpty(path))
                        goto default;
                    return new IOException(string.Format(SR.IO_FileExists_Name, path), MakeHRFromErrorCode(errorCode));
                case Interop.Errors.ERROR_OPERATION_ABORTED:
                    return new System.OperationCanceledException();
                case Interop.Errors.ERROR_INVALID_PARAMETER:
                default:
                    return new IOException(
                        string.IsNullOrEmpty(path) ? GetMessage(errorCode) : $"{GetMessage(errorCode)} : '{path}'",
                        MakeHRFromErrorCode(errorCode));
            }
        }

        /// <summary>
        /// If not already an HRESULT, returns an HRESULT for the specified Win32 error code.
        /// </summary>
        internal static int MakeHRFromErrorCode(int errorCode)
        {
            // Don't convert it if it is already an HRESULT
            if ((0xFFFF0000 & errorCode) != 0)
                return errorCode;

            return unchecked(((int)0x80070000) | errorCode);
        }

        /// <summary>
        /// Returns a string message for the specified Win32 error code.
        /// </summary>
        internal static string GetMessage(int errorCode) => Interop.Kernel32.GetMessage(errorCode);
    }
}