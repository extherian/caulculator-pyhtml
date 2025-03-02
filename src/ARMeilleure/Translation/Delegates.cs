using ARMeilleure.Instructions;
using ARMeilleure.State;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
// ReSharper disable RedundantTypeArgumentsOfMethod

namespace ARMeilleure.Translation
{
    static class Delegates
    {
        public static bool TryGetDelegateFuncPtrByIndex(int index, out nint funcPtr)
        {
            if (index >= 0 && index < _delegates.Count)
            {
                funcPtr = _delegates.Values[index].FuncPtr; // O(1).

                return true;
            }
            else
            {
                funcPtr = default;

                return false;
            }
        }

        public static nint GetDelegateFuncPtrByIndex(int index)
        {
            if (index < 0 || index >= _delegates.Count)
            {
                throw new ArgumentOutOfRangeException($"({nameof(index)} = {index})");
            }

            return _delegates.Values[index].FuncPtr; // O(1).
        }

        public static nint GetDelegateFuncPtr(MethodInfo info)
        {
            ArgumentNullException.ThrowIfNull(info);

            string key = GetKey(info);

            if (!_delegates.TryGetValue(key, out DelegateInfo dlgInfo)) // O(log(n)).
            {
                throw new KeyNotFoundException($"({nameof(key)} = {key})");
            }

            return dlgInfo.FuncPtr;
        }

        public static int GetDelegateIndex(MethodInfo info)
        {
            ArgumentNullException.ThrowIfNull(info);

            string key = GetKey(info);

            int index = _delegates.IndexOfKey(key); // O(log(n)).

            if (index == -1)
            {
                throw new KeyNotFoundException($"({nameof(key)} = {key})");
            }

            return index;
        }

        private static void SetDelegateInfo(Delegate dlg, nint funcPtr)
        {
            string key = GetKey(dlg.Method);

            _delegates.Add(key, new DelegateInfo(dlg, funcPtr)); // ArgumentException (key).
        }

        private static string GetKey(MethodInfo info)
        {
            return $"{info.DeclaringType.Name}.{info.Name}";
        }

        private static readonly SortedList<string, DelegateInfo> _delegates;

        static Delegates()
        {
            _delegates = new SortedList<string, DelegateInfo>();

            // ReSharper disable InconsistentNaming
            // ReSharper disable RedundantDelegateCreation
            MathAbs dlgMathAbs = new(Math.Abs);
            MathCeiling dlgMathCeiling = new(Math.Ceiling);
            MathFloor dlgMathFloor = new(Math.Floor);
            MathRound dlgMathRound = new(Math.Round);
            MathTruncate dlgMathTruncate = new(Math.Truncate);

            MathFAbs dlgMathFAbs = new(MathF.Abs);
            MathFCeiling dlgMathFCeiling = new(MathF.Ceiling);
            MathFFloor dlgMathFFloor = new(MathF.Floor);
            MathFRound dlgMathFRound = new(MathF.Round);
            MathFTruncate dlgMathFTruncate = new(MathF.Truncate);

            NativeInterfaceBreak dlgNativeInterfaceBreak = new(NativeInterface.Break);
            NativeInterfaceCheckSynchronization dlgNativeInterfaceCheckSynchronization = new(NativeInterface.CheckSynchronization);
            NativeInterfaceEnqueueForRejit dlgNativeInterfaceEnqueueForRejit = new(NativeInterface.EnqueueForRejit);
            NativeInterfaceGetCntfrqEl0 dlgNativeInterfaceGetCntfrqEl0 = new(NativeInterface.GetCntfrqEl0);
            NativeInterfaceGetCntpctEl0 dlgNativeInterfaceGetCntpctEl0 = new(NativeInterface.GetCntpctEl0);
            NativeInterfaceGetCntvctEl0 dlgNativeInterfaceGetCntvctEl0 = new(NativeInterface.GetCntvctEl0);
            NativeInterfaceGetCtrEl0 dlgNativeInterfaceGetCtrEl0 = new(NativeInterface.GetCtrEl0);
            NativeInterfaceGetDczidEl0 dlgNativeInterfaceGetDczidEl0 = new(NativeInterface.GetDczidEl0);
            NativeInterfaceGetFunctionAddress dlgNativeInterfaceGetFunctionAddress = new(NativeInterface.GetFunctionAddress);
            NativeInterfaceInvalidateCacheLine dlgNativeInterfaceInvalidateCacheLine = new(NativeInterface.InvalidateCacheLine);
            NativeInterfaceReadByte dlgNativeInterfaceReadByte = new(NativeInterface.ReadByte);
            NativeInterfaceReadUInt16 dlgNativeInterfaceReadUInt16 = new(NativeInterface.ReadUInt16);
            NativeInterfaceReadUInt32 dlgNativeInterfaceReadUInt32 = new(NativeInterface.ReadUInt32);
            NativeInterfaceReadUInt64 dlgNativeInterfaceReadUInt64 = new(NativeInterface.ReadUInt64);
            NativeInterfaceReadVector128 dlgNativeInterfaceReadVector128 = new(NativeInterface.ReadVector128);
            NativeInterfaceSignalMemoryTracking dlgNativeInterfaceSignalMemoryTracking = new(NativeInterface.SignalMemoryTracking);
            NativeInterfaceSupervisorCall dlgNativeInterfaceSupervisorCall = new(NativeInterface.SupervisorCall);
            NativeInterfaceThrowInvalidMemoryAccess dlgNativeInterfaceThrowInvalidMemoryAccess = new(NativeInterface.ThrowInvalidMemoryAccess);
            NativeInterfaceUndefined dlgNativeInterfaceUndefined = new(NativeInterface.Undefined);
            NativeInterfaceWriteByte dlgNativeInterfaceWriteByte = new(NativeInterface.WriteByte);
            NativeInterfaceWriteUInt16 dlgNativeInterfaceWriteUInt16 = new(NativeInterface.WriteUInt16);
            NativeInterfaceWriteUInt32 dlgNativeInterfaceWriteUInt32 = new(NativeInterface.WriteUInt32);
            NativeInterfaceWriteUInt64 dlgNativeInterfaceWriteUInt64 = new(NativeInterface.WriteUInt64);
            NativeInterfaceWriteVector128 dlgNativeInterfaceWriteVector128 = new(NativeInterface.WriteVector128);

            SoftFallbackCountLeadingSigns dlgSoftFallbackCountLeadingSigns = new(SoftFallback.CountLeadingSigns);
            SoftFallbackCountLeadingZeros dlgSoftFallbackCountLeadingZeros = new(SoftFallback.CountLeadingZeros);
            SoftFallbackCrc32b dlgSoftFallbackCrc32b = new(SoftFallback.Crc32b);
            SoftFallbackCrc32cb dlgSoftFallbackCrc32cb = new(SoftFallback.Crc32cb);
            SoftFallbackCrc32ch dlgSoftFallbackCrc32ch = new(SoftFallback.Crc32ch);
            SoftFallbackCrc32cw dlgSoftFallbackCrc32cw = new(SoftFallback.Crc32cw);
            SoftFallbackCrc32cx dlgSoftFallbackCrc32cx = new(SoftFallback.Crc32cx);
            
            SoftFallbackCrc32h dlgSoftFallbackCrc32h = new(SoftFallback.Crc32h);
            SoftFallbackCrc32w dlgSoftFallbackCrc32w = new(SoftFallback.Crc32w);
            SoftFallbackCrc32x dlgSoftFallbackCrc32x = new(SoftFallback.Crc32x);
            SoftFallbackDecrypt dlgSoftFallbackDecrypt = new(SoftFallback.Decrypt);
            SoftFallbackEncrypt dlgSoftFallbackEncrypt = new(SoftFallback.Encrypt);
            SoftFallbackFixedRotate dlgSoftFallbackFixedRotate = new(SoftFallback.FixedRotate);
            SoftFallbackHashChoose dlgSoftFallbackHashChoose = new(SoftFallback.HashChoose);
            SoftFallbackHashLower dlgSoftFallbackHashLower = new(SoftFallback.HashLower);
            SoftFallbackHashMajority dlgSoftFallbackHashMajority = new(SoftFallback.HashMajority);
            SoftFallbackHashParity dlgSoftFallbackHashParity = new(SoftFallback.HashParity);
            SoftFallbackHashUpper dlgSoftFallbackHashUpper = new(SoftFallback.HashUpper);
            SoftFallbackInverseMixColumns dlgSoftFallbackInverseMixColumns = new(SoftFallback.InverseMixColumns);
            SoftFallbackMixColumns dlgSoftFallbackMixColumns = new(SoftFallback.MixColumns);
            SoftFallbackPolynomialMult64_128 dlgSoftFallbackPolynomialMult64_128 = new(SoftFallback.PolynomialMult64_128);
            SoftFallbackSatF32ToS32 dlgSoftFallbackSatF32ToS32 = new(SoftFallback.SatF32ToS32);
            SoftFallbackSatF32ToS64 dlgSoftFallbackSatF32ToS64 = new(SoftFallback.SatF32ToS64);
            SoftFallbackSatF32ToU32 dlgSoftFallbackSatF32ToU32 = new(SoftFallback.SatF32ToU32);
            SoftFallbackSatF32ToU64 dlgSoftFallbackSatF32ToU64 = new(SoftFallback.SatF32ToU64);
            SoftFallbackSatF64ToS32 dlgSoftFallbackSatF64ToS32 = new(SoftFallback.SatF64ToS32);
            SoftFallbackSatF64ToS64 dlgSoftFallbackSatF64ToS64 = new(SoftFallback.SatF64ToS64);
            SoftFallbackSatF64ToU32 dlgSoftFallbackSatF64ToU32 = new(SoftFallback.SatF64ToU32);
            SoftFallbackSatF64ToU64 dlgSoftFallbackSatF64ToU64 = new(SoftFallback.SatF64ToU64);
            SoftFallbackSha1SchedulePart1 dlgSoftFallbackSha1SchedulePart1 = new(SoftFallback.Sha1SchedulePart1);
            SoftFallbackSha1SchedulePart2 dlgSoftFallbackSha1SchedulePart2 = new(SoftFallback.Sha1SchedulePart2);
            SoftFallbackSha256SchedulePart1 dlgSoftFallbackSha256SchedulePart1 = new(SoftFallback.Sha256SchedulePart1);
            SoftFallbackSha256SchedulePart2 dlgSoftFallbackSha256SchedulePart2 = new(SoftFallback.Sha256SchedulePart2);
            SoftFallbackSignedShrImm64 dlgSoftFallbackSignedShrImm64 = new(SoftFallback.SignedShrImm64);
            SoftFallbackTbl1 dlgSoftFallbackTbl1 = new(SoftFallback.Tbl1);
            SoftFallbackTbl2 dlgSoftFallbackTbl2 = new(SoftFallback.Tbl2);
            SoftFallbackTbl3 dlgSoftFallbackTbl3 = new(SoftFallback.Tbl3);
            SoftFallbackTbl4 dlgSoftFallbackTbl4 = new(SoftFallback.Tbl4);
            SoftFallbackTbx1 dlgSoftFallbackTbx1 = new(SoftFallback.Tbx1);
            SoftFallbackTbx2 dlgSoftFallbackTbx2 = new(SoftFallback.Tbx2);
            SoftFallbackTbx3 dlgSoftFallbackTbx3 = new(SoftFallback.Tbx3);
            SoftFallbackTbx4 dlgSoftFallbackTbx4 = new(SoftFallback.Tbx4);
            SoftFallbackUnsignedShrImm64 dlgSoftFallbackUnsignedShrImm64 = new(SoftFallback.UnsignedShrImm64);

            SoftFloat16_32FPConvert dlgSoftFloat16_32FPConvert = new(SoftFloat16_32.FPConvert);
            SoftFloat16_64FPConvert dlgSoftFloat16_64FPConvert = new(SoftFloat16_64.FPConvert);

            SoftFloat32FPAdd dlgSoftFloat32FPAdd = new(SoftFloat32.FPAdd);
            SoftFloat32FPAddFpscr dlgSoftFloat32FPAddFpscr = new(SoftFloat32.FPAddFpscr); // A32 only.
            SoftFloat32FPCompare dlgSoftFloat32FPCompare = new(SoftFloat32.FPCompare);
            SoftFloat32FPCompareEQ dlgSoftFloat32FPCompareEQ = new(SoftFloat32.FPCompareEQ);
            SoftFloat32FPCompareEQFpscr dlgSoftFloat32FPCompareEQFpscr = new(SoftFloat32.FPCompareEQFpscr); // A32 only.
            SoftFloat32FPCompareGE dlgSoftFloat32FPCompareGE = new(SoftFloat32.FPCompareGE);
            SoftFloat32FPCompareGEFpscr dlgSoftFloat32FPCompareGEFpscr = new(SoftFloat32.FPCompareGEFpscr); // A32 only.
            SoftFloat32FPCompareGT dlgSoftFloat32FPCompareGT = new(SoftFloat32.FPCompareGT);
            SoftFloat32FPCompareGTFpscr dlgSoftFloat32FPCompareGTFpscr = new(SoftFloat32.FPCompareGTFpscr); // A32 only.
            SoftFloat32FPCompareLE dlgSoftFloat32FPCompareLE = new(SoftFloat32.FPCompareLE);
            SoftFloat32FPCompareLEFpscr dlgSoftFloat32FPCompareLEFpscr = new(SoftFloat32.FPCompareLEFpscr); // A32 only.
            SoftFloat32FPCompareLT dlgSoftFloat32FPCompareLT = new(SoftFloat32.FPCompareLT);
            SoftFloat32FPCompareLTFpscr dlgSoftFloat32FPCompareLTFpscr = new(SoftFloat32.FPCompareLTFpscr); // A32 only.
            SoftFloat32FPDiv dlgSoftFloat32FPDiv = new(SoftFloat32.FPDiv);
            SoftFloat32FPMax dlgSoftFloat32FPMax = new(SoftFloat32.FPMax);
            SoftFloat32FPMaxFpscr dlgSoftFloat32FPMaxFpscr = new(SoftFloat32.FPMaxFpscr); // A32 only.
            SoftFloat32FPMaxNum dlgSoftFloat32FPMaxNum = new(SoftFloat32.FPMaxNum);
            SoftFloat32FPMaxNumFpscr dlgSoftFloat32FPMaxNumFpscr = new(SoftFloat32.FPMaxNumFpscr); // A32 only.
            SoftFloat32FPMin dlgSoftFloat32FPMin = new(SoftFloat32.FPMin);
            SoftFloat32FPMinFpscr dlgSoftFloat32FPMinFpscr = new(SoftFloat32.FPMinFpscr); // A32 only.
            SoftFloat32FPMinNum dlgSoftFloat32FPMinNum = new(SoftFloat32.FPMinNum);
            SoftFloat32FPMinNumFpscr dlgSoftFloat32FPMinNumFpscr = new(SoftFloat32.FPMinNumFpscr); // A32 only.
            SoftFloat32FPMul dlgSoftFloat32FPMul = new(SoftFloat32.FPMul);
            SoftFloat32FPMulFpscr dlgSoftFloat32FPMulFpscr = new(SoftFloat32.FPMulFpscr); // A32 only.
            SoftFloat32FPMulAdd dlgSoftFloat32FPMulAdd = new(SoftFloat32.FPMulAdd);
            SoftFloat32FPMulAddFpscr dlgSoftFloat32FPMulAddFpscr = new(SoftFloat32.FPMulAddFpscr); // A32 only.
            SoftFloat32FPMulSub dlgSoftFloat32FPMulSub = new(SoftFloat32.FPMulSub);
            SoftFloat32FPMulSubFpscr dlgSoftFloat32FPMulSubFpscr = new(SoftFloat32.FPMulSubFpscr); // A32 only.
            SoftFloat32FPMulX dlgSoftFloat32FPMulX = new(SoftFloat32.FPMulX);
            SoftFloat32FPNegMulAdd dlgSoftFloat32FPNegMulAdd = new(SoftFloat32.FPNegMulAdd);
            SoftFloat32FPNegMulSub dlgSoftFloat32FPNegMulSub = new(SoftFloat32.FPNegMulSub);
            SoftFloat32FPRecipEstimate dlgSoftFloat32FPRecipEstimate = new(SoftFloat32.FPRecipEstimate);
            SoftFloat32FPRecipEstimateFpscr dlgSoftFloat32FPRecipEstimateFpscr = new(SoftFloat32.FPRecipEstimateFpscr); // A32 only.
            SoftFloat32FPRecipStep dlgSoftFloat32FPRecipStep = new(SoftFloat32.FPRecipStep); // A32 only.
            SoftFloat32FPRecipStepFused dlgSoftFloat32FPRecipStepFused = new(SoftFloat32.FPRecipStepFused);
            SoftFloat32FPRecpX dlgSoftFloat32FPRecpX = new(SoftFloat32.FPRecpX);
            SoftFloat32FPRSqrtEstimate dlgSoftFloat32FPRSqrtEstimate = new(SoftFloat32.FPRSqrtEstimate);
            SoftFloat32FPRSqrtEstimateFpscr dlgSoftFloat32FPRSqrtEstimateFpscr = new(SoftFloat32.FPRSqrtEstimateFpscr); // A32 only.
            SoftFloat32FPRSqrtStep dlgSoftFloat32FPRSqrtStep = new(SoftFloat32.FPRSqrtStep); // A32 only.
            SoftFloat32FPRSqrtStepFused dlgSoftFloat32FPRSqrtStepFused = new(SoftFloat32.FPRSqrtStepFused);
            SoftFloat32FPSqrt dlgSoftFloat32FPSqrt = new(SoftFloat32.FPSqrt);
            SoftFloat32FPSub dlgSoftFloat32FPSub = new(SoftFloat32.FPSub);

            SoftFloat32_16FPConvert dlgSoftFloat32_16FPConvert = new(SoftFloat32_16.FPConvert);

            SoftFloat64FPAdd dlgSoftFloat64FPAdd = new(SoftFloat64.FPAdd);
            SoftFloat64FPAddFpscr dlgSoftFloat64FPAddFpscr = new(SoftFloat64.FPAddFpscr); // A32 only.
            SoftFloat64FPCompare dlgSoftFloat64FPCompare = new(SoftFloat64.FPCompare);
            SoftFloat64FPCompareEQ dlgSoftFloat64FPCompareEQ = new(SoftFloat64.FPCompareEQ);
            SoftFloat64FPCompareEQFpscr dlgSoftFloat64FPCompareEQFpscr = new(SoftFloat64.FPCompareEQFpscr); // A32 only.
            SoftFloat64FPCompareGE dlgSoftFloat64FPCompareGE = new(SoftFloat64.FPCompareGE);
            SoftFloat64FPCompareGEFpscr dlgSoftFloat64FPCompareGEFpscr = new(SoftFloat64.FPCompareGEFpscr); // A32 only.
            SoftFloat64FPCompareGT dlgSoftFloat64FPCompareGT = new(SoftFloat64.FPCompareGT);
            SoftFloat64FPCompareGTFpscr dlgSoftFloat64FPCompareGTFpscr = new(SoftFloat64.FPCompareGTFpscr); // A32 only.
            SoftFloat64FPCompareLE dlgSoftFloat64FPCompareLE = new(SoftFloat64.FPCompareLE);
            SoftFloat64FPCompareLEFpscr dlgSoftFloat64FPCompareLEFpscr = new(SoftFloat64.FPCompareLEFpscr); // A32 only.
            SoftFloat64FPCompareLT dlgSoftFloat64FPCompareLT = new(SoftFloat64.FPCompareLT);
            SoftFloat64FPCompareLTFpscr dlgSoftFloat64FPCompareLTFpscr = new(SoftFloat64.FPCompareLTFpscr); // A32 only.
            SoftFloat64FPDiv dlgSoftFloat64FPDiv = new(SoftFloat64.FPDiv);
            SoftFloat64FPMax dlgSoftFloat64FPMax = new(SoftFloat64.FPMax);
            SoftFloat64FPMaxFpscr dlgSoftFloat64FPMaxFpscr = new(SoftFloat64.FPMaxFpscr); // A32 only.
            SoftFloat64FPMaxNum dlgSoftFloat64FPMaxNum = new(SoftFloat64.FPMaxNum);
            SoftFloat64FPMaxNumFpscr dlgSoftFloat64FPMaxNumFpscr = new(SoftFloat64.FPMaxNumFpscr); // A32 only.
            SoftFloat64FPMin dlgSoftFloat64FPMin = new(SoftFloat64.FPMin);
            SoftFloat64FPMinFpscr dlgSoftFloat64FPMinFpscr = new(SoftFloat64.FPMinFpscr); // A32 only.
            SoftFloat64FPMinNum dlgSoftFloat64FPMinNum = new(SoftFloat64.FPMinNum);
            SoftFloat64FPMinNumFpscr dlgSoftFloat64FPMinNumFpscr = new(SoftFloat64.FPMinNumFpscr); // A32 only.
            SoftFloat64FPMul dlgSoftFloat64FPMul = new(SoftFloat64.FPMul);
            SoftFloat64FPMulFpscr dlgSoftFloat64FPMulFpscr = new(SoftFloat64.FPMulFpscr); // A32 only.
            SoftFloat64FPMulAdd dlgSoftFloat64FPMulAdd = new(SoftFloat64.FPMulAdd);
            SoftFloat64FPMulAddFpscr dlgSoftFloat64FPMulAddFpscr = new(SoftFloat64.FPMulAddFpscr); // A32 only.
            SoftFloat64FPMulSub dlgSoftFloat64FPMulSub = new(SoftFloat64.FPMulSub);
            SoftFloat64FPMulSubFpscr dlgSoftFloat64FPMulSubFpscr = new(SoftFloat64.FPMulSubFpscr); // A32 only.
            SoftFloat64FPMulX dlgSoftFloat64FPMulX = new(SoftFloat64.FPMulX);
            SoftFloat64FPNegMulAdd dlgSoftFloat64FPNegMulAdd = new(SoftFloat64.FPNegMulAdd);
            SoftFloat64FPNegMulSub dlgSoftFloat64FPNegMulSub = new(SoftFloat64.FPNegMulSub);
            SoftFloat64FPRecipEstimate dlgSoftFloat64FPRecipEstimate = new(SoftFloat64.FPRecipEstimate);
            SoftFloat64FPRecipEstimateFpscr dlgSoftFloat64FPRecipEstimateFpscr = new(SoftFloat64.FPRecipEstimateFpscr); // A32 only.
            SoftFloat64FPRecipStep dlgSoftFloat64FPRecipStep = new(SoftFloat64.FPRecipStep); // A32 only.
            SoftFloat64FPRecipStepFused dlgSoftFloat64FPRecipStepFused = new(SoftFloat64.FPRecipStepFused);
            SoftFloat64FPRecpX dlgSoftFloat64FPRecpX = new(SoftFloat64.FPRecpX);
            SoftFloat64FPRSqrtEstimate dlgSoftFloat64FPRSqrtEstimate = new(SoftFloat64.FPRSqrtEstimate);
            SoftFloat64FPRSqrtEstimateFpscr dlgSoftFloat64FPRSqrtEstimateFpscr = new(SoftFloat64.FPRSqrtEstimateFpscr); // A32 only.
            SoftFloat64FPRSqrtStep dlgSoftFloat64FPRSqrtStep = new(SoftFloat64.FPRSqrtStep); // A32 only.
            SoftFloat64FPRSqrtStepFused dlgSoftFloat64FPRSqrtStepFused = new(SoftFloat64.FPRSqrtStepFused);
            SoftFloat64FPSqrt dlgSoftFloat64FPSqrt = new(SoftFloat64.FPSqrt);
            SoftFloat64FPSub dlgSoftFloat64FPSub = new(SoftFloat64.FPSub);

            SoftFloat64_16FPConvert dlgSoftFloat64_16FPConvert = new(SoftFloat64_16.FPConvert);
            // ReSharper restore InconsistentNaming
            // ReSharper restore RedundantDelegateCreation

            SetDelegateInfo(dlgMathAbs, Marshal.GetFunctionPointerForDelegate<MathAbs>(dlgMathAbs));
            SetDelegateInfo(dlgMathCeiling, Marshal.GetFunctionPointerForDelegate<MathCeiling>(dlgMathCeiling));
            SetDelegateInfo(dlgMathFloor, Marshal.GetFunctionPointerForDelegate<MathFloor>(dlgMathFloor));
            SetDelegateInfo(dlgMathRound, Marshal.GetFunctionPointerForDelegate<MathRound>(dlgMathRound));
            SetDelegateInfo(dlgMathTruncate, Marshal.GetFunctionPointerForDelegate<MathTruncate>(dlgMathTruncate));

            SetDelegateInfo(dlgMathFAbs, Marshal.GetFunctionPointerForDelegate<MathFAbs>(dlgMathFAbs));
            SetDelegateInfo(dlgMathFCeiling, Marshal.GetFunctionPointerForDelegate<MathFCeiling>(dlgMathFCeiling));
            SetDelegateInfo(dlgMathFFloor, Marshal.GetFunctionPointerForDelegate<MathFFloor>(dlgMathFFloor));
            SetDelegateInfo(dlgMathFRound, Marshal.GetFunctionPointerForDelegate<MathFRound>(dlgMathFRound));
            SetDelegateInfo(dlgMathFTruncate, Marshal.GetFunctionPointerForDelegate<MathFTruncate>(dlgMathFTruncate));

            SetDelegateInfo(dlgNativeInterfaceBreak, Marshal.GetFunctionPointerForDelegate<NativeInterfaceBreak>(dlgNativeInterfaceBreak));
            SetDelegateInfo(dlgNativeInterfaceCheckSynchronization, Marshal.GetFunctionPointerForDelegate<NativeInterfaceCheckSynchronization>(dlgNativeInterfaceCheckSynchronization));
            SetDelegateInfo(dlgNativeInterfaceEnqueueForRejit, Marshal.GetFunctionPointerForDelegate<NativeInterfaceEnqueueForRejit>(dlgNativeInterfaceEnqueueForRejit));
            SetDelegateInfo(dlgNativeInterfaceGetCntfrqEl0, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetCntfrqEl0>(dlgNativeInterfaceGetCntfrqEl0));
            SetDelegateInfo(dlgNativeInterfaceGetCntpctEl0, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetCntpctEl0>(dlgNativeInterfaceGetCntpctEl0));
            SetDelegateInfo(dlgNativeInterfaceGetCntvctEl0, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetCntvctEl0>(dlgNativeInterfaceGetCntvctEl0));
            SetDelegateInfo(dlgNativeInterfaceGetCtrEl0, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetCtrEl0>(dlgNativeInterfaceGetCtrEl0));
            SetDelegateInfo(dlgNativeInterfaceGetDczidEl0, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetDczidEl0>(dlgNativeInterfaceGetDczidEl0));
            SetDelegateInfo(dlgNativeInterfaceGetFunctionAddress, Marshal.GetFunctionPointerForDelegate<NativeInterfaceGetFunctionAddress>(dlgNativeInterfaceGetFunctionAddress));
            SetDelegateInfo(dlgNativeInterfaceInvalidateCacheLine, Marshal.GetFunctionPointerForDelegate<NativeInterfaceInvalidateCacheLine>(dlgNativeInterfaceInvalidateCacheLine));
            SetDelegateInfo(dlgNativeInterfaceReadByte, Marshal.GetFunctionPointerForDelegate<NativeInterfaceReadByte>(dlgNativeInterfaceReadByte));
            SetDelegateInfo(dlgNativeInterfaceReadUInt16, Marshal.GetFunctionPointerForDelegate<NativeInterfaceReadUInt16>(dlgNativeInterfaceReadUInt16));
            SetDelegateInfo(dlgNativeInterfaceReadUInt32, Marshal.GetFunctionPointerForDelegate<NativeInterfaceReadUInt32>(dlgNativeInterfaceReadUInt32));
            SetDelegateInfo(dlgNativeInterfaceReadUInt64, Marshal.GetFunctionPointerForDelegate<NativeInterfaceReadUInt64>(dlgNativeInterfaceReadUInt64));
            SetDelegateInfo(dlgNativeInterfaceReadVector128, Marshal.GetFunctionPointerForDelegate<NativeInterfaceReadVector128>(dlgNativeInterfaceReadVector128));
            SetDelegateInfo(dlgNativeInterfaceSignalMemoryTracking, Marshal.GetFunctionPointerForDelegate<NativeInterfaceSignalMemoryTracking>(dlgNativeInterfaceSignalMemoryTracking));
            SetDelegateInfo(dlgNativeInterfaceSupervisorCall, Marshal.GetFunctionPointerForDelegate<NativeInterfaceSupervisorCall>(dlgNativeInterfaceSupervisorCall));
            SetDelegateInfo(dlgNativeInterfaceThrowInvalidMemoryAccess, Marshal.GetFunctionPointerForDelegate<NativeInterfaceThrowInvalidMemoryAccess>(dlgNativeInterfaceThrowInvalidMemoryAccess));
            SetDelegateInfo(dlgNativeInterfaceUndefined, Marshal.GetFunctionPointerForDelegate<NativeInterfaceUndefined>(dlgNativeInterfaceUndefined));
            SetDelegateInfo(dlgNativeInterfaceWriteByte, Marshal.GetFunctionPointerForDelegate<NativeInterfaceWriteByte>(dlgNativeInterfaceWriteByte));
            SetDelegateInfo(dlgNativeInterfaceWriteUInt16, Marshal.GetFunctionPointerForDelegate<NativeInterfaceWriteUInt16>(dlgNativeInterfaceWriteUInt16));
            SetDelegateInfo(dlgNativeInterfaceWriteUInt32, Marshal.GetFunctionPointerForDelegate<NativeInterfaceWriteUInt32>(dlgNativeInterfaceWriteUInt32));
            SetDelegateInfo(dlgNativeInterfaceWriteUInt64, Marshal.GetFunctionPointerForDelegate<NativeInterfaceWriteUInt64>(dlgNativeInterfaceWriteUInt64));
            SetDelegateInfo(dlgNativeInterfaceWriteVector128, Marshal.GetFunctionPointerForDelegate<NativeInterfaceWriteVector128>(dlgNativeInterfaceWriteVector128));

            SetDelegateInfo(dlgSoftFallbackCountLeadingSigns, Marshal.GetFunctionPointerForDelegate<SoftFallbackCountLeadingSigns>(dlgSoftFallbackCountLeadingSigns));
            SetDelegateInfo(dlgSoftFallbackCountLeadingZeros, Marshal.GetFunctionPointerForDelegate<SoftFallbackCountLeadingZeros>(dlgSoftFallbackCountLeadingZeros));
            SetDelegateInfo(dlgSoftFallbackCrc32b, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32b>(dlgSoftFallbackCrc32b));
            SetDelegateInfo(dlgSoftFallbackCrc32cb, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32cb>(dlgSoftFallbackCrc32cb));
            SetDelegateInfo(dlgSoftFallbackCrc32ch, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32ch>(dlgSoftFallbackCrc32ch));
            SetDelegateInfo(dlgSoftFallbackCrc32cw, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32cw>(dlgSoftFallbackCrc32cw));
            SetDelegateInfo(dlgSoftFallbackCrc32cx, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32cx>(dlgSoftFallbackCrc32cx));
            SetDelegateInfo(dlgSoftFallbackCrc32h, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32h>(dlgSoftFallbackCrc32h));
            SetDelegateInfo(dlgSoftFallbackCrc32w, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32w>(dlgSoftFallbackCrc32w));
            SetDelegateInfo(dlgSoftFallbackCrc32x, Marshal.GetFunctionPointerForDelegate<SoftFallbackCrc32x>(dlgSoftFallbackCrc32x));
            SetDelegateInfo(dlgSoftFallbackDecrypt, Marshal.GetFunctionPointerForDelegate<SoftFallbackDecrypt>(dlgSoftFallbackDecrypt));
            SetDelegateInfo(dlgSoftFallbackEncrypt, Marshal.GetFunctionPointerForDelegate<SoftFallbackEncrypt>(dlgSoftFallbackEncrypt));
            SetDelegateInfo(dlgSoftFallbackFixedRotate, Marshal.GetFunctionPointerForDelegate<SoftFallbackFixedRotate>(dlgSoftFallbackFixedRotate));
            SetDelegateInfo(dlgSoftFallbackHashChoose, Marshal.GetFunctionPointerForDelegate<SoftFallbackHashChoose>(dlgSoftFallbackHashChoose));
            SetDelegateInfo(dlgSoftFallbackHashLower, Marshal.GetFunctionPointerForDelegate<SoftFallbackHashLower>(dlgSoftFallbackHashLower));
            SetDelegateInfo(dlgSoftFallbackHashMajority, Marshal.GetFunctionPointerForDelegate<SoftFallbackHashMajority>(dlgSoftFallbackHashMajority));
            SetDelegateInfo(dlgSoftFallbackHashParity, Marshal.GetFunctionPointerForDelegate<SoftFallbackHashParity>(dlgSoftFallbackHashParity));
            SetDelegateInfo(dlgSoftFallbackHashUpper, Marshal.GetFunctionPointerForDelegate<SoftFallbackHashUpper>(dlgSoftFallbackHashUpper));
            SetDelegateInfo(dlgSoftFallbackInverseMixColumns, Marshal.GetFunctionPointerForDelegate<SoftFallbackInverseMixColumns>(dlgSoftFallbackInverseMixColumns));
            SetDelegateInfo(dlgSoftFallbackMixColumns, Marshal.GetFunctionPointerForDelegate<SoftFallbackMixColumns>(dlgSoftFallbackMixColumns));
            SetDelegateInfo(dlgSoftFallbackPolynomialMult64_128, Marshal.GetFunctionPointerForDelegate<SoftFallbackPolynomialMult64_128>(dlgSoftFallbackPolynomialMult64_128));
            SetDelegateInfo(dlgSoftFallbackSatF32ToS32, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF32ToS32>(dlgSoftFallbackSatF32ToS32));
            SetDelegateInfo(dlgSoftFallbackSatF32ToS64, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF32ToS64>(dlgSoftFallbackSatF32ToS64));
            SetDelegateInfo(dlgSoftFallbackSatF32ToU32, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF32ToU32>(dlgSoftFallbackSatF32ToU32));
            SetDelegateInfo(dlgSoftFallbackSatF32ToU64, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF32ToU64>(dlgSoftFallbackSatF32ToU64));
            SetDelegateInfo(dlgSoftFallbackSatF64ToS32, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF64ToS32>(dlgSoftFallbackSatF64ToS32));
            SetDelegateInfo(dlgSoftFallbackSatF64ToS64, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF64ToS64>(dlgSoftFallbackSatF64ToS64));
            SetDelegateInfo(dlgSoftFallbackSatF64ToU32, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF64ToU32>(dlgSoftFallbackSatF64ToU32));
            SetDelegateInfo(dlgSoftFallbackSatF64ToU64, Marshal.GetFunctionPointerForDelegate<SoftFallbackSatF64ToU64>(dlgSoftFallbackSatF64ToU64));
            SetDelegateInfo(dlgSoftFallbackSha1SchedulePart1, Marshal.GetFunctionPointerForDelegate<SoftFallbackSha1SchedulePart1>(dlgSoftFallbackSha1SchedulePart1));
            SetDelegateInfo(dlgSoftFallbackSha1SchedulePart2, Marshal.GetFunctionPointerForDelegate<SoftFallbackSha1SchedulePart2>(dlgSoftFallbackSha1SchedulePart2));
            SetDelegateInfo(dlgSoftFallbackSha256SchedulePart1, Marshal.GetFunctionPointerForDelegate<SoftFallbackSha256SchedulePart1>(dlgSoftFallbackSha256SchedulePart1));
            SetDelegateInfo(dlgSoftFallbackSha256SchedulePart2, Marshal.GetFunctionPointerForDelegate<SoftFallbackSha256SchedulePart2>(dlgSoftFallbackSha256SchedulePart2));
            SetDelegateInfo(dlgSoftFallbackSignedShrImm64, Marshal.GetFunctionPointerForDelegate<SoftFallbackSignedShrImm64>(dlgSoftFallbackSignedShrImm64));
            SetDelegateInfo(dlgSoftFallbackTbl1, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbl1>(dlgSoftFallbackTbl1));
            SetDelegateInfo(dlgSoftFallbackTbl2, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbl2>(dlgSoftFallbackTbl2));
            SetDelegateInfo(dlgSoftFallbackTbl3, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbl3>(dlgSoftFallbackTbl3));
            SetDelegateInfo(dlgSoftFallbackTbl4, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbl4>(dlgSoftFallbackTbl4));
            SetDelegateInfo(dlgSoftFallbackTbx1, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbx1>(dlgSoftFallbackTbx1));
            SetDelegateInfo(dlgSoftFallbackTbx2, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbx2>(dlgSoftFallbackTbx2));
            SetDelegateInfo(dlgSoftFallbackTbx3, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbx3>(dlgSoftFallbackTbx3));
            SetDelegateInfo(dlgSoftFallbackTbx4, Marshal.GetFunctionPointerForDelegate<SoftFallbackTbx4>(dlgSoftFallbackTbx4));
            SetDelegateInfo(dlgSoftFallbackUnsignedShrImm64, Marshal.GetFunctionPointerForDelegate<SoftFallbackUnsignedShrImm64>(dlgSoftFallbackUnsignedShrImm64));

            SetDelegateInfo(dlgSoftFloat16_32FPConvert, Marshal.GetFunctionPointerForDelegate<SoftFloat16_32FPConvert>(dlgSoftFloat16_32FPConvert));
            SetDelegateInfo(dlgSoftFloat16_64FPConvert, Marshal.GetFunctionPointerForDelegate<SoftFloat16_64FPConvert>(dlgSoftFloat16_64FPConvert));

            SetDelegateInfo(dlgSoftFloat32FPAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPAdd>(dlgSoftFloat32FPAdd));
            SetDelegateInfo(dlgSoftFloat32FPAddFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPAddFpscr>(dlgSoftFloat32FPAddFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPCompare, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompare>(dlgSoftFloat32FPCompare));
            SetDelegateInfo(dlgSoftFloat32FPCompareEQ, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareEQ>(dlgSoftFloat32FPCompareEQ));
            SetDelegateInfo(dlgSoftFloat32FPCompareEQFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareEQFpscr>(dlgSoftFloat32FPCompareEQFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPCompareGE, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareGE>(dlgSoftFloat32FPCompareGE));
            SetDelegateInfo(dlgSoftFloat32FPCompareGEFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareGEFpscr>(dlgSoftFloat32FPCompareGEFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPCompareGT, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareGT>(dlgSoftFloat32FPCompareGT));
            SetDelegateInfo(dlgSoftFloat32FPCompareGTFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareGTFpscr>(dlgSoftFloat32FPCompareGTFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPCompareLE, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareLE>(dlgSoftFloat32FPCompareLE));
            SetDelegateInfo(dlgSoftFloat32FPCompareLEFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareLEFpscr>(dlgSoftFloat32FPCompareLEFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPCompareLT, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareLT>(dlgSoftFloat32FPCompareLT));
            SetDelegateInfo(dlgSoftFloat32FPCompareLTFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPCompareLTFpscr>(dlgSoftFloat32FPCompareLTFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPDiv, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPDiv>(dlgSoftFloat32FPDiv));
            SetDelegateInfo(dlgSoftFloat32FPMax, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMax>(dlgSoftFloat32FPMax));
            SetDelegateInfo(dlgSoftFloat32FPMaxFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMaxFpscr>(dlgSoftFloat32FPMaxFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMaxNum, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMaxNum>(dlgSoftFloat32FPMaxNum));
            SetDelegateInfo(dlgSoftFloat32FPMaxNumFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMaxNumFpscr>(dlgSoftFloat32FPMaxNumFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMin, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMin>(dlgSoftFloat32FPMin));
            SetDelegateInfo(dlgSoftFloat32FPMinFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMinFpscr>(dlgSoftFloat32FPMinFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMinNum, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMinNum>(dlgSoftFloat32FPMinNum));
            SetDelegateInfo(dlgSoftFloat32FPMinNumFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMinNumFpscr>(dlgSoftFloat32FPMinNumFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMul, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMul>(dlgSoftFloat32FPMul));
            SetDelegateInfo(dlgSoftFloat32FPMulFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulFpscr>(dlgSoftFloat32FPMulFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMulAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulAdd>(dlgSoftFloat32FPMulAdd));
            SetDelegateInfo(dlgSoftFloat32FPMulAddFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulAddFpscr>(dlgSoftFloat32FPMulAddFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMulSub, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulSub>(dlgSoftFloat32FPMulSub));
            SetDelegateInfo(dlgSoftFloat32FPMulSubFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulSubFpscr>(dlgSoftFloat32FPMulSubFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPMulX, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPMulX>(dlgSoftFloat32FPMulX));
            SetDelegateInfo(dlgSoftFloat32FPNegMulAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPNegMulAdd>(dlgSoftFloat32FPNegMulAdd));
            SetDelegateInfo(dlgSoftFloat32FPNegMulSub, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPNegMulSub>(dlgSoftFloat32FPNegMulSub));
            SetDelegateInfo(dlgSoftFloat32FPRecipEstimate, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRecipEstimate>(dlgSoftFloat32FPRecipEstimate));
            SetDelegateInfo(dlgSoftFloat32FPRecipEstimateFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRecipEstimateFpscr>(dlgSoftFloat32FPRecipEstimateFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPRecipStep, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRecipStep>(dlgSoftFloat32FPRecipStep)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPRecipStepFused, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRecipStepFused>(dlgSoftFloat32FPRecipStepFused));
            SetDelegateInfo(dlgSoftFloat32FPRecpX, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRecpX>(dlgSoftFloat32FPRecpX));
            SetDelegateInfo(dlgSoftFloat32FPRSqrtEstimate, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRSqrtEstimate>(dlgSoftFloat32FPRSqrtEstimate));
            SetDelegateInfo(dlgSoftFloat32FPRSqrtEstimateFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRSqrtEstimateFpscr>(dlgSoftFloat32FPRSqrtEstimateFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPRSqrtStep, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRSqrtStep>(dlgSoftFloat32FPRSqrtStep)); // A32 only.
            SetDelegateInfo(dlgSoftFloat32FPRSqrtStepFused, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPRSqrtStepFused>(dlgSoftFloat32FPRSqrtStepFused));
            SetDelegateInfo(dlgSoftFloat32FPSqrt, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPSqrt>(dlgSoftFloat32FPSqrt));
            SetDelegateInfo(dlgSoftFloat32FPSub, Marshal.GetFunctionPointerForDelegate<SoftFloat32FPSub>(dlgSoftFloat32FPSub));

            SetDelegateInfo(dlgSoftFloat32_16FPConvert, Marshal.GetFunctionPointerForDelegate<SoftFloat32_16FPConvert>(dlgSoftFloat32_16FPConvert));

            SetDelegateInfo(dlgSoftFloat64FPAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPAdd>(dlgSoftFloat64FPAdd));
            SetDelegateInfo(dlgSoftFloat64FPAddFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPAddFpscr>(dlgSoftFloat64FPAddFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPCompare, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompare>(dlgSoftFloat64FPCompare));
            SetDelegateInfo(dlgSoftFloat64FPCompareEQ, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareEQ>(dlgSoftFloat64FPCompareEQ));
            SetDelegateInfo(dlgSoftFloat64FPCompareEQFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareEQFpscr>(dlgSoftFloat64FPCompareEQFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPCompareGE, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareGE>(dlgSoftFloat64FPCompareGE));
            SetDelegateInfo(dlgSoftFloat64FPCompareGEFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareGEFpscr>(dlgSoftFloat64FPCompareGEFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPCompareGT, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareGT>(dlgSoftFloat64FPCompareGT));
            SetDelegateInfo(dlgSoftFloat64FPCompareGTFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareGTFpscr>(dlgSoftFloat64FPCompareGTFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPCompareLE, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareLE>(dlgSoftFloat64FPCompareLE));
            SetDelegateInfo(dlgSoftFloat64FPCompareLEFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareLEFpscr>(dlgSoftFloat64FPCompareLEFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPCompareLT, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareLT>(dlgSoftFloat64FPCompareLT));
            SetDelegateInfo(dlgSoftFloat64FPCompareLTFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPCompareLTFpscr>(dlgSoftFloat64FPCompareLTFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPDiv, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPDiv>(dlgSoftFloat64FPDiv));
            SetDelegateInfo(dlgSoftFloat64FPMax, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMax>(dlgSoftFloat64FPMax));
            SetDelegateInfo(dlgSoftFloat64FPMaxFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMaxFpscr>(dlgSoftFloat64FPMaxFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMaxNum, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMaxNum>(dlgSoftFloat64FPMaxNum));
            SetDelegateInfo(dlgSoftFloat64FPMaxNumFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMaxNumFpscr>(dlgSoftFloat64FPMaxNumFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMin, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMin>(dlgSoftFloat64FPMin));
            SetDelegateInfo(dlgSoftFloat64FPMinFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMinFpscr>(dlgSoftFloat64FPMinFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMinNum, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMinNum>(dlgSoftFloat64FPMinNum));
            SetDelegateInfo(dlgSoftFloat64FPMinNumFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMinNumFpscr>(dlgSoftFloat64FPMinNumFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMul, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMul>(dlgSoftFloat64FPMul));
            SetDelegateInfo(dlgSoftFloat64FPMulFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulFpscr>(dlgSoftFloat64FPMulFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMulAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulAdd>(dlgSoftFloat64FPMulAdd));
            SetDelegateInfo(dlgSoftFloat64FPMulAddFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulAddFpscr>(dlgSoftFloat64FPMulAddFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMulSub, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulSub>(dlgSoftFloat64FPMulSub));
            SetDelegateInfo(dlgSoftFloat64FPMulSubFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulSubFpscr>(dlgSoftFloat64FPMulSubFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPMulX, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPMulX>(dlgSoftFloat64FPMulX));
            SetDelegateInfo(dlgSoftFloat64FPNegMulAdd, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPNegMulAdd>(dlgSoftFloat64FPNegMulAdd));
            SetDelegateInfo(dlgSoftFloat64FPNegMulSub, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPNegMulSub>(dlgSoftFloat64FPNegMulSub));
            SetDelegateInfo(dlgSoftFloat64FPRecipEstimate, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRecipEstimate>(dlgSoftFloat64FPRecipEstimate));
            SetDelegateInfo(dlgSoftFloat64FPRecipEstimateFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRecipEstimateFpscr>(dlgSoftFloat64FPRecipEstimateFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPRecipStep, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRecipStep>(dlgSoftFloat64FPRecipStep)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPRecipStepFused, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRecipStepFused>(dlgSoftFloat64FPRecipStepFused));
            SetDelegateInfo(dlgSoftFloat64FPRecpX, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRecpX>(dlgSoftFloat64FPRecpX));
            SetDelegateInfo(dlgSoftFloat64FPRSqrtEstimate, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRSqrtEstimate>(dlgSoftFloat64FPRSqrtEstimate));
            SetDelegateInfo(dlgSoftFloat64FPRSqrtEstimateFpscr, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRSqrtEstimateFpscr>(dlgSoftFloat64FPRSqrtEstimateFpscr)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPRSqrtStep, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRSqrtStep>(dlgSoftFloat64FPRSqrtStep)); // A32 only.
            SetDelegateInfo(dlgSoftFloat64FPRSqrtStepFused, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPRSqrtStepFused>(dlgSoftFloat64FPRSqrtStepFused));
            SetDelegateInfo(dlgSoftFloat64FPSqrt, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPSqrt>(dlgSoftFloat64FPSqrt));
            SetDelegateInfo(dlgSoftFloat64FPSub, Marshal.GetFunctionPointerForDelegate<SoftFloat64FPSub>(dlgSoftFloat64FPSub));

            SetDelegateInfo(dlgSoftFloat64_16FPConvert, Marshal.GetFunctionPointerForDelegate<SoftFloat64_16FPConvert>(dlgSoftFloat64_16FPConvert));
        }

        private delegate double MathAbs(double value);
        private delegate double MathCeiling(double a);
        private delegate double MathFloor(double d);
        private delegate double MathRound(double value, MidpointRounding mode);
        private delegate double MathTruncate(double d);

        private delegate float MathFAbs(float x);
        private delegate float MathFCeiling(float x);
        private delegate float MathFFloor(float x);
        private delegate float MathFRound(float x, MidpointRounding mode);
        private delegate float MathFTruncate(float x);

        private delegate void NativeInterfaceBreak(ulong address, int imm);
        private delegate bool NativeInterfaceCheckSynchronization();
        private delegate void NativeInterfaceEnqueueForRejit(ulong address);
        private delegate ulong NativeInterfaceGetCntfrqEl0();
        private delegate ulong NativeInterfaceGetCntpctEl0();
        private delegate ulong NativeInterfaceGetCntvctEl0();
        private delegate ulong NativeInterfaceGetCtrEl0();
        private delegate ulong NativeInterfaceGetDczidEl0();
        private delegate ulong NativeInterfaceGetFunctionAddress(ulong address);
        private delegate void NativeInterfaceInvalidateCacheLine(ulong address);
        private delegate byte NativeInterfaceReadByte(ulong address);
        private delegate ushort NativeInterfaceReadUInt16(ulong address);
        private delegate uint NativeInterfaceReadUInt32(ulong address);
        private delegate ulong NativeInterfaceReadUInt64(ulong address);
        private delegate V128 NativeInterfaceReadVector128(ulong address);
        private delegate void NativeInterfaceSignalMemoryTracking(ulong address, ulong size, bool write);
        private delegate void NativeInterfaceSupervisorCall(ulong address, int imm);
        private delegate void NativeInterfaceThrowInvalidMemoryAccess(ulong address);
        private delegate void NativeInterfaceUndefined(ulong address, int opCode);
        private delegate void NativeInterfaceWriteByte(ulong address, byte value);
        private delegate void NativeInterfaceWriteUInt16(ulong address, ushort value);
        private delegate void NativeInterfaceWriteUInt32(ulong address, uint value);
        private delegate void NativeInterfaceWriteUInt64(ulong address, ulong value);
        private delegate void NativeInterfaceWriteVector128(ulong address, V128 value);

        private delegate ulong SoftFallbackCountLeadingSigns(ulong value, int size);
        private delegate ulong SoftFallbackCountLeadingZeros(ulong value, int size);
        private delegate uint SoftFallbackCrc32b(uint crc, byte value);
        private delegate uint SoftFallbackCrc32cb(uint crc, byte value);
        private delegate uint SoftFallbackCrc32ch(uint crc, ushort value);
        private delegate uint SoftFallbackCrc32cw(uint crc, uint value);
        private delegate uint SoftFallbackCrc32cx(uint crc, ulong value);
        private delegate uint SoftFallbackCrc32h(uint crc, ushort value);
        private delegate uint SoftFallbackCrc32w(uint crc, uint value);
        private delegate uint SoftFallbackCrc32x(uint crc, ulong value);
        private delegate V128 SoftFallbackDecrypt(V128 value, V128 roundKey);
        private delegate V128 SoftFallbackEncrypt(V128 value, V128 roundKey);
        private delegate uint SoftFallbackFixedRotate(uint hash_e);
        private delegate V128 SoftFallbackHashChoose(V128 hash_abcd, uint hash_e, V128 wk);
        private delegate V128 SoftFallbackHashLower(V128 hash_abcd, V128 hash_efgh, V128 wk);
        private delegate V128 SoftFallbackHashMajority(V128 hash_abcd, uint hash_e, V128 wk);
        private delegate V128 SoftFallbackHashParity(V128 hash_abcd, uint hash_e, V128 wk);
        private delegate V128 SoftFallbackHashUpper(V128 hash_abcd, V128 hash_efgh, V128 wk);
        private delegate V128 SoftFallbackInverseMixColumns(V128 value);
        private delegate V128 SoftFallbackMixColumns(V128 value);
        private delegate V128 SoftFallbackPolynomialMult64_128(ulong op1, ulong op2);
        private delegate int SoftFallbackSatF32ToS32(float value);
        private delegate long SoftFallbackSatF32ToS64(float value);
        private delegate uint SoftFallbackSatF32ToU32(float value);
        private delegate ulong SoftFallbackSatF32ToU64(float value);
        private delegate int SoftFallbackSatF64ToS32(double value);
        private delegate long SoftFallbackSatF64ToS64(double value);
        private delegate uint SoftFallbackSatF64ToU32(double value);
        private delegate ulong SoftFallbackSatF64ToU64(double value);
        private delegate V128 SoftFallbackSha1SchedulePart1(V128 w0_3, V128 w4_7, V128 w8_11);
        private delegate V128 SoftFallbackSha1SchedulePart2(V128 tw0_3, V128 w12_15);
        private delegate V128 SoftFallbackSha256SchedulePart1(V128 w0_3, V128 w4_7);
        private delegate V128 SoftFallbackSha256SchedulePart2(V128 w0_3, V128 w8_11, V128 w12_15);
        private delegate long SoftFallbackSignedShrImm64(long value, long roundConst, int shift);
        private delegate V128 SoftFallbackTbl1(V128 vector, int bytes, V128 tb0);
        private delegate V128 SoftFallbackTbl2(V128 vector, int bytes, V128 tb0, V128 tb1);
        private delegate V128 SoftFallbackTbl3(V128 vector, int bytes, V128 tb0, V128 tb1, V128 tb2);
        private delegate V128 SoftFallbackTbl4(V128 vector, int bytes, V128 tb0, V128 tb1, V128 tb2, V128 tb3);
        private delegate V128 SoftFallbackTbx1(V128 dest, V128 vector, int bytes, V128 tb0);
        private delegate V128 SoftFallbackTbx2(V128 dest, V128 vector, int bytes, V128 tb0, V128 tb1);
        private delegate V128 SoftFallbackTbx3(V128 dest, V128 vector, int bytes, V128 tb0, V128 tb1, V128 tb2);
        private delegate V128 SoftFallbackTbx4(V128 dest, V128 vector, int bytes, V128 tb0, V128 tb1, V128 tb2, V128 tb3);
        private delegate ulong SoftFallbackUnsignedShrImm64(ulong value, long roundConst, int shift);

        private delegate float SoftFloat16_32FPConvert(ushort valueBits);

        private delegate double SoftFloat16_64FPConvert(ushort valueBits);

        private delegate float SoftFloat32FPAdd(float value1, float value2);
        private delegate float SoftFloat32FPAddFpscr(float value1, float value2, bool standardFpscr);
        private delegate int SoftFloat32FPCompare(float value1, float value2, bool signalNaNs);
        private delegate float SoftFloat32FPCompareEQ(float value1, float value2);
        private delegate float SoftFloat32FPCompareEQFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPCompareGE(float value1, float value2);
        private delegate float SoftFloat32FPCompareGEFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPCompareGT(float value1, float value2);
        private delegate float SoftFloat32FPCompareGTFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPCompareLE(float value1, float value2);
        private delegate float SoftFloat32FPCompareLEFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPCompareLT(float value1, float value2);
        private delegate float SoftFloat32FPCompareLTFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPDiv(float value1, float value2);
        private delegate float SoftFloat32FPMax(float value1, float value2);
        private delegate float SoftFloat32FPMaxFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMaxNum(float value1, float value2);
        private delegate float SoftFloat32FPMaxNumFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMin(float value1, float value2);
        private delegate float SoftFloat32FPMinFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMinNum(float value1, float value2);
        private delegate float SoftFloat32FPMinNumFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMul(float value1, float value2);
        private delegate float SoftFloat32FPMulFpscr(float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMulAdd(float valueA, float value1, float value2);
        private delegate float SoftFloat32FPMulAddFpscr(float valueA, float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMulSub(float valueA, float value1, float value2);
        private delegate float SoftFloat32FPMulSubFpscr(float valueA, float value1, float value2, bool standardFpscr);
        private delegate float SoftFloat32FPMulX(float value1, float value2);
        private delegate float SoftFloat32FPNegMulAdd(float valueA, float value1, float value2);
        private delegate float SoftFloat32FPNegMulSub(float valueA, float value1, float value2);
        private delegate float SoftFloat32FPRecipEstimate(float value);
        private delegate float SoftFloat32FPRecipEstimateFpscr(float value, bool standardFpscr);
        private delegate float SoftFloat32FPRecipStep(float value1, float value2);
        private delegate float SoftFloat32FPRecipStepFused(float value1, float value2);
        private delegate float SoftFloat32FPRecpX(float value);
        private delegate float SoftFloat32FPRSqrtEstimate(float value);
        private delegate float SoftFloat32FPRSqrtEstimateFpscr(float value, bool standardFpscr);
        private delegate float SoftFloat32FPRSqrtStep(float value1, float value2);
        private delegate float SoftFloat32FPRSqrtStepFused(float value1, float value2);
        private delegate float SoftFloat32FPSqrt(float value);
        private delegate float SoftFloat32FPSub(float value1, float value2);

        private delegate ushort SoftFloat32_16FPConvert(float value);

        private delegate double SoftFloat64FPAdd(double value1, double value2);
        private delegate double SoftFloat64FPAddFpscr(double value1, double value2, bool standardFpscr);
        private delegate int SoftFloat64FPCompare(double value1, double value2, bool signalNaNs);
        private delegate double SoftFloat64FPCompareEQ(double value1, double value2);
        private delegate double SoftFloat64FPCompareEQFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPCompareGE(double value1, double value2);
        private delegate double SoftFloat64FPCompareGEFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPCompareGT(double value1, double value2);
        private delegate double SoftFloat64FPCompareGTFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPCompareLE(double value1, double value2);
        private delegate double SoftFloat64FPCompareLEFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPCompareLT(double value1, double value2);
        private delegate double SoftFloat64FPCompareLTFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPDiv(double value1, double value2);
        private delegate double SoftFloat64FPMax(double value1, double value2);
        private delegate double SoftFloat64FPMaxFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMaxNum(double value1, double value2);
        private delegate double SoftFloat64FPMaxNumFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMin(double value1, double value2);
        private delegate double SoftFloat64FPMinFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMinNum(double value1, double value2);
        private delegate double SoftFloat64FPMinNumFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMul(double value1, double value2);
        private delegate double SoftFloat64FPMulFpscr(double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMulAdd(double valueA, double value1, double value2);
        private delegate double SoftFloat64FPMulAddFpscr(double valueA, double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMulSub(double valueA, double value1, double value2);
        private delegate double SoftFloat64FPMulSubFpscr(double valueA, double value1, double value2, bool standardFpscr);
        private delegate double SoftFloat64FPMulX(double value1, double value2);
        private delegate double SoftFloat64FPNegMulAdd(double valueA, double value1, double value2);
        private delegate double SoftFloat64FPNegMulSub(double valueA, double value1, double value2);
        private delegate double SoftFloat64FPRecipEstimate(double value);
        private delegate double SoftFloat64FPRecipEstimateFpscr(double value, bool standardFpscr);
        private delegate double SoftFloat64FPRecipStep(double value1, double value2);
        private delegate double SoftFloat64FPRecipStepFused(double value1, double value2);
        private delegate double SoftFloat64FPRecpX(double value);
        private delegate double SoftFloat64FPRSqrtEstimate(double value);
        private delegate double SoftFloat64FPRSqrtEstimateFpscr(double value, bool standardFpscr);
        private delegate double SoftFloat64FPRSqrtStep(double value1, double value2);
        private delegate double SoftFloat64FPRSqrtStepFused(double value1, double value2);
        private delegate double SoftFloat64FPSqrt(double value);
        private delegate double SoftFloat64FPSub(double value1, double value2);

        private delegate ushort SoftFloat64_16FPConvert(double value);
    }
}
