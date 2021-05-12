using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Software.Org.Core.Data
{
    /// <summary>
    /// DataList 的 IList 实现。
    /// </summary>
    [Serializable]
    public class NorDataList : DataList
    {
        private IList<DataPacket> FList;

        /// <summary>
        /// 初始化 H.Software.Org.Core.Data.NorDataList 类的新实例，该实例为空并且具有默认初始容量。
        /// </summary>
        public NorDataList()
        {
            FList = new List<DataPacket>();
        }

        /// <summary>
        /// 初始化 H.Software.Org.Core.Data.NorDataList 类的新实例，该实例为空并且具有指定的初始容量。
        /// </summary>
        /// <param name="capacity">新列表最初可以存储的元素数。</param>
        public NorDataList(int capacity)
        {
            FList = new List<DataPacket>(capacity);
        }

        /// <summary>
        /// 初始化 H.Software.Org.Core.Data.NorDataList 类的新实例，该实例包含从指定集合复制的元素并且具有与所复制的元素数相同的初始容量。
        /// </summary>
        /// <param name="c">System.Collections.Generic.IEnumerable，它的元素被复制到新列表中。该表中元素必须是 DataPacket 的派生类。</param>
        public NorDataList(IEnumerable<DataPacket> c)
        {
            FList = new List<DataPacket>(c);
        }

        /// <summary>
        /// 将元素添加到 H.Software.Org.Core.Data.DataList 中。
        /// </summary>
        /// <param name="item">要添加到集合的 DataPacket 元素</param>
        /// <returns>新元素的插入位置</returns>
        protected override int AddItem(DataPacket item)
        {
            FList.Add(item);
            return FList.Count;
        }
        /// <summary>
        /// 清除 H.Software.Org.Core.Data.DataList 中的所有元素。
        /// </summary>
        protected override void ClearItems()
        {
            FList.Clear();
        }
        /// <summary>
        /// 移除指定索引处的 H.Software.Org.Core.Data.DataList 项。
        /// </summary>
        /// <param name="index">从零开始的索引（属于要移除的项）。</param>
        protected override void DeleteItem(int index)
        {
            FList.RemoveAt(index);
        }
        /// <summary>
        /// 用于实现ICollection.CopyTo()的虚函数，需重载。
        /// </summary>
        /// <param name="array">作为从 System.Collections.ICollection 复制的元素的目标位置的一维 System.Array。System.Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制。</param>
        protected override void DoCopyTo(Array array, int index)
        {
            //FList.CopyTo(Array.Resize(DataPacket[]), index);
        }

        /// <summary>
        /// 获取可用于同步对 ICollection 的访问的对象。
        /// </summary>
        /// <returns>用于同步对 ICollection 的访问的对象。</returns>
        protected override object GetSyncRoot()
        {
            return this;
        }

        /// <summary>
        /// 获取一个值，该值指示是否同步对 ICollection 的访问（线程安全）。
        /// </summary>
        /// <returns>如果对 true 的访问是同步的（线程安全），则为 ICollection；否则为 false。</returns>
        protected override bool GetIsSynchronized()
        {
            return false;
        }

        /// <summary>
        /// 获取 H.Software.Org.Core.Data.DataList 中实际包含的元素数。
        /// </summary>
        protected override int GetCount()
        {
            return FList.Count;
        }
        /// <summary>
        ///  获取指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns>指定索引处的 DataPacket 元素。</returns>
        protected override DataPacket GetItems(int index)
        {
            return FList[index];
        }
        /// <summary>
        /// 确定 H.Software.Org.Core.Data.DataList 中特定元素的索引。
        /// </summary>
        /// <param name="item">要在 H.Software.Org.Core.Data.DataList 中查找的 DataPacket 元素。</param>
        /// <returns>如果在列表中找到，则为 item 的索引；否则为 -1。</returns>
        protected override int IndexOfItem(DataPacket item)
        {
            return FList.IndexOf(item);
        }
        /// <summary>
        ///  将一个新元素插入指定索引处的 H.Software.Org.Core.Data.DataList。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 item。</param>
        /// <param name="item">要插入H.Software.Org.Core.Data.DataList 中的 DataPacket 元素。</param>
        protected override void InsertItem(int index, DataPacket item)
        {
            FList.Insert(index, item);
        }
        /// <summary>
        /// 从 H.Software.Org.Core.Data.DataList 中移除特定元素的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 H.Software.Org.Core.Data.DataList 移除的 DataPacket 元素。</param>
        protected override void RemoveItem(DataPacket item)
        {
           FList.Remove(item);
        }
        /// <summary>
        ///  设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <param name="value">指定的 DataPacket 元素。</param>
        protected override void SetItems(int index, DataPacket value)
        {
            FList[index] = value;
        }
    }
}
