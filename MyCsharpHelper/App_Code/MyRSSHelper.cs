using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.Collections;
using System.Web.UI;

public class MyRSSHelper
{
    private static XmlTextWriter AddRSSItem(XmlTextWriter writer, string sItemTitle, string sItemLink, string sItemDescription, string sDate, string sAuthor)
    {
        writer.WriteStartElement("item");
        writer.WriteElementString("title", HttpContext.Current.Server.HtmlDecode(sItemTitle));
        writer.WriteElementString("link", sItemLink);
        writer.WriteElementString("description", sItemDescription);
        writer.WriteElementString("pubDate", sDate);
        writer.WriteElementString("author", HttpContext.Current.Server.HtmlDecode(sAuthor));
        writer.WriteEndElement();

        return writer;
    }
    private static XmlTextWriter WriteRSSClosing(XmlTextWriter writer)
    {
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.WriteEndDocument();

        return writer;
    }
    // Creating RSS Header
    private static XmlTextWriter WriteRSSPrologue(XmlTextWriter writer, string strRSSType, string url)
    {
        writer.WriteStartDocument();
        writer.WriteStartElement("rss");
        writer.WriteAttributeString("version", "2.0");
        writer.WriteAttributeString("xmlns:blogChannel", "bgurl");
        writer.WriteStartElement("channel");

        string strTitle = "Turksin Last Members";
        if (strRSSType == "Comments")
            strTitle += (" Turksin link Company");

        writer.WriteElementString("title", strTitle);
        writer.WriteElementString("link", "http://" + url);
        writer.WriteElementString("description", "description ");
        writer.WriteElementString("copyright", "Copyright " + "copyright");
        writer.WriteElementString("generator", "generator");
        return writer;
    }

}
