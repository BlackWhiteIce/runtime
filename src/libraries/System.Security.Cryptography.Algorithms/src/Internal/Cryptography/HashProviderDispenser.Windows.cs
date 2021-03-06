// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Internal.Cryptography
{
    //
    // Provides hash services via the native provider (CNG).
    //
    internal static partial class HashProviderDispenser
    {
        public static HashProvider CreateHashProvider(string hashAlgorithmId)
        {
            return new HashProviderCng(hashAlgorithmId, null);
        }

        public static HashProvider CreateMacProvider(string hashAlgorithmId, ReadOnlySpan<byte> key)
        {
            return new HashProviderCng(hashAlgorithmId, key, isHmac: true);
        }

        public static class OneShotHashProvider
        {
            public static unsafe int HashData(string hashAlgorithmId, ReadOnlySpan<byte> source, Span<byte> destination)
            {
                using (HashProviderCng hashProvider = new HashProviderCng(hashAlgorithmId, null))
                {
                    hashProvider.AppendHashData(source);
                    return hashProvider.FinalizeHashAndReset(destination);
                }
            }
        }
    }
}
