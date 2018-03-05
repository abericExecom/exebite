﻿using Exebite.Business.GoogleApiImportExport;
using Exebite.JobScheduler.Unity;
using Quartz;
using System.Threading.Tasks;
using Unity;

namespace Exebite.JobScheduler.Jobs
{
    public class UpdateDailyMenus : IJob
    {
        IGoogleApiOldSheets _oldSheets;
        

        public UpdateDailyMenus()
        {
            _oldSheets = UnityConfig.Container.Resolve<IGoogleApiOldSheets>();
        }
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => _oldSheets.UpdateDailyMenu());
            return task;
        }
    }
}

