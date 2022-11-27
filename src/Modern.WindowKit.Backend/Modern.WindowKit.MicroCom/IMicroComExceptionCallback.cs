using System;

namespace Modern.WindowKit.MicroCom
{
    public interface IMicroComExceptionCallback
    {
        void RaiseException(Exception e);
    }
}
