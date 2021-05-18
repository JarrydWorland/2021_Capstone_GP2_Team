using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
	public class CircularQueue<T>
	{
		private readonly List<T> _list = new List<T>();

		public T Peek() => _list.FirstOrDefault();

		public void Enqueue(T item) => _list.Add(item);

		public T Dequeue()
		{
			T item = Peek();
			if (item != null) _list.RemoveAt(0);

			return item;
		}

		public void Requeue(bool reverse = false)
		{
			if (reverse)
			{
				T item = _list.Last();
				_list.RemoveAt(Count - 1);
				_list.Insert(0, item);
			}
			else Enqueue(Dequeue());
		}

		public int Count => _list.Count;
		public bool Contains(T other) => _list.Contains(other);
		public bool Remove(T item) => _list.Remove(item);
	}
}