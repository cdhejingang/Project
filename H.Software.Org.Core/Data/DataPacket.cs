using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using static System.String;

namespace H.Software.Org.Core.Data
{
    /// <summary>
    /// 所有数据类的基类
    /// </summary>
    [Serializable]
    public class DataPacket
    {
        private int FVersion;
        /// <summary>
        /// 设置或获取对象的版本号
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public int Version
        {
            get => FVersion;
            set
            {
                if (FVersion < value)
                {
                    FVersion = value;
                }
            }
        }

        private int FID;
        /// <summary>
        /// 设置或获取对象的序列号(整型数)
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public int RID
        {
            get => FID;
            set => FID = value;
        }

        private Guid FUniqueID;
        /// <summary>
        /// 设置或获取对象的Guid
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public Guid UniqueID
        {
            get => FUniqueID;
            set => FUniqueID = value;
        }

        /// <summary>
        /// 设置或获取对象的字符串格式的Guid
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public virtual string UniqueIDStr
        {
            get => FUniqueID != Guid.Empty ? FUniqueID.ToString() : null;
            set
            {
                if (IsNullOrWhiteSpace(value))
                {
                    FUniqueID = Guid.Empty;
                    return;
                }
                FUniqueID = new Guid(value);
            }
        }

        private int FStatus;
        /// <summary>
        /// 设置或获取对象的状态字
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public int DataStatus
        {
            get => FStatus;
            set => FStatus = value;
        }

        /// <summary>
        /// 获取或设置对象的Json
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public virtual string JsonText
        {
            get => GetJsonText();
            set => SetJsonText(value);
        }

        public virtual string GetJsonText()
        {
            return JsonConvert.SerializeObject(this);
        }

        public virtual void SetJsonText(string jsonText)
        {
            if (IsNullOrWhiteSpace(jsonText))
                return;
            var obj = JsonConvert.DeserializeObject(jsonText, GetType());
            SetThisValue(obj);
        }

        /// <summary>
        /// 设置或获取对象的Xml序列。
        /// </summary>
        [JsonIgnore, XmlIgnore]
        public virtual string XMLText
        {
            get => GetXMLText();
            set => SetXMLText(value);
        }

        public virtual string GetXMLText()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(Empty, Empty);
            var serializer = new XmlSerializer(GetType(), new XmlRootAttribute(RootXmlNodeName()));
            var writer = new StringWriter(CultureInfo.InvariantCulture);
            serializer.Serialize(writer, this, namespaces);
            var xml = writer.ToString();
            writer.Close();
            writer.Dispose();
            return xml;
        }

        public virtual void SetXMLText(string xmlText)
        {
            if (IsNullOrWhiteSpace(xmlText))
                return;
            var serializer = new XmlSerializer(GetType(), new XmlRootAttribute(RootXmlNodeName()));
            var reader = new StringReader(xmlText);
            var result = serializer.Deserialize(reader);
            reader.Close();
            reader.Dispose();
            SetThisValue(result);
        }

        protected virtual string RootXmlNodeName()
        {
            return "HDataPacket";
        }

        /// <summary>
        /// 设置当前对象的值
        /// </summary>
        /// <param name="obj">来源于</param>
        public virtual void SetThisValue(object obj)
        {
            if (obj == null)
                return;

            foreach (var info in GetType().GetProperties())
            {
                if (info.IsDefined(typeof(JsonIgnoreAttribute), true) || info.IsDefined(typeof(XmlIgnoreAttribute), true))
                {
                    continue;
                }

                info.SetValue(this, obj.GetType().GetProperty(info.Name)?.GetValue(obj));
            }
        }

        /// <summary>
        /// 初始化所有字段
        /// </summary>
        public virtual void Clear()
        {
            FID = 0;
            FStatus = 0;
            FVersion = 0;
            FUniqueID = Guid.Empty;
        }
    }
}
