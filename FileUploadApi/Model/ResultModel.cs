using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Model
{
    public class ResultModel
    {
        public ResultModel()
        {
            DashboardPostDatas = new List<DashboardPostData>();
        }
        public string CategoryName { get; set; }
        public IList<DashboardPostData> DashboardPostDatas { get; set; }
    }
    public class DashboardPostData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
