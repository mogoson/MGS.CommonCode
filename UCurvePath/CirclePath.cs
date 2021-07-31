﻿/*************************************************************************
 *  Copyright © 2018-2019 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CirclePath.cs
 *  Description  :  Define path base on circle curve.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  7/17/2018
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.UCurve;
using UnityEngine;

namespace MGS.CurvePath
{
    /// <summary>
    /// Path base on circle curve.
    /// </summary>
    [AddComponentMenu("MGS/CurvePath/CirclePath")]
    public class CirclePath : MonoCurvePath
    {
        #region Field and Property
        /// <summary>
        /// Radius of circle curve.
        /// </summary>
        [Tooltip("Radius of circle curve.")]
        public float radius = 1.0f;

        /// <summary>
        /// Curve for path.
        /// </summary>
        protected override ICurve Curve { get { return curve; } }

        /// <summary>
        /// Curve of path.
        /// </summary>
        protected EllipseCurve curve = new EllipseCurve();
        #endregion

        #region Public Method
        /// <summary>
        /// Rebuild path.
        /// </summary>
        public override void Rebuild()
        {
            curve.args.semiMinorAxis = radius;
            curve.args.semiMajorAxis = radius;
            base.Rebuild();
        }
        #endregion
    }
}