﻿using Exebite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exebite.Business.GoogleApiImportExport
{
    public interface IGoogleDataImporter
    {
        void UpdateRestorauntsMenu();
        void ImportUsersFromKasa();
    }
}