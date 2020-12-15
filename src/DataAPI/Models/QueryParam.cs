using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAPI.Models
{
    public class QueryParam
    {
        /// <summary>
        /// 'user id' from sql client's connection string
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 'initial catalog' from sql client's connection string
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// sql client's SqlCommand 
        /// </summary>
        public string SqlText { get; set; }
    }
}