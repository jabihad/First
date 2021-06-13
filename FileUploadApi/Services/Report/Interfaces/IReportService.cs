using FileUploadApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Services.Report.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportModel>> Generate(string email);
    }
}
