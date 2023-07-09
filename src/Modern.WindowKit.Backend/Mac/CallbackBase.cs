using System;
using System.Runtime.ExceptionServices;
using Modern.WindowKit.MicroCom;

namespace Modern.WindowKit.Mac
{
    internal abstract class NativeCallbackBase : CallbackBase, IMicroComExceptionCallback
    {
        public void RaiseException(Exception e)
        {
            Console.WriteLine(e.ToString());
        //    if (AvaloniaLocator.Current.GetService<IDispatcherImpl>() is DispatcherImpl dispatcherImpl)
        //    {
        //        dispatcherImpl.PropagateCallbackException(ExceptionDispatchInfo.Capture(e));
        //    }
        }
    }
}
