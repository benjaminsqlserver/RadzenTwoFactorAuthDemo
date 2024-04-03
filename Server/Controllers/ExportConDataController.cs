using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using HouseholdAppliancesApp.Server.Data;

namespace HouseholdAppliancesApp.Server.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/householdappliances/csv")]
        [HttpGet("/export/ConData/householdappliances/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHouseholdAppliancesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetHouseholdAppliances(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/householdappliances/excel")]
        [HttpGet("/export/ConData/householdappliances/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHouseholdAppliancesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetHouseholdAppliances(), Request.Query, false), fileName);
        }
    }
}
