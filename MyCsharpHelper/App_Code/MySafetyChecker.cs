using System;
using System.Web;
using System.Data;
using System.Xml;

// This class help us to check any SQL Injection Characters like  --, ',select,drop,exec ect... while user control.
// While login in with web form application with username and password, we checked SQL Injection possibilites. 
// In this project we storaged critical user information in different server than app and web server
// SQL Server - Connection string does not exist in application and web server. 
// We used web services to get username and password while cheking credencial.
// Storing SQL connection information in web.config file allow attacher to access all critical user information.
// Public after starting to use .Net Core
public static class MySafetyChecker
{
    public static DataSet StringtoDataset(string xmlstring, string username, string password)
    {
        com.turksin.checkuser.Service sr = new com.turksin.checkuser.Service();
        XmlNodeReader nr = new XmlNodeReader(sr.XmlIslet(StrtoXmldocument(xmlstring).DocumentElement, StrtoXmldocument("<User><Username>" + username + "</Username><Password>" + password + "</Password></User>").DocumentElement));
        DataSet ds = new DataSet();

        try
        {
            ds.ReadXml(nr);
        }
        catch
        { }

        if (ds.Tables[0].Columns.Contains("Error"))
        {
            throw new Exception(ds.Tables[0].Rows[0]["Error"].ToString());
        }
        else
        {
            return ds;
        }
    }

    public static XmlDocument StrtoXmldocument(string str)
    {
        XmlDocument xd = new XmlDocument();
        try
        {
            string str1 = str;
            xd.LoadXml(str1);
        }
        catch
        {

        }

        return xd;
    }

    public static void Set_Session(string key, string data)
    {
        System.Web.HttpContext.Current.Session[key] = data;
    }


    public static void Set_Cookie(string key, string data)
    {
        key = HttpContext.Current.Server.UrlEncode(key);
        HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
        cookie = new HttpCookie(Basic_Encode(1, key), Basic_Encode(1, data));
        DateTime ExpiryDate = DateTime.Now.AddYears(1);
        cookie.Expires = ExpiryDate;
        HttpContext.Current.Response.Cookies.Set(cookie);
    }



    public static string Get_Cookie(string key)
    {
        try
        {
            key = Stop_injection(key);

            return Stop_injection(Decoder(1, System.Web.HttpContext.Current.Request.Cookies[Basic_Encode(1, key)].Value));

        }
        catch
        {
            return "";
        }
    }

    private static string Decoder(int v, string value)
    {
        throw new NotImplementedException();
    }

    public static string Get_Session(string key)
    {
        try
        {
            key = Stop_injection(key);
            return Stop_injection(System.Web.HttpContext.Current.Session[key].ToString());
        }
        catch
        {
            return "";
        }
    }



    public static string Basic_Encode(int deep, string data)
    {
        int i;
        string s = data;

        for (i = 0; i < deep; i++)
        {
            s = Encoder(s);
        }

        return s;
    }



    public static string Basic_Decode(int deep, string data)
    {
        int i;
        string s = data;

        for (i = 0; i < deep; i++)
        {
            s = Decoder(s);
        }
        return s;
    }


    private static string Encoder(string s)
    {
        string result = "";
        System.Text.Encoding ascii = System.Text.Encoding.UTF8;
        Byte[] encodedbytes = ascii.GetBytes(s);
        foreach (Byte b in encodedbytes)
        {
            if (b.ToString().Length == 2) result += "0";
            result += b;
        }
        return result;
    }

    private static string Decoder(string s)
    {
        string result = "";
        string s1 = "";
        System.Text.Encoding ascii = System.Text.Encoding.UTF8;
        int k;
        k = s.Length;
        k = k / 3; // Number of character
        Byte[] encodedbytes = new Byte[k];
        for (int i = 0; i < k; i++)
        {
            s1 = "" + s[i * 3] + s[3 * i + 1] + s[3 * i + 2];
            try
            {
                encodedbytes[i] = Convert.ToByte(s1);
            }
            catch
            {
                encodedbytes[i] = 0;
            }
        }
        result = ascii.GetString(encodedbytes);
        return result;
    }


    public static string Get_Request(string data)
    {
        try
        {
            data = Stop_injection(data);
            return Stop_injection(HttpContext.Current.Request[data].ToString());
        }
        catch
        {
            return "";
        }
    }

    public static string Stop_injection(string data)
    {
        try
        {
            if (data.Trim() != "")
            {
                data = data.Replace("select", "");
                data = data.Replace("SELECT", "");
                data = data.Replace("from", "");
                data = data.Replace("FROM", "");
                data = data.Replace("=insert", "");
                data = data.Replace("INSERT", "");
                data = data.Replace("'", "");
                data = data.Replace(" or ", "");
                data = data.Replace(" OR ", "");
                data = data.Replace("--", "");
                data = data.Replace("drop", "");
                data = data.Replace("DROP", "");
                data = data.Replace("UPDATE", "");
                data = data.Replace("update", "");
                data = data.Replace("TABLE", "");
                data = data.Replace("table", "");
                data = data.Replace("exec", "");


                return data;
            }
            else
            {
                return "";
            }
        }
        catch
        {
            return "";
        }

    }

    public static void Redirect_to(string url)
    {
        System.Web.HttpContext.Current.Response.Redirect(url);
    }

   
}