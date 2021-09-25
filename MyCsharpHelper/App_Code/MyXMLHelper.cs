using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text;


public static class MyXMLHelper
{

    public static string ToWriteXml(object Obj, System.Type ObjType)
    {

        XmlSerializer ser;
        ser = new XmlSerializer(ObjType, TargetNamespace);
        MemoryStream memStream;
        memStream = new MemoryStream();
        XmlTextWriter xmlWriter;
        xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8);
        xmlWriter.Namespaces = true;
        ser.Serialize(xmlWriter, Obj, GetNamespaces());
        xmlWriter.Close();
        memStream.Close();
        string xml;
        xml = Encoding.UTF8.GetString(memStream.GetBuffer());
        xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
        xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
        return xml;

    }
    public static object ReadFromXml(string Xml, System.Type ObjType)
    {
        XmlSerializer ser;
        ser = new XmlSerializer(ObjType);
        StringReader stringReader;
        stringReader = new StringReader(Xml);
        XmlTextReader xmlReader;
        xmlReader = new XmlTextReader(stringReader);
        object obj;
        obj = ser.Deserialize(xmlReader);
        xmlReader.Close();
        stringReader.Close();
        return obj;

    }

    private static XmlSerializerNamespaces GetNamespaces()
    {

        XmlSerializerNamespaces ns;
        ns = new XmlSerializerNamespaces();
        ns.Add("xs", "http://www.w3.org/2001/XMLSchema");
        ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        return ns;

    }

    //Returns the target namespace for the serializer.
    private static string TargetNamespace
    {
        get
        {
            return "http://www.w3.org/2001/XMLSchema";
        }

    }
}