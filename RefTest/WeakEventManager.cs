﻿using System;

/// <summary>
/// Static Class that holds the extension methods to handle events using weak references.
/// This way we do not need to worry about unregistered the event handler.
/// </summary>
public static class WeakEventManager
{
    /// <summary>
    /// This overload handles any type of EventHandler
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    /// <typeparam name="TDelegate">The type of the T delegate.</typeparam>
    /// <typeparam name="TArgs">The type of the T args.</typeparam>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="converter">The converter.</param>
    /// <param name="add">The add.</param>
    /// <param name="remove">The remove.</param>
    /// <param name="action">The action.</param>
    public static void SetAnyHandler<T, TDelegate, TArgs>(this T subscriber,
        Func<EventHandler<TArgs>, TDelegate> converter,
        Action<TDelegate> add, Action<TDelegate> remove,
        Action<T, TArgs> action)
        where TArgs : EventArgs
        where TDelegate : class
        where T : class
    {
        var subsWeakRef = new WeakReference(subscriber);
        TDelegate handler = null;
        handler = converter(new EventHandler<TArgs>(
            (s, e) =>
            {
                var subsStrongRef = subsWeakRef.Target as T;
                if (subsStrongRef != null)
                {
                    action(subsStrongRef, e);
                }
                else
                {
                    remove(handler);
                    handler = null;
                }
            }));
        add(handler);
    }
    /// <summary>
    /// this overload is simplified for generic EventHandlers
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    /// <typeparam name="TArgs">The type of the T args.</typeparam>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="add">The add.</param>
    /// <param name="remove">The remove.</param>
    /// <param name="action">The action.</param>
    public static void SetAnyHandler<T, TArgs>(this T subscriber,
        Action<EventHandler<TArgs>> add, Action<EventHandler<TArgs>> remove,
        Action<T, TArgs> action)
        where TArgs : EventArgs
        where T : class
    {
        SetAnyHandler<T, EventHandler<TArgs>, TArgs>(subscriber,
            h => h,
            add,
            remove,
            action);
    }
    /// <summary>
    /// this overload is simplified for EventHandlers.
    /// </summary>
    /// <typeparam name="T">The type of the T.</typeparam>
    /// <param name="subscriber">The subscriber.</param>
    /// <param name="add">The add.</param>
    /// <param name="remove">The remove.</param>
    /// <param name="action">The action.</param>
    public static void SetAnyHandler<T>(this T subscriber,
        Action<EventHandler> add, Action<EventHandler> remove,
        Action<T, EventArgs> action)
        where T : class
    {
        SetAnyHandler<T, EventHandler, EventArgs>(subscriber,
            h => (o, e) => h(o, e), //This is a workaround from Rx
            add,
            remove,
            action);
    }
}