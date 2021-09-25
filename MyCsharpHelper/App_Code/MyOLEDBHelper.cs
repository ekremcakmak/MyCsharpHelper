using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;

public class MyOLEDBHelper
{
    private static OleDbConnection oleConn;
    private string strConn;

    private static void CreateConn(string strConn)
    {
        oleConn = new OleDbConnection(strConn);
    }
    public static string SampleDbYol(string dbroot)
    {
        return "Provider=Microsoft.Jet.OleDb.4.0;Data Source=" +
            HttpContext.Current.Server.MapPath("App_Data/db.mdb");
    }

    private HttpContext curcont;
    public string DbTool()
    {
        strConn = ConfigurationManager.AppSettings["MdbConnStrng"].ToString();
        curcont = HttpContext.Current;
        return strConn;
    }
    public static DataView Get_Dv(string psql, string pconstr)
    {
        CreateConn(pconstr);
        DataSet ds = new DataSet();
        DataView dv = null;

        OleDbDataAdapter da = new OleDbDataAdapter(psql, oleConn);
        //try
        //{
        da.Fill(ds);
        dv = ds.Tables[0].DefaultView;
        //}
        //catch
        //{dv = null;}
        return dv;
    }
    public static string ExecuteQuery(string psql, string pconstr)
    {
        string s = "";
        CreateConn(pconstr);
        OleDbCommand com = new OleDbCommand(psql, oleConn);
        try
        {
            if (oleConn.State != ConnectionState.Open)
                oleConn.Open();
            com.ExecuteNonQuery();
        }
        catch (Exception ex) { s = ex.Message; }
        finally
        {
            oleConn.Close();
        }
        return s;
    }
    public static string ExecuteQueryReturn(string psql, string pdonecek, string pconstr)
    {
        string s = "";
        CreateConn(pconstr);
        OleDbCommand com = new OleDbCommand(psql, oleConn);
        try
        {
            if (oleConn.State != ConnectionState.Open)
                oleConn.Open();
            s = com.ExecuteScalar().ToString();
        }
        catch (Exception ex) { s = ""; }
        finally
        {
            oleConn.Close();
        }
        return s;
    }
   
    public static string ExecuteProc(string pstrConn, string psql, params string[] prms)
    {
        string s = "0";
        OleDbParameter pr;
        CreateConn(pstrConn);

        OleDbDataAdapter da = new OleDbDataAdapter();
        da.SelectCommand = new OleDbCommand();
        da.SelectCommand.CommandText = psql;
        da.SelectCommand.CommandType = CommandType.Text;
        da.SelectCommand.Connection = oleConn;

        for (int i = 0; i < prms.Length; i += 2)
        {
            pr = new OleDbParameter(prms[i], prms[i + 1]);
            da.SelectCommand.Parameters.Add(pr);
        }

        try
        {
            oleConn.Open();
            da.SelectCommand.ExecuteNonQuery();
        }
        catch (Exception exc) { s = exc.Message; }
        finally
        {
            if (oleConn.State == ConnectionState.Open)
                oleConn.Close();
        }
        return s;
    }

   

    

}
