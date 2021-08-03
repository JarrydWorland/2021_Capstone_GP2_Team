using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utilities
{
	public class CircularQueue<T>
	{
		/// <summary>
		/// The underlying list object.
		/// </summary>
		private readonly List<T> _list = new List<T>();

		/// <summary>
		/// Get the item at the front of the queue.
		/// </summary>
		/// <returns>Returns the item if at least one exists, otherwise returns default.</returns>
		public T Peek() => _list.FirstOrDefault();

		/// <summary>
		/// Add an item to the back of the queue.
		/// </summary>
		/// <param name="item">The item to be added to the queue.</param>
		public void Enqueue(T item) => _list.Add(item);

		/// <summary>
		/// Get the first item in the queue and remove it from the queue.
		/// </summary>
		/// <returns>Returns the item if at least one exists, otherwise returns default.</returns>
		public T Dequeue()
		{
			T item = Peek();
			if (item != null) _list.RemoveAt(0);

			return item;
		}

		/// <summary>
		/// Takes an item out of the queue and places it at the opposite end.
		/// </summary>
		/// <param name="reverse">The boolean to indicate the direction of the requeue.</param>
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

		/// <summary>
		/// The number of items in the queue.
		/// </summary>
		public int Count => _list.Count;

		/// <summary>
		/// Checks if the given item is in the queue.
		/// </summary>
		/// <param name="other">The item to be checked.</param>
		/// <returns>Returns true if the item is contained in the queue, otherwise returns false.</returns>
		public bool Contains(T other) => _list.Contains(other);

		/// <summary>
		/// Remove an item from the queue, bypassing the queue order.
		/// </summary>
		/// <param name="item">The item to be removed from the queue.</param>
		/// <returns>Returns true if the item was successfully removed, otherwise returns false.</returns>
		public bool Remove(T item) => _list.Remove(item);
	}
}