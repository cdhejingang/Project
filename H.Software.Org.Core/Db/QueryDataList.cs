using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using H.Software.Org.Core.Data;

namespace H.Software.Org.Core.Db
{
    /// <summary>
    /// QueryDataList 的摘要说明。
    /// </summary>
    [Serializable]
    public class QueryDataList : NorDataList
    {
        private int allCount;

        /// <summary>
        /// 总记录数 
        /// </summary>
        public int TotalCount
        {
            get => allCount;
            set => allCount = value;
        }

        /// <summary>
        /// 清除所有数据。
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            allCount = 0;
        }
    }
}
