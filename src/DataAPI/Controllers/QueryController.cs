using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;

namespace DataAPI.Controllers
{
    [Authorize]
    public class QueryController : ApiController
    {
        public System.Data.DataTable Post(Models.QueryParam p)
        {
            // now, get result from sql text
            if (p.SqlText == "sysinfo")
                return ServerRuntimeInfoResult();
            // or do some update then return rows
            else if (p.SqlText == "execute blablabla")
                ExecuteNonQueryResult(1);

            // or get different results for different database
            if (p.Database == "dbName")
                return ServerRuntimeInfoResult();

            // or even get different results for different users
            if (p.Username == "testuser")
                return ClientInfoResult();

            // DO NOT return null EVER! the server may consider it's error
            return EmptyResult();

            // or
            // TODO: define your 'SqlText' process logic then return System.Data.DataTable
            // get your data from any sources like text/docx/xslx/pdf/cvs files or sqlserver/mysql or anything else
        }


        System.Data.DataTable ServerRuntimeInfoResult()
        {
            var ret = new System.Data.DataTable();

            ret.Columns.Add("FrameworkDescription", typeof(string));
            ret.Columns.Add("OSDescription", typeof(string));
            ret.Columns.Add("OSArchitecture", typeof(string));            
            ret.Columns.Add("ProcessArchitecture", typeof(string));
            ret.Columns.Add("IsWindows", typeof(bool));
            ret.Columns.Add("IsLinux", typeof(bool));
            ret.Columns.Add("IsOSX", typeof(bool));
            ret.Columns.Add("OSVersion", typeof(string));
            ret.Columns.Add("OSx64", typeof(bool));
            ret.Columns.Add("ProcessorCount", typeof(int));
            ret.Columns.Add("MachineName", typeof(string));
            ret.Columns.Add("WorkingSet", typeof(long));
            ret.Columns.Add("SystemPageSize", typeof(int));

            ret.Rows.Add(RuntimeInformation.FrameworkDescription,
                         RuntimeInformation.OSDescription,
                         RuntimeInformation.OSArchitecture,                         
                         RuntimeInformation.ProcessArchitecture,
                         RuntimeInformation.IsOSPlatform(OSPlatform.Windows),
                         RuntimeInformation.IsOSPlatform(OSPlatform.Linux),
                         RuntimeInformation.IsOSPlatform(OSPlatform.OSX),
                         Environment.OSVersion.VersionString,
                         Environment.Is64BitOperatingSystem,
                         Environment.ProcessorCount,
                         Environment.MachineName,
                         Environment.WorkingSet,
                         Environment.SystemPageSize);

            return ret;
        }

        System.Data.DataTable ClientInfoResult()
        {
            var headers = string.Join(", ", this.Request.Headers.Select(kv => $"{kv.Key}"));
            var props = string.Join(", ", this.Request.Properties.Select(kv => $"{kv.Key}"));
            
            var ret = new System.Data.DataTable();

            ret.Columns.Add("RequestHeaders", typeof(string));
            ret.Columns.Add("RequestProperties", typeof(string));

            ret.Rows.Add(headers, props);

            return ret;
        }

        System.Data.DataTable ExecuteNonQueryResult(int rows)
        {
            var ret = new System.Data.DataTable();

            ret.Columns.Add("EffectRows", typeof(string));

            ret.Rows.Add(rows);

            return ret;
        }

        System.Data.DataTable EmptyResult()
        {
            var ret = new System.Data.DataTable();

            ret.Columns.Add("Empty", typeof(string));

            return ret;
        }
    }
}