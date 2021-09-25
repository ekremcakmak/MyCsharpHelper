using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;

public static class MyCsharpHelper
{
    static HttpServerUtility server;


    public static string GetValue(string pKey)
    {
        string returnValue = string.Empty;
        try
        {
            returnValue = ConfigurationManager.AppSettings[pKey];
        }
        catch (Exception ex)
        {
            returnValue = string.Empty;
        }
        return returnValue;
    }

    public static string ScriptName
    {
        get
        {
            string[] sayfaAdi = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString().Split('/');
            return sayfaAdi[sayfaAdi.Length - 1];
        }
    }
    public static string GetScriptName
    {
        get
        {
            return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
        }
    }

    public static void RestartApplication()
    {
        System.Web.HttpRuntime.UnloadAppDomain();
    }
    public static string CleanText(string text)
    {
        string deger = text;

        deger = deger.Replace("'", "");
        deger = deger.Replace("<", "");
        deger = deger.Replace(">", "");
        deger = deger.Replace("&", "");
        deger = deger.Replace("[", "");
        deger = deger.Replace("]", "");

        return deger;
    }
    public static string IntControl(string Text)
    {
        try
        {
            int x = Convert.ToInt32(Text);
        }
        catch
        {
            Text = "0";
        }

        return Text;
    }

    public static string UrlSeo(string Metin)
    {
        string deger = Metin;
        deger = deger.Replace("'", "");
        deger = deger.Replace(" ", "_");
        deger = deger.Replace("<", "");
        deger = deger.Replace(">", "");
        deger = deger.Replace("&", "");
        deger = deger.Replace("[", "");
        deger = deger.Replace("]", "");
        deger = deger.Replace("ı", "i");
        deger = deger.Replace("ö", "o");
        deger = deger.Replace("ü", "u");
        deger = deger.Replace("ş", "s");
        deger = deger.Replace("ç", "c");
        deger = deger.Replace("ğ", "g");
        deger = deger.Replace("İ", "i");
        deger = deger.Replace("Ö", "o");
        deger = deger.Replace("Ü", "u");
        deger = deger.Replace("Ş", "s");
        deger = deger.Replace("Ç", "c");
        deger = deger.Replace("Ğ", "g");

        return deger;
    }

    public static string ShotText(string text, int lenth)
    {
        if (text.Length >= lenth)
            text = text.Substring(0, lenth);

        return text;
    }

    public static string RetrieveSubDomain(Uri url)
    {
        string subDomain = "";
        if (url.HostNameType == UriHostNameType.Dns && (!(url.HostNameType == UriHostNameType.Unknown)))
        {
            string host = url.Host;
            int length = host.Split('.').Length;
            if (length > 2)
            {
                int last = host.LastIndexOf(".");
                int idx = host.LastIndexOf(".", last - 1);
                subDomain = host.Substring(0, idx);
            }
        }
        return subDomain;
    }
    public static string RefererPage()
    {
        string returnValue = "";
        if (HttpContext.Current.Request.UrlReferrer != null)
        {
            returnValue = HttpContext.Current.Request.UrlReferrer.AbsolutePath.Replace("/", "");
        }
        return returnValue;
    }

    public static string GetCleanString(object pObject, int pLength, bool pTryFinishWithSpace)
    {
        string returnValue = pObject.ToString();
        if (!string.IsNullOrEmpty(returnValue))
        {
            returnValue = DisarmTags(returnValue);
            if (pLength > 0)
            {
                if (pLength >= returnValue.Length)
                    pLength = returnValue.Length;
                if (pTryFinishWithSpace)
                {
                    int k = returnValue.IndexOf(" ", pLength);
                    if (k > pLength)
                    {
                        pLength = k;
                    }
                }
                returnValue = returnValue.Substring(0, pLength);
            }
        }
        else
        {
            returnValue = string.Empty;
        }
        return returnValue;
    }
    

    public static string RemoveSpecialChars(string str)
    {
        string[] chars = new string[] { " ", ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };

        for (int i = 0; i < chars.Length; i++)
        {
            if (str.Contains(chars[i]))
            {
                str = str.Replace(chars[i], "-");
            }
        }
        return str;
    }

    public static bool isNullorEmpty(object deger)
    {
        bool sonuc = false;
        if (deger == null)
        {
            sonuc = true;
        }
        else
        {
            if (Convert.ToString(deger) == "")
            {
                sonuc = true;
            }
        }
        return sonuc;
    }
    public static string NowEx()
    {
        DateTime now = DateTime.Now;
        return DateEx(now);
    }

    public static string IsUnique(string text)
    {
        string session = text;
        if (HttpContext.Current.Session[session] == null)
        {
            Random rnd = new Random();
            HttpContext.Current.Session[session] = rnd.Next(1, 999999);
        }
        else
        {
            int sayi = Convert.ToInt32(HttpContext.Current.Session[session]);
            HttpContext.Current.Session[session] = ++sayi;
        }
        return HttpContext.Current.Session[session].ToString();
    }

   
    public static string DateEx(DateTime pDate)
    {
        return pDate.Year.ToString() + Add0toBegining(pDate.Month.ToString(), 2) + Add0toBegining(pDate.Day.ToString(), 2) + Add0toBegining(pDate.Hour.ToString(), 2) + Add0toBegining(pDate.Minute.ToString(), 2) + Add0toBegining(pDate.Second.ToString(), 2);
    }
    public static int DateExSmall(DateTime pDate)
    {
        return (pDate.Year.ToString() + Add0toBegining(pDate.Month.ToString(), 2) + Add0toBegining(pDate.Day.ToString(), 2)).ToInt32();
    }
    public static string Add0toBegining(string sayi, int toplamKarakterSayisi)
    {
        for (int i = sayi.Length; i < toplamKarakterSayisi; i++)
        {
            sayi = "0" + sayi;
        }
        return sayi;
    }


       public static void jsAlert(Page page, string Mesaj)
    {
        page.RegisterStartupScript("mesaj_" + IsUnique("Unique"), "<script>alert('" + Mesaj + "')</script>");
    }

    [Obsolete]
    public static void jsAlert(Page page, string Mesaj, string location)
    {
        page.RegisterStartupScript("mesaj", "<script>alert('" + Mesaj + "');window.location='" + location + "'</script>");
    }
    [Obsolete]
    public static void jsAlert(Page page, string Mesaj, bool refresh)
    {
        if (refresh)
        {
            page.RegisterStartupScript("mesaj", "<script>alert('" + Mesaj + "');window.location='" + HttpContext.Current.Request.Url.ToString() + "'</script>");
        }
        else
        {
            page.RegisterStartupScript("mesaj", "<script>alert('" + Mesaj + "')</script>");
        }
    }

    public static void Redirect(string Url, bool endResponse)
    {
        HttpContext.Current.Response.Redirect(Url, true);
    }
    public static void RedirectHome()
    {
        HttpContext.Current.Response.Redirect("/", true);
    }
    public static void Redirect(string sayfaUrl)
    {
        Redirect(sayfaUrl, true);
    }
    public static int Levenshtein(string s, string t)
    {
        int n = s.Length; //length of s
        int m = t.Length; //length of t
        int[,] d = new int[n + 1, m + 1]; // matrix
        int cost; // cost
        if (n == 0) return m;
        if (m == 0) return n;
        for (int i = 0; i <= n; d[i, 0] = i++) ;
        for (int j = 0; j <= m; d[0, j] = j++) ;
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
                d[i, j] = System.Math.Min(System.Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }
    public static void SetSelectedItemsOnCheckBoxList(ref CheckBoxList cbl, string itemIds)
    {
        string[] arrItemId = itemIds.Split(',');
        foreach (string id in arrItemId)
        {
            ListItem foundItem = cbl.Items.FindByValue(id);
            if (foundItem != null)
            {
                foundItem.Selected = true;
            }
        }
    }
    public static string GetSelectedItemsOnCheckBoxList(CheckBoxList cbl)
    {
        string itemIds = string.Empty;
        foreach (ListItem liSelectedItem in cbl.Items)
        {
            if (liSelectedItem.Selected)
                itemIds += liSelectedItem.Value + ',';
        }
        if (itemIds.Length > 0)
        {
            if (itemIds[itemIds.Length - 1] == ',')
                itemIds = itemIds.Substring(0, itemIds.Length - 1);
        }
        return itemIds;
    }
    public static string StripForUrl(string pUrl)
    {
        //string harfler = "abcdefghijklmnopqrstuvwxyz1234567890-";
        if (string.IsNullOrEmpty(pUrl))
            return "GENEL";
        pUrl = pUrl.Replace((char)246, (char)111);
        pUrl = pUrl.Replace((char)214, (char)111);
        pUrl = pUrl.Replace((char)231, (char)99);
        pUrl = pUrl.Replace((char)199, (char)99);
        pUrl = pUrl.Replace((char)351, (char)115);
        pUrl = pUrl.Replace((char)350, (char)115);
        pUrl = pUrl.Replace((char)304, (char)105);
        pUrl = pUrl.Replace((char)305, (char)105);
        pUrl = pUrl.Replace((char)252, (char)117);
        pUrl = pUrl.Replace((char)220, (char)117);
        pUrl = pUrl.Replace((char)287, (char)103);
        pUrl = pUrl.Replace((char)286, (char)103);
        pUrl = pUrl.Replace("'", "");
        //pUrl = pUrl.Replace(".", "");        
        //pUrl = pUrl.Replace("\"", "");
        //pUrl = pUrl.Replace(',', ' ');
        //pUrl = pUrl.Replace("(", " ");
        //pUrl = pUrl.Replace("!", " ");
        //pUrl = pUrl.Replace(")", " ");
        //pUrl = pUrl.Replace("?", " ");
        //pUrl = pUrl.Replace(":", " ");
        //pUrl = pUrl.Replace(" - ", "-");
        //pUrl = pUrl.Replace(" ", "-");
        pUrl = Regex.Replace(pUrl, @"[^a-z^A-Z^0-9^-]", "-");
        while (pUrl.Contains("--"))
        {
            pUrl = pUrl.Replace("--", "-");
        }
        if (pUrl[pUrl.Length - 1] == '-')
            pUrl = pUrl.Substring(0, pUrl.Length - 1);

        return pUrl;

    }



    // www.developerbarn.com/code-samples/40-vb-net-c-bbcode-function.html
    public static string BBCode(string strTextToReplace)
    {
        Regex regExp;
        regExp = new Regex(@"\[url\]([^\]]+)\[\/url\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<a href=\"$1\">$1</a>");

        regExp = new Regex(@"\[url=([^\]]+)\]([^\]]+)\[\/url\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<a href=\"$1\">$2</a>");

        regExp = new Regex(@"\[img\]([^\]]+)\[\/img\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<img src=\"$1\" />");

        regExp = new Regex(@"\[b\](.+?)\[\/b\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<b>$1</b>");

        regExp = new Regex(@"\[i\](.+?)\[\/i\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<i>$1</i>");

        regExp = new Regex(@"\[u\](.+?)\[\/u\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<u>$1</u>");

        regExp = new Regex(@"\[size=([^\]]+)\]([^\]]+)\[\/size\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<span style=\"font-size: $1px\">$2</span>");

        regExp = new Regex(@"\[color=([^\]]+)\]([^\]]+)\[\/color\]");
        strTextToReplace = regExp.Replace(strTextToReplace, "<span style=\"color: $1\">$2</span>");

        //regExp = new Regex(@"\[video id=([^\]]+)\]([^\]]+)\[\/video\]");
        //strTextToReplace = regExp.Replace(strTextToReplace, "<span style=\"color: $1\">$2</span>");

        return strTextToReplace;
    }



    public static void EndSession()
    {
        HttpContext.Current.Application["UserSession"] = HttpContext.Current.Application["UserSession"].ToInt32() - 1;
        HttpContext.Current.Session.Clear();
    }



    public static void KickOut()
    {
        HttpContext.Current.Session.Abandon();
        HttpContext.Current.Response.Redirect("/");
        HttpContext.Current.Response.End();
    }

    public static string DisarmTags(string pText)
    {
        return HttpUtility.HtmlEncode(pText);
    }
    public static string ReplaceString(string inputStr, string searchStr, string replaceStr, int startIndex)
    {

        if (inputStr == null || inputStr.Length == 0 || searchStr == null || replaceStr == null || searchStr.Length == 0) return inputStr;

        int searchIndex = inputStr.IndexOf(searchStr, startIndex, StringComparison.InvariantCultureIgnoreCase);

        if (searchIndex == -1)

            return inputStr;

        else

            return ReplaceString(inputStr.Remove(searchIndex, searchStr.Length).Insert(searchIndex, replaceStr), searchStr, replaceStr, searchIndex + replaceStr.Length);



    }
    public static void ReplaceEx(ref string pText, string text1, string text2)
    {
        pText = pText.Replace(text1, text2);
    }
    public static bool dogrulaEPosta(string pAddress)
    {
        return Regex.Match(pAddress, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").Success;
    }
    public static bool verifyGraphicExtension(string Path)
    {
        Regex r = new Regex(@"\.gif$|\.jpg$|\.jpeg$|\.png$", RegexOptions.IgnoreCase);
        if (r.IsMatch(Path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static string BuildPubDate(DateTime d)
    {
        try
        {
            string RV = "";
            string day = d.Day.ToString();
            if (day.Length == 1) { day = "0" + day; }
            string month = d.Month.ToString();
            if (month == "1") { month = "January"; }
            else if (month == "2") { month = "February"; }
            else if (month == "3") { month = "March"; }
            else if (month == "4") { month = "April"; }
            else if (month == "5") { month = "May"; }
            else if (month == "6") { month = "June"; }
            else if (month == "7") { month = "July"; }
            else if (month == "8") { month = "August"; }
            else if (month == "9") { month = "September"; }
            else if (month == "10") { month = "October"; }
            else if (month == "11") { month = "November"; }
            else if (month == "12") { month = "December"; }

            string mTime = "";
            DateTime mDate = d.ToUniversalTime();
            if (mDate.Hour.ToString().Length == 1)
            {
                mTime = "0" + mDate.Hour.ToString();
            }
            else
            {
                mTime = mDate.Hour.ToString();
            }

            mTime += ":";
            if (mDate.Minute.ToString().Length == 1)
            {
                mTime += "0" + mDate.Minute.ToString();
            }
            else
            {
                mTime += mDate.Minute.ToString();
            }

            mTime += ":";
            if (mDate.Second.ToString().Length == 1)
            {
                mTime += "0" + mDate.Second.ToString();
            }
            else
            {
                mTime += mDate.Second.ToString();
            }

            RV = d.DayOfWeek.ToString().Substring(0, 3);
            RV += ", " + day + " " + month.Substring(0, 3);
            RV += " " + d.Year.ToString() + " " + mTime + " GMT";
            return RV;

        }
        catch (Exception)
        {
            return null;
        }
    }




    public static void jsAlertAjax(Page pPage, string pMessage)
    {
        ScriptManager.RegisterStartupScript(pPage, typeof(Page), IsUnique("Unique") + "_alerts", "window.alert('" + pMessage + "');", true);
    }
    public static void ListedenItemSec(ListControl listBox, string deger)
    {
        ListControl myListBox = listBox;
        foreach (ListItem liItem in myListBox.Items)
        {
            if (liItem.Value.Equals(deger))
            {
                liItem.Selected = true;
                return;
            }
        }
    }



  
    private static Hashtable m_executingPages = new Hashtable();

    public static void Show(string sMessage)
    {
        // If this is the first time a page has called this method then
        if (!m_executingPages.Contains(HttpContext.Current.Handler))
        {
            // Attempt to cast HttpHandler as a Page.
            Page executingPage = HttpContext.Current.Handler as Page;

            if (executingPage != null)
            {
                // Create a Queue to hold one or more messages.
                Queue messageQueue = new Queue();

                // Add our message to the Queue
                messageQueue.Enqueue(sMessage);

                // Add our message queue to the hash table. Use our page reference
                // (IHttpHandler) as the key.
                m_executingPages.Add(HttpContext.Current.Handler, messageQueue);

                // Wire up Unload event so that we can inject 
                // some JavaScript for the alerts.
                executingPage.Unload += new EventHandler(ExecutingPage_Unload);
            }
        }
        else
        {
            // If were here then the method has allready been 
            // called from the executing Page.
            // We have allready created a message queue and stored a
            // reference to it in our hastable. 
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];

            // Add our message to the Queue
            queue.Enqueue(sMessage);
        }
    }


    // Our page has finished rendering so lets output the
    // JavaScript to produce the alert's
    private static void ExecutingPage_Unload(object sender, EventArgs e)
    {
        // Get our message queue from the hashtable
        Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];

        if (queue != null)
        {
            StringBuilder sb = new StringBuilder();

            // How many messages have been registered?
            int iMsgCount = queue.Count;

            // Use StringBuilder to build up our client slide JavaScript.
            sb.Append("<script language='javascript'>");

            // Loop round registered messages
            string sMsg;
            while (iMsgCount-- > 0)
            {
                sMsg = (string)queue.Dequeue();
                sMsg = sMsg.Replace("\n", "\\n");
                sMsg = sMsg.Replace("\"", "'");
                sb.Append(@"alert( """ + sMsg + @""" );");
            }

            // Close our JS
            sb.Append(@"</script>");

            // Were done, so remove our page reference from the hashtable
            m_executingPages.Remove(HttpContext.Current.Handler);

            // Write the JavaScript to the end of the response stream.
            HttpContext.Current.Response.Write(sb.ToString());
        }
    }





    public static int CharCount(string str, char character)
    {
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == character)
                count++;
        }
        return count;
    }
    public static List<Control> FindControlRecursive(Control parControl, string ctrlName)
    {
        List<Control> lst = new List<Control>();
        foreach (Control ctrl in parControl.Controls)
        {
            if (ctrl.ID.ToLowerInvariant() == ctrlName.ToLowerInvariant())
            {
                lst.Add(ctrl);
            }
            List<Control> retval = FindControlRecursive(ctrl, ctrlName);
            lst.AddRange(retval);
        }
        return lst;
    }
    public static String Language
    {
        get
        {
            if (HttpContext.Current.Session["Language"] == null || HttpContext.Current.Session["Language"].ToString() == "")
                return "tr-TR";
            else
                return HttpContext.Current.Session["Language"].ToString();
        }
        set
        {
            HttpContext.Current.Session["Language"] = value;
        }
    }

    public static bool Simple_sendmail(string sender, string mailto, string subject, string message, string host, string name, bool ishtml)
    {
        bool sonuc;
        sonuc = false;
        System.Net.Mail.MailMessage Mail = new System.Net.Mail.MailMessage();

        Mail.IsBodyHtml = ishtml;
        Mail.IsBodyHtml = ishtml;
        Mail.Headers.Add("MIME-Version", "1.0");
        Mail.Headers.Add("Content-type", "text/html");
        Mail.Headers.Add("charset", "iso-8859-9");
        System.Net.Mail.MailAddress ma = new System.Net.Mail.MailAddress(sender, name);
        Mail.From = ma;
        Mail.To.Add(mailto);
        Mail.CC.Add(sender);
        Mail.Bcc.Add(mailto);
        Mail.Subject = subject;
        Mail.Body = message;

        try
        {
            System.Net.Mail.SmtpClient smtpMailObj = new System.Net.Mail.SmtpClient();
            smtpMailObj.Host = host;
            smtpMailObj.Send(Mail);
            sonuc = true;
        }

        catch (Exception ex)
        {
            sonuc = false;
        }
        return sonuc;
    }


    // To return xml to datatable result from Facebook FQL query
    // Sample
    // fql = "SELECT src_big,src_small FROM photo WHERE aid IN (SELECT aid FROM album WHERE owner=" + users.UserId + ")";
    //  string xml = base.FBService.DirectFQLQuery(fql);
    public static DataTable FQLXMLQueryToDT(string xml)
    {
        if (!String.IsNullOrEmpty(xml))
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);
                //xmlDocument.LoadXml(File.ReadAllText(Server.MapPath("~/XMLFile1.xml"))); 
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("fql_query_response");
                XmlNodeList results = nodeList[0].ChildNodes;

                DataTable dtTable = new DataTable();

                if (nodeList != null && nodeList.Count > 0)
                {
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (results[i].HasChildNodes)
                        {
                            XmlNodeList childNodeElement = results[i].ChildNodes;
                            if (dtTable.Columns.Count == 0)
                            {
                                dtTable = new DataTable(results[0].Name);
                                for (int j = 0; j < childNodeElement.Count; j++)
                                {
                                    DataColumn cl = new DataColumn(childNodeElement[j].Name, typeof(System.String));
                                    dtTable.Columns.Add(cl);
                                }
                            }

                            DataRow dtRow = dtTable.NewRow();
                            for (int j = 0; j < childNodeElement.Count; j++)
                            {
                                dtRow[childNodeElement[j].Name] = childNodeElement[j].ChildNodes[0].Value;
                            }
                            dtTable.Rows.Add(dtRow);
                        }
                    }

                    return dtTable;
                }
            }
            catch
            {
                return new DataTable();
            }
        }
        return new DataTable();
    }

    //To place one image over another image.
    

    


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