﻿/*************************************************************************
 *  Copyright (C) 2018-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MonoAnimation.cs
 *  Description  :  Define mono animation.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  6/23/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;

namespace MGS.UAnimation
{
    /// <summary>
    /// Mono animation.
    /// </summary>
    public abstract class MonoAnimation : MonoBehaviour, IAnimation
    {
        /// <summary>
        /// Speed of animation.
        /// </summary>
        [SerializeField]
        protected float speed = 5;

        /// <summary>
        /// Loop mode of animation.
        /// </summary>
        [SerializeField]
        protected LoopMode loopMode = LoopMode.Once;

        /// <summary>
        /// Speed of animation.
        /// </summary>
        public virtual float Speed
        {
            set { speed = value; }
            get { return speed; }
        }

        /// <summary>
        /// Loop mode of animation.
        /// </summary>
        public virtual LoopMode LoopMode
        {
            set { loopMode = value; }
            get { return loopMode; }
        }

        /// <summary>
        /// Animation is playing?
        /// </summary>
        public virtual bool IsPlaying
        {
            protected set;
            get;
        }

        /// <summary>
        /// Play animation.
        /// </summary>
        public virtual void Play()
        {
            enabled = IsPlaying = true;
        }

        /// <summary>
        /// Pause animation.
        /// </summary>
        public virtual void Pause()
        {
            enabled = IsPlaying = false;
        }

        /// <summary>
        /// Rewind animation.
        /// </summary>
        /// <param name="progress">Progress of animation in the range[0~1]</param>
        public abstract void Rewind(float progress = 0);

        /// <summary>
        /// Stop animation.
        /// </summary>
        public virtual void Stop()
        {
            enabled = IsPlaying = false;
            Rewind(0);
        }
    }
}