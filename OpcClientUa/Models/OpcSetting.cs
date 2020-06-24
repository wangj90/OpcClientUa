using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpcClientUa.Models
{
    public class OpcSetting
    {
        /// <summary>
        /// Item Id
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 高报警值
        /// </summary>
        public int? HighAlarm { get; set; }
        /// <summary>
        /// 低报警值
        /// </summary>
        public int? LowAlarm { get; set; }
    }
}
