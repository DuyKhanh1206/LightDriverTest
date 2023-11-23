using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace CommonLibrary
{
    class XSDUtils//private class(同名前空間のみのクラス)
    {
        /// <summary>XMLがXSDどおりに記載されているかチェックする</summary>            Kiểm tra xem XML có được viết theo XSD không
        public static bool XmlValidate(string sXmlPath, string sXsdString)
        {
            bool bRes = true;

            XmlSchema xs;
            XmlDocument xml = new XmlDocument();
            xml.Load(sXmlPath); // đọc tài liệu từ đường dẫn đã cho

            try
            {
                //xml schema 自体のエラーもチェック                Đồng thời kiểm tra lỗi trong chính lược đồ xml

                // Đọc XSD Schema từ chuỗi sXsdString và tạo đối tượng XmlSchema
                using (StringReader sr = new StringReader(sXsdString))
                {
                    xs = XmlSchema.Read(XmlReader.Create(sr), XSDUtils.xsdErrorHandler); // tạo và đọc XSD
                }
                // Thêm XSD Schema vào XmlDocument để kiểm tra tính hợp lệ
                xml.Schemas.Add(xs);

                // Tải lại tài liệu XML
                xml.Load(sXmlPath);

                // Thực hiện kiểm tra tính hợp lệ của tài liệu XML bằng XSD Schema
                xml.Validate(XSDUtils.xsdErrorHandler);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                bRes = false;
            }

            return bRes;
        }

        private static void xsdErrorHandler(object aSender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                throw new Exception(e.Message);
            }
        }

        #region ■参考
        //参考１）Load機能はいらないためこちらを元に改造
        //public static XmlDocument checkAndLoad(string sCheckXmlFilePath, string sXsdString)
        //{
        //    XmlSchema xs;
        //    XmlDocument xml = new XmlDocument();
        //    xml.Load(sCheckXmlFilePath);

        //    using (StringReader sr = new StringReader(sXsdString))
        //    //xml schema 自体のエラーもチェック
        //    {
        //        xs = XmlSchema.Read(XmlReader.Create(sr), XSDUtils.xsdErrorHandler);
        //    }

        //    xml.Schemas.Add(xs);
        //    xml.Load(sCheckXmlFilePath);
        //    xml.Validate(XSDUtils.xsdErrorHandler);

        //    return xml;
        //}

        //参考２）HPにはあったが、使わないのでコメント化
        //public static XmlDocument checkAndLoad(FileInfo sCheckXmlFilePath, string sXsdString)

        //{//オーバーロード

        //    XmlSchema xs;
        //    XmlDocument xml = new XmlDocument();
        //    xml.Load(sCheckXmlFilePath.FullName);

        //    using (StringReader sr = new StringReader(sXsdString))
        //    {//xml schema 自体のエラーもチェック
        //        xs = XmlSchema.Read(XmlReader.Create(sr), XSDUtils.xsdErrorHandler);
        //    }

        //    xml.Schemas.Add(xs);
        //    xml.Load(sCheckXmlFilePath.FullName);
        //    xml.Validate(XSDUtils.xsdErrorHandler);

        //    return xml;
        //}
        #endregion
    }
}
