using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using static System.String;

namespace H.Software.Org.Core.Data
{
    /// <summary>
    /// 从DataPacket派生，实现数据集合管理
    /// </summary>
    [Serializable]
    public abstract class DataList : DataPacket, IList
    {
        /// <summary>
        ///  将一个新元素插入指定索引处的 H.Software.Org.Core.Data.DataList。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 item。</param>
        /// <param name="item">要插入H.Software.Org.Core.Data.DataList 中的 DataPacket 元素。</param>
        protected abstract void InsertItem(int index, DataPacket item);

        /// <summary>
        /// 从 H.Software.Org.Core.Data.DataList 中移除特定元素的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 H.Software.Org.Core.Data.DataList 移除的 DataPacket 元素。</param>
        protected abstract void RemoveItem(DataPacket item);

        /// <summary>
        /// 移除指定索引处的 H.Software.Org.Core.Data.DataList 项。
        /// </summary>
        /// <param name="index">从零开始的索引（属于要移除的项）。</param>
        protected abstract void DeleteItem(int index);

        /// <summary>
        /// 确定 H.Software.Org.Core.Data.DataList 中特定元素的索引。
        /// </summary>
        /// <param name="item">要在 H.Software.Org.Core.Data.DataList 中查找的 DataPacket 元素。</param>
        /// <returns>如果在列表中找到，则为 item 的索引；否则为 -1。</returns>
        protected abstract int IndexOfItem(DataPacket item);

        /// <summary>
        ///  获取指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns>指定索引处的 DataPacket 元素。</returns>
        protected abstract DataPacket GetItems(int index);

        /// <summary>
        ///  设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <param name="value">指定的 DataPacket 元素。</param>
        protected abstract void SetItems(int index, DataPacket value);

        /// <summary>
        /// 清除 H.Software.Org.Core.Data.DataList 中的所有元素。
        /// </summary>
        protected abstract void ClearItems();

        /// <summary>
        /// 将元素添加到 H.Software.Org.Core.Data.DataList 中。
        /// </summary>
        /// <param name="item">要添加到集合的 DataPacket 元素</param>
        /// <returns>新元素的插入位置</returns>
        protected abstract int AddItem(DataPacket item);

        /// <summary>
        /// 获取 H.Software.Org.Core.Data.DataList 中实际包含的元素数。
        /// </summary>
        protected abstract int GetCount();

        /// <summary>
        /// 用于实现ICollection.CopyTo()的虚函数。
        /// </summary>
        /// <param name="array">作为从 System.Collections.ICollection 复制的元素的目标位置的一维 System.Array。System.Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制。</param>
        protected abstract void DoCopyTo(Array array, int index);

        /// <summary>
        /// 获取可用于同步对 ICollection 的访问的对象。
        /// </summary>
        /// <returns>用于同步对 ICollection 的访问的对象。</returns>
        protected abstract object GetSyncRoot();

        /// <summary>
        /// 获取一个值，该值指示是否同步对 ICollection 的访问（线程安全）。
        /// </summary>
        /// <returns>如果对 true 的访问是同步的（线程安全），则为 ICollection；否则为 false。</returns>
        protected abstract bool GetIsSynchronized();



        private Type FItemType;
        /// <summary>
        /// 设置或获取被管理元素的缺省类型，可以为 null。如果要从Xml中恢复，必须指定该属性。
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public Type ItemType
        {
            get => GetItemType();
            set => FItemType = value;
        }

        /// <summary>
        /// 取得ItemType的虚函数, 如果未设ItemType且有元素存在，则取第一个元素类型
        /// </summary>
        /// <returns>ItemType</returns>
        protected virtual Type GetItemType()
        {
            if (FItemType == null && Count > 0)
            {
                FItemType = this[0].GetType();
            }
            return FItemType;
        }

        /// <summary>
        /// 根据ItemType创建一个新 DataPacket 元素
        /// </summary>
        /// <returns>新 DataPacket 元素, 如果 ItemType 为 null 则返回 null。</returns>
        protected virtual DataPacket CreateNewItem()
        {
            if (ItemType != null)
            {
                return Activator.CreateInstance(ItemType) as DataPacket;
            }
            return null;
        }

        /// <summary>
        /// 从一个集合中添加元素引用，并不复制元素。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="c">源集合</param>
        protected virtual void AddFrom(ICollection c)
        {
            foreach (object current in c)
            {
                Add(current);
            }
        }

        /// <summary>
        /// 从一个集合中添加元素附本(复制元素)。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="c">源集合</param>
        protected virtual void AddCopyFrom(ICollection c)
        {
            foreach (object current in c)
            {
                DataPacket pack1 = (DataPacket)current;
                DataPacket pack2 = CreateNewItem() ?? (DataPacket)Activator.CreateInstance(pack1.GetType());
                pack2.JsonText = pack1.JsonText;
                Add(pack2);
            }
        }

        /// <summary>
        /// 获取一个值，该值指示 IList 是否为只读。
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// 获取一个值，该值指示 IList 是否具有固定大小。
        /// </summary>
        public bool IsFixedSize => false;
        /// <summary>
        /// 获取一个值，该值指示是否同步对 ICollection 的访问（线程安全）。
        /// </summary>
        public bool IsSynchronized => GetIsSynchronized();
        /// <summary>
        /// 获取可用于同步对 ICollection 的访问的对象。
        /// </summary>
        public object SyncRoot => GetSyncRoot();

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        object IList.this[int index]
        {
            get => GetItems(index);
            set => SetItems(index, (DataPacket)value);
        }

        /// <summary>
        /// 获取 ICollection 中包含的元素数。
        /// </summary>
        public int Count => GetCount();

        /// <summary>
        /// 从 IList 中移除所有项。
        /// </summary>
        public override void Clear()
        {
            base.Clear();
            ClearItems();
        }

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        public DataPacket this[int index]
        {
            get => GetItems(index);
            set => SetItems(index, value);
        }

        protected virtual string ListXmlNodeName()
        {
            return "HDataList";
        }

        /// <summary>
        /// 将某项添加到 IList 中。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="value">要添加到 IList 的对象。</param>
        /// <returns>新元素插入到的位置；或者为 -1，指示该项未插入到集合中。</returns>
        public int Add(object value)
        {
            return AddItem((DataPacket)value);
        }

        /// <summary>
        /// 确定 IList 是否包含特定值。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="value">要在 IList 中定位的对象。</param>
        /// <returns>如果在 IList 中找到了 Object，则为 true；否则为 false。</returns>
        public bool Contains(object value)
        {
            return IndexOf(value) > -1;
        }

        /// <summary>
        /// 确定 IList 中特定项的索引。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="value">要在 IList 中定位的对象。</param>
        /// <returns>如果在列表中找到，则为 value 的索引；否则为 -1。</returns>
        public int IndexOf(object value)
        {
            return IndexOfItem((DataPacket)value);
        }

        /// <summary>
        /// 在 IList 中的指定索引处插入一个项。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="index">应插入 value 的从零开始的索引。</param>
        /// <param name="value">要插入到 IList 中的对象。</param>
        public void Insert(int index, object value)
        {
            InsertItem(index, (DataPacket)value);
        }

        /// <summary>
        /// 从 IList 中移除特定对象的第一个匹配项。要求元素必须是 DataPacket
        /// </summary>
        /// <param name="value">要从 IList 中删除的对象。</param>
        public void Remove(object value)
        {
            RemoveItem((DataPacket)value);
        }

        /// <summary>
        /// 从特定的 ICollection 索引开始，将 Array 的元素复制到一个 Array 中。
        /// </summary>
        /// <param name="array">一维 Array，它是从 ICollection 复制的元素的目标。 Array 必须具有从零开始的索引。</param>
        /// <param name="index">array 中从零开始的索引，从此处开始复制。</param>
        public void CopyTo(Array array, int index)
        {
            DoCopyTo(array, index);
        }

        /// <summary>
        /// 移除位于指定索引处的 IList 项。
        /// </summary>
        /// <param name="index">要移除的项的从零开始的索引。</param>
        public void RemoveAt(int index)
        {
            DeleteItem(index);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的 IEnumerator 对象。</returns>
        public IEnumerator GetEnumerator()
        {
            return new DataListEnumerator(this);
        }

        /// <summary>
        /// DataList集合的简单迭代。
        /// </summary>
        private class DataListEnumerator : IEnumerator
        {
            private int _indx = -1;
            private DataList _dataList;

            /// <summary>
            /// 初始化 DataListEnumerator
            /// </summary>
            /// <param name="list">DataList集合</param>
            public DataListEnumerator(DataList list)
            {
                _dataList = list;
            }

            /// <summary>
            /// 获取集合中位于枚举数当前位置的元素。
            /// </summary>
            public object Current => _dataList[_indx];

            /// <summary>
            /// 将枚举数推进到集合的下一个元素。
            /// </summary>
            /// <returns>如果枚举数已成功地推进到下一个元素，则为 true；如果枚举数传递到集合的末尾，则为 false。</returns>
            public bool MoveNext()
            {
                if (_indx < _dataList.Count - 1)
                {
                    _indx++;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 将枚举数设置为其初始位置，该位置位于集合中第一个元素之前。
            /// </summary>
            public void Reset()
            {
                _indx = -1;
            }
        }
    }
}
