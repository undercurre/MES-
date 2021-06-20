using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Models.SmtMSD
{
    public class MSDData
    {
        /// <summary>
        /// 料卷條碼
        /// </summary>
        public string ReelCode
        {
            get;
            set;
        }

        /// <summary>
        /// 操作溫度
        /// </summary>
        public decimal Temperature
        {
            get;
            set;
        }

        /// <summary>
        /// 操作濕度
        /// </summary>
        public decimal Humidity
        {
            get;
            set;
        }

        /// <summary>
        /// 當前動作
        /// </summary>
        public decimal CurrentAction
        {
            get;
            set;
        }

        public string CurrentActionName
        {
            get;
            set;
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        public decimal NewAction
        {
            get;
            set;
        }

        public string OperateBy
        {
            get;
            set;
        }

        /// <summary>
        /// 作業區域
        /// </summary>
        public string ActionArea
        {
            get;
            set;
        }

        /// <summary>
        /// 作業區域類型
        /// </summary>
        public string ActionAreaType
        {
            get;
            set;
        }

        /// <summary>
        /// Floor Life
        /// </summary>
        public decimal FloorLife
        {
            get;
            set;
        }

        /// <summary>
        /// 元件等級
        /// </summary>
        public string LevelCode
        {
            get;
            set;
        }

        /// <summary>
        /// 元件厚度
        /// </summary>
        public decimal Thickness
        {
            get;
            set;
        }

        /// <summary>
        /// MSDRuncardRow
        /// </summary>
        public SmtMsdRuncard MSDRuncardRow
        {
            get;
            set;
        }

        /// <summary>
        /// 暴露時間
        /// </summary>
        public decimal TotalOpenTime
        {
            get;
            set;
        }

        public decimal StandardBakeTime
        {
            get;
            set;
        }
    }
}
