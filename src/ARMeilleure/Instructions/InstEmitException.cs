using ARMeilleure.Decoders;
using ARMeilleure.Translation;

using static ARMeilleure.IntermediateRepresentation.Operand.Factory;

namespace ARMeilleure.Instructions
{
    static partial class InstEmit
    {
        private const string SupervisorCallName = nameof(NativeInterface.SupervisorCall);
        private const string BreakName = nameof(NativeInterface.Break);
        private const string UndefinedName = nameof(NativeInterface.Undefined);
        
        public static void Brk(ArmEmitterContext context)
        {
            OpCodeException op = (OpCodeException)context.CurrOp;

            context.StoreToContext();

            context.Call(NativeInterface.Type.GetMethod(BreakName), Const(op.Address), Const(op.Id));

            context.LoadFromContext();

            context.Return(Const(op.Address));
        }

        public static void Svc(ArmEmitterContext context)
        {
            OpCodeException op = (OpCodeException)context.CurrOp;
            
            context.StoreToContext();

            context.Call(NativeInterface.Type.GetMethod(SupervisorCallName), Const(op.Address), Const(op.Id));

            context.LoadFromContext();

            Translator.EmitSynchronization(context);
        }

        public static void Und(ArmEmitterContext context)
        {
            OpCode op = context.CurrOp;

            context.StoreToContext();

            context.Call(NativeInterface.Type.GetMethod(UndefinedName), Const(op.Address), Const(op.RawOpCode));

            context.LoadFromContext();

            context.Return(Const(op.Address));
        }
    }
}
