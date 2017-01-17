using System;

namespace Engine.Common
{
    public class DataEventArgs<T1> : EventArgs
    {
        public T1 Arg1;

        public DataEventArgs(T1 t)
        {
            Arg1 = t;
        }
    }
    public class DataEventArgs<T1, T2> : EventArgs
    {
        public T1 Arg1;
        public T2 Arg2;

        public DataEventArgs(T1 t1, T2 t2)
        {
            Arg1 = t1;
            Arg2 = t2;
        }
    }
}
