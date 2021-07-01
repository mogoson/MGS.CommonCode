﻿/*************************************************************************
 *  Copyright © 2020 Mogoson. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  CompressManager.cs
 *  Description  :  Compress manager.
 *------------------------------------------------------------------------
 *  Author       :  Mogoson
 *  Version      :  1.0
 *  Date         :  5/30/2020
 *  Description  :  Initial development version.
 *************************************************************************/

using MGS.Common.Generic;
using MGS.DesignPattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace MGS.Compress
{
    /// <summary>
    /// Compress manager.
    /// </summary>
    public sealed class CompressManager : SingleTimer<CompressManager>, ICompressManager
    {
        #region Field and Property
        /// <summary>
        /// Compressor for manager.
        /// </summary>
        public ICompressor Compressor { set; get; }

        /// <summary>
        /// Max run count of async operate.
        /// </summary>
        public int MaxRunCount { set; get; }

        /// <summary>
        /// List to cache tasks.
        /// [Usually not too many tasks so do not use Dictionary to cache tasks]
        /// </summary>
        private List<ITask> tasks = new List<ITask>();

        /// <summary>
        /// Locker for task cache list.
        /// </summary>
        private readonly object locker = new object();
        #endregion

        #region Private Method
        /// <summary>
        /// Constructor.
        /// </summary>
        private CompressManager()
        {
#if USE_IONIC_ZIP
            Compressor = new IonicCompressor();
#elif USE_SHARPCOMPRESS
            Compressor = new SharpCompressor();
#endif
            MaxRunCount = 10;
        }
        #endregion

        #region Protected Method
        /// <summary>
        /// Timer tick.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        protected override void Tick(object sender, ElapsedEventArgs e)
        {
            if (tasks.Count == 0)
            {
                return;
            }

            lock (locker)
            {
                var runner = 0;
                for (int i = 0; i < tasks.Count; i++)
                {
                    var task = tasks[i];
                    switch (task.State)
                    {
                        case TaskState.Idle:
                            if (runner < MaxRunCount)
                            {
                                task.Start();
                            }
                            break;

                        case TaskState.Working:
                            runner++;
                            break;

                        case TaskState.Finished:
                            tasks.Remove(task);
                            i--;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Add task to cache list.
        /// </summary>
        /// <param name="task"></param>
        private void AddTask(ITask task)
        {
            lock (locker)
            {
                tasks.Add(task);
            }
        }

        /// <summary>
        /// Check compressor is valid?
        /// </summary>
        /// <param name="compressor"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool CheckCompressor(ICompressor compressor, out Exception error)
        {
            error = null;
            if (compressor == null)
            {
                error = new NullReferenceException("The compressor for manager does not set an instance.");
                return false;
            }

            return true;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Compress entrie[files or directories] to dest file async.
        /// </summary>
        /// <param name="entries">Target entrie[files or directories].</param>
        /// <param name="destFile">The dest file.</param>
        /// <param name="encoding">Encoding for zip file.</param>
        /// <param name="directoryPathInArchive">Directory path in archive of zip file.</param>
        /// <param name="clearBefor">Clear origin file(if exists) befor compress.</param>
        /// <param name="progressCallback">Progress callback.</param>
        /// <param name="finishedCallback">Finished callback.</param>
        /// <returns>Guid of async operate.</returns>
        public string CompressAsync(IEnumerable<string> entries, string destFile,
            Encoding encoding, string directoryPathInArchive = null, bool clearBefor = true,
            Action<float> progressCallback = null, Action<bool, object> finishedCallback = null)
        {
            if (entries == null)
            {
                var error = new ArgumentNullException("entries", "The params is invalid.");
                DelegateUtility.Invoke(finishedCallback, false, error);
                return null;
            }

            if (string.IsNullOrEmpty(destFile))
            {
                var error = new ArgumentNullException("destFile", "The params is invalid.");
                DelegateUtility.Invoke(finishedCallback, false, error);
                return null;
            }

            if (!CheckCompressor(Compressor, out Exception ex))
            {
                DelegateUtility.Invoke(finishedCallback, false, ex);
                return null;
            }

            var task = new AsyncCompressTask(Compressor, entries, destFile, encoding, directoryPathInArchive, clearBefor,
                progress =>
                {
                    DelegateUtility.Invoke(progressCallback, progress);
                },
                (isSucceed, info) =>
                {
                    DelegateUtility.Invoke(finishedCallback, isSucceed, info);
                });

            AddTask(task);
            return task.GUID;
        }

        /// <summary>
        /// Decompress file to dest dir async.
        /// </summary>
        /// <param name="filePath">Target file.</param>
        /// <param name="destDir">The dest decompress directory.</param>
        /// <param name="clearBefor">Clear the dest dir before decompress.</param>
        /// <param name="progressCallback">Progress callback.</param>
        /// <param name="finishedCallback">Finished callback.</param>
        /// <returns>Guid of async operate.</returns>
        public string DecompressAsync(string filePath, string destDir, bool clearBefor = false,
            Action<float> progressCallback = null, Action<bool, object> finishedCallback = null)
        {
            if (!File.Exists(filePath))
            {
                var error = new FileNotFoundException("Can not find the file.", filePath);
                DelegateUtility.Invoke(finishedCallback, false, error);
                return null;
            }

            if (string.IsNullOrEmpty(destDir))
            {
                var error = new ArgumentNullException("destDir", "The params is invalid.");
                DelegateUtility.Invoke(finishedCallback, false, error);
                return null;
            }

            if (!CheckCompressor(Compressor, out Exception ex))
            {
                DelegateUtility.Invoke(finishedCallback, false, ex);
                return null;
            }

            var task = new AsyncDecompressTask(Compressor, filePath, destDir, clearBefor,
                 progress =>
                 {
                     DelegateUtility.Invoke(progressCallback, progress);
                 },
                (isSucceed, info) =>
                {
                    DelegateUtility.Invoke(finishedCallback, isSucceed, info);
                });

            AddTask(task);
            return task.GUID;
        }

        /// <summary>
        /// Abort async operate.
        /// </summary>
        /// <param name="guid">Guid of async operate.</param>
        public void AbortAsync(string guid)
        {
            if (tasks.Count == 0)
            {
                return;
            }

            lock (locker)
            {
                foreach (var task in tasks)
                {
                    if (task.GUID == guid)
                    {
                        task.Abort();
                        return;
                    }
                }
            }
        }
        #endregion
    }
}