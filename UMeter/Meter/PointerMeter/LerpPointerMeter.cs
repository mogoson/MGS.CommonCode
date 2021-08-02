﻿/*************************************************************************
 *  Copyright © 2016-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  LerpPointerMeter.cs
 *  Description  :  Define lerp Meter.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  3/9/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using System;
using UnityEngine;

namespace MGS.Meter
{
    /// <summary>
    /// Meter with lerp rotate pointers.
    /// </summary>
    public class LerpPointerMeter : PointerMeter
    {
        #region Field and Property
        /// <summary>
        /// Meter lerp mode.
        /// </summary>
        [HideInInspector]
        public LerpMode lerpMode;

        /// <summary>
        /// Speed of main pointer.
        /// </summary>
        [HideInInspector]
        public float mainSpeed = 120;

        /// <summary>
        /// Min speed of main pointer.
        /// </summary>
        [HideInInspector]
        public float minSpeed = 30;

        /// <summary>
        /// Current lerp value of main angle.
        /// </summary>
        public float LerpAngle { protected set; get; }

        /// <summary>
        /// Event on start lerp.
        /// </summary>
        public event Action OnLerpStart;

        /// <summary>
        /// Event on stay lerp.
        /// </summary>
        public event Action OnLerpStay;

        /// <summary>
        /// Event on exit lerp.
        /// </summary>
        public event Action OnLerpExit;
        #endregion

        #region Protected Method
        /// <summary>
        /// On main pointer's angle changed.
        /// </summary>
        /// <param name="mainAngle">Main pointer's angle.</param>
        protected override void OnMainAngleChanged(float mainAngle)
        {
            CheckLerp(mainAngle);
            if (enabled)
            {
                InvokeOnLerpStartEvent();
            }
        }

        /// <summary>
        /// Drive pointers to target angle.
        /// </summary>
        protected virtual void Update()
        {
            if (lerpMode == LerpMode.Lerp)
            {
                var lastLerp = LerpAngle;
                LerpAngle = Mathf.Lerp(LerpAngle, mainPointerAngle, mainSpeed / 180 * Time.deltaTime);

                var lerpSpeed = Mathf.Abs((LerpAngle - lastLerp) / Time.deltaTime);
                if (lerpSpeed < minSpeed)
                {
                    LerpAngle = Mathf.MoveTowards(lastLerp, mainPointerAngle, minSpeed * Time.deltaTime);
                }
            }
            else
            {
                LerpAngle = Mathf.MoveTowards(LerpAngle, mainPointerAngle, mainSpeed * Time.deltaTime);
            }

            SetPointersAngle(LerpAngle);
            CheckLerp(mainPointerAngle);
            InvokeOnLerpStayEvent();

            if (!enabled)
            {
                InvokeOnLerpExitEvent();
            }
        }

        /// <summary>
        /// Check need lerp angle.
        /// </summary>
        /// <param name="mainAngle">Main pointer's angle.</param>
        protected void CheckLerp(float mainAngle)
        {
            enabled = mainAngle - LerpAngle != 0;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnLerpStartEvent()
        {
            OnLerpStart?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnLerpStayEvent()
        {
            OnLerpStay?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void InvokeOnLerpExitEvent()
        {
            OnLerpExit?.Invoke();
        }
        #endregion
    }
}