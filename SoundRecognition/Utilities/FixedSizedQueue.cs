using System.Collections.Concurrent;

namespace SoundRecognition
{
    internal class FixedSizedQueue<T> : ConcurrentQueue<T>
    {
        public int MaximalSize { get; set; }

        public FixedSizedQueue(int size)
        {
            MaximalSize = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            if (base.Count > MaximalSize)
            {
                T outObj;
                base.TryDequeue(out outObj);
            }
        }
    }
}
