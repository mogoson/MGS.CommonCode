﻿/*************************************************************************
 *  Copyright © 2016-2021 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  MathSmoother.cs
 *  Description  :  Math smoother base mathematical methods.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  6/21/2016
 *  Description  :  Initial development version.
 *************************************************************************/

namespace MGS.Mathematics
{
    /// <summary>
    /// Math smoother base mathematical methods.
    /// </summary>
    public sealed class MathSmoother
    {
        #region Linear
        /// <summary>
        /// Three point linear smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] ThreePointLinear(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 3)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (5 * source[0] + 2 * source[1] - source[2]) / 6;
                for (int i = 1; i < n - 1; i++)
                {
                    result[i] = (source[i - 1] + source[i] + source[i + 1]) / 3;
                }
                result[n - 1] = (5 * source[n - 1] + 2 * source[n - 2] - source[n - 3]) / 6;
            }
            return result;
        }

        /// <summary>
        /// Five point linear smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] FivePointLinear(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (3 * source[0] + 2 * source[1] + source[2] - source[4]) / 5;
                result[1] = (4 * source[0] + 3 * source[1] + 2 * source[2] + source[3]) / 10;
                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (source[i - 2] + source[i - 1] + source[i] + source[i + 1] + source[i + 2]) / 5;
                }
                result[n - 2] = (4 * source[n - 1] + 3 * source[n - 2] + 2 * source[n - 3] + source[n - 4]) / 10;
                result[n - 1] = (3 * source[n - 1] + 2 * source[n - 2] + source[n - 3] - source[n - 5]) / 5;
            }
            return result;
        }

        /// <summary>
        /// Seven point linear smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] SevenPointLinear(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (13 * source[0] + 10 * source[1] + 7 * source[2] + 4 * source[3] +
                    source[4] - 2 * source[5] - 5 * source[6]) / 28;

                result[1] = (5 * source[0] + 4 * source[1] + 3 * source[2] + 2 * source[3] +
                    source[4] - source[6]) / 14;

                result[2] = (7 * source[0] + 6 * source[1] + 5 * source[2] + 4 * source[3] +
                    3 * source[4] + 2 * source[5] + source[6]) / 28;

                for (int i = 3; i < n - 3; i++)
                {
                    result[i] = (source[i - 3] + source[i - 2] + source[i - 1] + source[i] +
                        source[i + 1] + source[i + 2] + source[i + 3]) / 7;
                }

                result[n - 3] = (7 * source[n - 1] + 6 * source[n - 2] + 5 * source[n - 3] +
                    4 * source[n - 4] + 3 * source[n - 5] + 2 * source[n - 6] + source[n - 7]) / 28;

                result[n - 2] = (5 * source[n - 1] + 4 * source[n - 2] + 3 * source[n - 3] +
                    2 * source[n - 4] + source[n - 5] - source[n - 7]) / 14;

                result[n - 1] = (13 * source[n - 1] + 10 * source[n - 2] + 7 * source[n - 3] +
                    4 * source[n - 4] + source[n - 5] - 2 * source[n - 6] - 5 * source[n - 7]) / 28;
            }
            return result;
        }
        #endregion

        #region Quadratic
        /// <summary>
        /// Five point quadratic smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] FivePointQuadratic(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (31 * source[0] + 9 * source[1] - 3 * source[2] - 5 * source[3] + 3 * source[4]) / 35;
                result[1] = (9 * source[0] + 13 * source[1] + 12 * source[2] + 6 * source[3] - 5 * source[4]) / 35;
                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (-3 * (source[i - 2] + source[i + 2]) +
                              12 * (source[i - 1] + source[i + 1]) + 17 * source[i]) / 35;
                }
                result[n - 2] = (9 * source[n - 1] + 13 * source[n - 2] + 12 * source[n - 3] + 6 * source[n - 4] - 5 * source[n - 5]) / 35;
                result[n - 1] = (31 * source[n - 1] + 9 * source[n - 2] - 3 * source[n - 3] - 5 * source[n - 4] + 3 * source[n - 5]) / 35;
            }
            return result;
        }

        /// <summary>
        /// Seven point quadratic smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] SevenPointQuadratic(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (32 * source[0] + 15 * source[1] + 3 * source[2] - 4 * source[3] -
                    6 * source[4] - 3 * source[5] + 5 * source[6]) / 42;

                result[1] = (5 * source[0] + 4 * source[1] + 3 * source[2] + 2 * source[3] +
                    source[4] - source[6]) / 14;

                result[2] = (source[0] + 3 * source[1] + 4 * source[2] + 4 * source[3] +
                    3 * source[4] + source[5] - 2 * source[6]) / 14;

                for (int i = 3; i < n - 3; i++)
                {
                    result[i] = (-2 * (source[i - 3] + source[i + 3]) + 3 * (source[i - 2] + source[i + 2]) +
                        6 * (source[i - 1] + source[i + 1]) + 7 * source[i]) / 21;
                }

                result[n - 3] = (source[n - 1] + 3 * source[n - 2] + 4 * source[n - 3] + 4 * source[n - 4] +
                    3 * source[n - 5] + source[n - 6] - 2 * source[n - 7]) / 14;

                result[n - 2] = (5 * source[n - 1] + 4 * source[n - 2] + 3 * source[n - 3] + 2 * source[n - 4] +
                    source[n - 5] - source[n - 7]) / 14;

                result[n - 1] = (32 * source[n - 1] + 15 * source[n - 2] + 3 * source[n - 3] - 4 * source[n - 4] -
                    6 * source[n - 5] - 3 * source[n - 6] + 5 * source[n - 7]) / 42;
            }
            return result;
        }
        #endregion

        #region Cubic
        /// <summary>
        /// Five point cubic smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] FivePointCubic(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 5)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (69 * source[0] + 4 * source[1] - 6 * source[2] +
                    4 * source[3] - source[4]) / 70;

                result[1] = (2 * source[0] + 27 * source[1] + 12 * source[2] -
                    8 * source[3] + 2 * source[4]) / 35;

                for (int i = 2; i < n - 2; i++)
                {
                    result[i] = (-3 * (source[i - 2] + source[i + 2]) + 12 * (source[i - 1] +
                        source[i + 1]) + 17 * source[i]) / 35;
                }

                result[n - 2] = (2 * source[n - 5] - 8 * source[n - 4] + 12 * source[n - 3] +
                    27 * source[n - 2] + 2 * source[n - 1]) / 35;

                result[n - 1] = (-source[n - 5] + 4 * source[n - 4] - 6 * source[n - 3] +
                    4 * source[n - 2] + 69 * source[n - 1]) / 70;
            }
            return result;
        }

        /// <summary>
        /// Seven point cubic smooth.
        /// </summary>
        /// <param name="source">Data source.</param>
        /// <returns>Smooth result.</returns>
        public static double[] SevenPointCubic(double[] source)
        {
            var n = source.Length;
            var result = new double[n];
            if (n < 7)
            {
                for (int i = 0; i < n; i++)
                {
                    result[i] = source[i];
                }
            }
            else
            {
                result[0] = (39 * source[0] + 8 * source[1] - 4 * source[2] - 4 * source[3] +
                    source[4] + 4 * source[5] - 2 * source[6]) / 42;

                result[1] = (8 * source[0] + 19 * source[1] + 16 * source[2] + 6 * source[3] -
                    4 * source[4] - 7 * source[5] + 4 * source[6]) / 42;

                result[2] = (-4 * source[0] + 16 * source[1] + 19 * source[2] + 12 * source[3] +
                    2 * source[4] - 4 * source[5] + source[6]) / 42;

                for (int i = 3; i <= n - 4; i++)
                {
                    result[i] = (-2 * (source[i - 3] + source[i + 3]) + 3 * (source[i - 2] +
                        source[i + 2]) + 6 * (source[i - 1] + source[i + 1]) + 7 * source[i]) / 21;
                }

                result[n - 3] = (-4 * source[n - 1] + 16 * source[n - 2] + 19 * source[n - 3] +
                    12 * source[n - 4] + 2 * source[n - 5] - 4 * source[n - 6] + source[n - 7]) / 42;

                result[n - 2] = (8 * source[n - 1] + 19 * source[n - 2] + 16 * source[n - 3] +
                    6 * source[n - 4] - 4 * source[n - 5] - 7 * source[n - 6] + 4 * source[n - 7]) / 42;

                result[n - 1] = (39 * source[n - 1] + 8 * source[n - 2] - 4 * source[n - 3] -
                    4 * source[n - 4] + source[n - 5] + 4 * source[n - 6] - 2 * source[n - 7]) / 42;
            }
            return result;
        }
        #endregion
    }
}