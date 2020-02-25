using Krypton.Support;
using System;
using System.Collections.Generic;
using Amib.Threading;
using System.Threading;

namespace Krypton.MinimalLoader.Core.Threading
{
    public class ThreadPoolComponent : KryptonComponent<ThreadPoolComponent>
    {
        private SmartThreadPool ThreadPool { get; set; }
        private List<IWaitableResult> WaitableResults { get; set; }

        public ThreadPoolComponent()
        {
            ThreadPool = new SmartThreadPool();
            WaitableResults = new List<IWaitableResult>();
        }

        public IWorkItemResult Add<T>(Action<T> act, T arg, WorkItemPriority priority = WorkItemPriority.Normal)
        {
            var work_item = GetPool().QueueWorkItem(act, arg, priority);
            WaitableResults.Add(work_item);

            return work_item;
        }
        public IWorkItemResult Add<T1, T2>(Action<T1, T2> act, T1 arg1, T2 arg2, WorkItemPriority priority = WorkItemPriority.Normal)
        {
            var work_item = GetPool().QueueWorkItem(act, arg1, arg2, priority);
            WaitableResults.Add(work_item);

            return work_item;
        }
        public IWorkItemResult Add<T1, T2, T3>(Action<T1, T2, T3> act, T1 arg1, T2 arg2, T3 arg3, WorkItemPriority priority = WorkItemPriority.Normal)
        {
            var work_item = GetPool().QueueWorkItem(act, arg1, arg2, arg3, priority);
            WaitableResults.Add(work_item);

            return work_item;
        }
        public IWorkItemResult Add<T1, T2, T3, T4>(Action<T1, T2, T3, T4> act, T1 arg1, T2 arg2, T3 arg3, T4 arg4, WorkItemPriority priority = WorkItemPriority.Normal)
        {
            var work_item = GetPool().QueueWorkItem(act, arg1, arg2, arg3, arg4, priority);
            WaitableResults.Add(work_item);

            return work_item;
        }

        public IWaitableResult[] WaitAll(IWaitableResult[] threads = null)
        {
            if (threads == null)
            {
                threads = WaitableResults.ToArray();
            }

            var is_success = SmartThreadPool.WaitAll(threads);
            if (is_success)
            {
                return threads;
            }

            return null;
        }

        public IWaitableResult WaitAny(IWaitableResult[] evaluate_results, int timeout)
        {
            var index = SmartThreadPool.WaitAny(evaluate_results);
            if(index != WaitHandle.WaitTimeout)
            {
                return evaluate_results[index].GetWorkItemResult();
            }

            return null;
        }

        public void Stop()
        {
            if (!GetPool().IsShuttingdown)
            {
                GetPool().Shutdown();
            }
        }

        public SmartThreadPool GetPool()
            => ThreadPool;
    }
}
