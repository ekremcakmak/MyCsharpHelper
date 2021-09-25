using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public class Db
{
    private string sConStr = "";
    public IDbCommand myCommand = null;
    public IDbTransaction myTransaction = null;
    public IDbConnection myConn = null;
    public IDataReader myReader = null;
    public IDbDataAdapter myDataAdapter = null;
    public IDataReader myDataReader = null;
    public DataSet myDataSet = null;

    public Db()
    {
        try
        {
            sConStr = @"Data Source=EminLabs\SQLEXPRESS;Initial Catalog=EkremDB;Persist Security Info=True;User ID=ekremcakmak;Password=123456";
        }
        catch
        {


        }
    }
    //Create
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(sConStr);
    }

    public IDbCommand CreateCommand()
    {
        return new SqlCommand();
    }

    public IDbTransaction CreateTransaction()
    {
        SqlTransaction mySqlTrans = null;
        return mySqlTrans;
    }

    public IDbDataAdapter CreateDataAdapter()
    {
        return new SqlDataAdapter();
    }
    //Create Bitiş


    public DataSet FillDataSet(string sSQL, IDbConnection dbConn)
    {
        myDataAdapter = CreateDataAdapter();
        myDataSet = new DataSet();
        myCommand = CreateCommand();
        myCommand.Connection = CreateConnection();
        myCommand.CommandType = CommandType.Text;
        myCommand.CommandText = sSQL;
        myDataAdapter.SelectCommand = myCommand;
        myDataAdapter.Fill(myDataSet);
        myCommand.Dispose();
        DisconnectAndDispose();
        myDataAdapter = null;
        return myDataSet;

    }
    public DataSet FillDataSet(string sSQL)
    {
        myDataAdapter = CreateDataAdapter();
        myDataSet = new DataSet();
        myCommand = CreateCommand();
        myCommand.Connection = CreateConnection();
        myCommand.CommandType = CommandType.Text;
        myCommand.CommandText = sSQL;
        myDataAdapter.SelectCommand = myCommand;
        myDataAdapter.Fill(myDataSet);
        myCommand.Dispose();
        DisconnectAndDispose();
        myDataAdapter = null;
        return myDataSet;

    }

    public IDataReader FillDataReader(string sSQL)
    {
        myCommand = CreateCommand();
        ConnectAndInitialize();
        myCommand.CommandType = CommandType.Text;
        myCommand.CommandText = sSQL;

        myDataReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
        myCommand.Dispose();
        return myDataReader;
    }

    public void Dispose()
    {
        if (myCommand != null) myCommand.Dispose();
        if (myDataReader != null) myDataReader.Dispose();
        if (myDataSet != null) myDataSet.Dispose();
        myDataAdapter = null;
    }

    public void ConnectAndInitialize()
    {
        Db myData = new Db();
        myConn = myData.CreateConnection();
        myConn.Open();
        myCommand = myData.CreateCommand();
        myCommand.Connection = myConn;

    }

    public void DisconnectAndDispose()
    {
        Db myData = new Db();
        if (myCommand != null) myCommand.Dispose();
        if (myConn != null) myConn.Close();
        if (myConn != null) myConn.Dispose();
        if (myData != null) myData = null;
    }

    public void StartTransaction()
    {
        Db myData = new Db();
        myTransaction = myData.CreateTransaction();
        myTransaction = myConn.BeginTransaction();
        myCommand.Transaction = myTransaction;
    }
    //Bindli İşlemler
    public void bindGrid(GridView gView, string sql)
    {
        try
        {
            ConnectAndInitialize();
            gView.DataSource = FillDataSet(sql, myConn);
            gView.DataBind();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }

    public void bindDropDownList(DropDownList dr, string sql, string dtf, string dvf, string text, string itemValue)
    {
        Db myData = new Db();
        try
        {
            myData.ConnectAndInitialize();
            dr.DataSource = myData.FillDataSet(sql, myData.myConn);
            dr.DataTextField = dtf;
            dr.DataValueField = dvf;
            dr.DataBind();
            ListItem li = new ListItem(text, itemValue);
            dr.Items.Insert(0, li);
        }
        catch
        {
            dr.Items.Clear();
            ListItem li = new ListItem(text, itemValue);
            dr.Items.Insert(0, li);
        }
        finally
        {
            myData.DisconnectAndDispose();
        }
    }
    public void binddatalist(DataList gView, string sql)
    {
        try
        {
            ConnectAndInitialize();
            gView.DataSource = FillDataSet(sql, myConn);
            gView.DataBind();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }
    public void binlistview(ListView gView, string sql)
    {
        try
        {
            ConnectAndInitialize();
            gView.DataSource = FillDataSet(sql, myConn);
            gView.DataBind();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }
    public void formviev(FormView gView, string sql)
    {
        try
        {
            ConnectAndInitialize();
            gView.DataSource = FillDataSet(sql, myConn);
            gView.DataBind();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }
    public void bindListBox(ListBox lb, string sql, string dtf, string dvf)
    {
        Db myData = new Db();
        try
        {
            myData.ConnectAndInitialize();
            lb.DataSource = myData.FillDataSet(sql, myData.myConn);
            lb.DataTextField = dtf;
            lb.DataValueField = dvf;
            lb.DataBind();
        }
        catch
        {
            lb.Items.Clear();
        }
        finally
        {
            myData.DisconnectAndDispose();

        }

    }


    public void bindrepater(Repeater Repeater1, string sql)
    {
        try
        {
            ConnectAndInitialize();
            Repeater1.DataSource = FillDataSet(sql, myConn);
            Repeater1.DataBind();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }

    public Object sonuc(string sql)
    {
        try
        {
            object sonuc = 0;
            myCommand = CreateCommand();
            ConnectAndInitialize();
            myCommand.CommandType = CommandType.Text;
            myCommand.CommandText = sql;

            sonuc = myCommand.ExecuteScalar();
            myCommand.Dispose();
            DisconnectAndDispose();
            return sonuc;

        }
        catch
        {
            DisconnectAndDispose();
            return 0;
        }

    }

    public void bindrepater(Repeater Repeater1, string sql, Label label)
    {
        try
        {
            ConnectAndInitialize();
            Repeater1.DataSource = FillDataSet(sql, myConn);
            Repeater1.DataBind();
            label.Text = myDataSet.Tables[0].Rows.Count.ToString();
        }
        catch
        {

        }
        finally
        {
            DisconnectAndDispose();
        }
    }


    public static void DropDol(bool temizle, string seciniz, string secinizval, string secili, DropDownList drp, DataView dv,
        string txt1, string val, string txt2)
    {
        ListItem item = null;
        try
        {
            if (temizle) drp.Items.Clear();

            if (seciniz != "")
            {
                item = new ListItem();
                item.Text = seciniz;
                item.Value = secinizval;
                drp.Items.Add(item);
            }
            if (dv != null && dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    item = new ListItem();
                    item.Text = dv[i][txt1].ToString() + (txt2 != "" ? " - " + dv[i][txt2].ToString() : "");
                    item.Value = dv[i][val].ToString();
                    drp.Items.Add(item);
                }
                item = null;
                item = drp.Items.FindByValue(secili);
                if (item != null)
                    item.Selected = true;
            }
        }
        catch { }
    }


    public static string[] getCommon(string[] arrOne, string[] arrTwo)
    {
        System.Collections.ArrayList result = new System.Collections.ArrayList();

        foreach (string elem in arrOne)
        {
            result.Add(elem);
        }
        foreach (string elem in arrTwo)
        {
            result.Add(elem);
        }

        /*
        System.Collections.Hashtable ht = new System.Collections.Hashtable();

        string[] arrSmaller;
        string[] arrOther;

        if (arrOne.Length < arrTwo.Length)
        {
            arrSmaller = arrOne;
            arrOther = arrTwo;
        }
        else
        {
            arrSmaller = arrTwo;
            arrOther = arrOne;
        }

        foreach (string elem in arrSmaller)
        {
            ht[elem] = elem;
        }

        System.Collections.ArrayList result = new System.Collections.ArrayList();

        
        foreach (string elem in arrOther)
        {
            if (ht.ContainsKey(elem))
            {
                result.Add(elem);
            }
        }
        */


        return (string[])result.ToArray(typeof(string)); //(string[])result.ToArray();
    }

}
