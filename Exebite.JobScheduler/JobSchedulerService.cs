﻿using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Exebite.JobScheduler
{
    public class JobSchedulerService
    {
        private IScheduler _scheduler;
        private ISchedulerFactory _schedulerFactory = new StdSchedulerFactory();

        public JobSchedulerService()
        {
            _scheduler = _schedulerFactory.GetScheduler().Result;
        }

        public void Start()
        {
            Task.Run(() => _scheduler.Start());
        }

        public void Stop()
        {
            _scheduler.Shutdown();
        }
    }
}
