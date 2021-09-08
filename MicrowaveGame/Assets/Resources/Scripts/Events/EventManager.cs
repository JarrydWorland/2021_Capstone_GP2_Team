using System;
using System.Collections.Generic;
using Scripts.Utilities;
using System.Linq;

namespace Scripts.Events
{
	public static class EventManager
	{
		private static readonly Dictionary<Type, object> Handlers = new Dictionary<Type, object>();
		private static ulong _counter;

		/// <summary>
		/// Registers a handler for calling later via Emit().
		/// </summary>
		/// <param name="handler">The handler to be registered.</param>
		/// <typeparam name="T">The event type of the handler.</typeparam>
		/// <returns>The event ID of the new handler.</returns>
		public static EventId<T> Register<T>(Action<T> handler) where T : EventArgs
		{
			List<Handler<T>> handlers = GetHandlersFromType<T>();

			EventId<T> eventId = _counter++;

			handlers.Add(new Handler<T>
			{
				EventId = eventId,
				Action = handler
			});

			Log.Info($"{Log.Green(GetCallerString())} registered {Log.Cyan(handler.Method.Name)} for " + Log.Yellow($"<{typeof(T).Name}> "), LogCategory.EventManager);

			return eventId;
		}

		/// <summary>
		/// Unregisters a handler.
		/// </summary>
		/// <param name="eventId">The event ID of the handler.</param>
		/// <typeparam name="T">The event type of the handler.</typeparam>
		public static void Unregister<T>(EventId<T> eventId) where T : EventArgs
		{
			List<Handler<T>> handlers = GetHandlersFromType<T>();
			int index = handlers.FindIndex(x => x.EventId == eventId);
			if (index > -1)
			{
				Log.Info($"{Log.Green(GetCallerString())} unregistered {Log.Cyan(handlers[index].Action.Method.Name)} from " + Log.Yellow($"<{typeof(T).Name}>"), LogCategory.EventManager);
				handlers.RemoveAt(index);
			}
		}

		/// <summary>
		/// Run all handlers listening for the given event type.
		/// </summary>
		/// <param name="eventArgs">The event args that will be parsed to the handlers.</param>
		/// <typeparam name="T">The event type of the handlers.</typeparam>
		public static void Emit<T>(T eventArgs) where T : EventArgs
		{
			List<Handler<T>> handlers = GetHandlersFromType<T>();
			Log.Info($"{Log.Green(GetCallerString())} emitting " + Log.Yellow($"<{typeof(T).Name}>") + $" event to {Log.Cyan(handlers.Count)} handlers.", LogCategory.EventManager);
			foreach (Handler<T> handler in handlers.ToArray()) handler.Action.Invoke(eventArgs);
		}

		/// <summary>
		/// Gets the handlers from a given event type.
		/// </summary>
		/// <typeparam name="T">The event type of the handlers.</typeparam>
		/// <returns>A list of handlers from the given event type.</returns>
		private static List<Handler<T>> GetHandlersFromType<T>() where T : EventArgs
		{
			Type eventType = typeof(T);

			// Note: Be aware that casting occurs here. You've been warned!
			if (!Handlers.ContainsKey(eventType)) Handlers.Add(eventType, (object) new List<Handler<T>>());
			return (List<Handler<T>>) Handlers[eventType];
		}

		private struct Handler<T> where T : EventArgs
		{
			public EventId<T> EventId;
			public Action<T> Action;
		}

		private static string GetCallerString()
		{
			var stackTrace = new System.Diagnostics.StackTrace(true).GetFrame(2);
			string callerString = $"{stackTrace.GetFileName().Split('\\').Last()}:{stackTrace.GetFileLineNumber()}";
			return callerString;
		}
	}
}
