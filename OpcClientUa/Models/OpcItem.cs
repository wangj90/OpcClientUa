﻿namespace OpcClientUa.Models
{
    public class OpcItem
    {
        /// <summary>
        /// Item Id
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// Item值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Item质量
        /// </summary>
        public object Quality { get; internal set; }
        /// <summary>
        /// Item获取时间
        /// </summary>
        public object TimeStamp { get; internal set; }
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