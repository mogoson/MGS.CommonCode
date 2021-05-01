﻿/*************************************************************************
 *  Copyright © 2018-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  ObjectPool.cs
 *  Description  :  Define ObjectPool.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  2/9/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Logger;
using System;
using System.Collections.Generic;

namespace MGS.DesignPattern
{
    /// <summary>
    /// Generic object pool.
    /// </summary>
    /// <typeparam name="T">Specified type of object.</typeparam>
    public class ObjectPool<T>
    {
        #region Field and Property
        /// <summary>
        /// Max count limit of objects in pool.
        /// </summary>
        public int MaxCount { set; get; }

        /// <summary>
        /// Current count of objects in pool.
        /// </summary>
        public int CurrentCount { get { return objectStack.Count; } }

        /// <summary>
        /// Stack store objects.
        /// </summary>
        protected Stack<T> objectStack = new Stack<T>();

        /// <summary>
        /// Function of create new object.
        /// </summary>
        protected Func<T> createFunc;

        /// <summary>
        /// Action of reset object to default.
        /// </summary>
        protected Action<T> resetAction;

        /// <summary>
        /// Action of dispose object.
        /// </summary>
        protected Action<T> disposeAction;
        #endregion

        #region Public Method
        /// <summary>
        /// Constructor of ObjectPool.
        /// </summary>
        /// <param name="create">Function of create new object.</param>
        /// <param name="reset">Action of reset object to default.</param>
        /// <param name="dispose">Action of dispose object.</param>
        /// <param name="maxCount">Max count limit of objects in pool.</param>
        public ObjectPool(Func<T> create, Action<T> reset, Action<T> dispose, int maxCount = 100)
        {
            createFunc = create;
            resetAction = reset;
            disposeAction = dispose;
            MaxCount = maxCount;
        }

        /// <summary>
        /// Take a object from pool.
        /// </summary>
        /// <returns>A object.</returns>
        public virtual T Take()
        {
            if (objectStack.Count > 0)
            {
                return objectStack.Pop();
            }
            if (createFunc == null)
            {
                return default(T);
            }
            return createFunc.Invoke();
        }

        /// <summary>
        /// Recycle object to pool.
        /// </summary>
        /// <param name="obj">Object to recycle.</param>
        public virtual void Recycle(T obj)
        {
            //Null object do not need to recycle.
            if (obj == null)
            {
                LogUtility.LogWarning("Recycle object to pool cancelled: Null object do not need to recycle.");
                return;
            }

            //Avoid repeated recycle. 
            if (objectStack.Contains(obj))
            {
                return;
            }

            if (objectStack.Count < MaxCount)
            {
                resetAction?.Invoke(obj);
                objectStack.Push(obj);
            }
            else
            {
                disposeAction?.Invoke(obj);
            }
        }

        /// <summary>
        /// Clear all objects.
        /// </summary>
        public virtual void Clear()
        {
            if (disposeAction != null)
            {
                foreach (var obj in objectStack)
                {
                    disposeAction.Invoke(obj);
                }
            }
            objectStack.Clear();
        }
        #endregion
    }
}