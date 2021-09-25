using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

public static class MyFilesIOHelper
{

    public static void FileWrite(string fileName, string content)
    {
        StreamWriter writeter = new StreamWriter(fileName);
        writeter.Write(content);
        writeter.Close();
    }

    public static string RemoveHtml(string Html)
    {
        return Regex.Replace(Html, @"<(.|\n)*?>", string.Empty);
    }

    public static string CleanFileName(string kelime)
    {
        kelime = kelime.Replace("\"", "");
        kelime = kelime.Replace("\\", "");
        kelime = kelime.Replace("/", "");
        kelime = kelime.Replace(":", "");
        kelime = kelime.Replace("*", "");
        kelime = kelime.Replace("?", "");
        kelime = kelime.Replace("<", "");
        kelime = kelime.Replace(">", "");
        kelime = kelime.Replace("|", "");

        return kelime;
    }
    public static void SaveFileFromURL(string URL, string FullPath)
    {
        string path = System.IO.Path.GetDirectoryName(FullPath);
        if (!System.IO.Directory.Exists(path))
            System.IO.Directory.CreateDirectory(path);

        System.Net.WebRequest request = System.Net.WebRequest.Create(URL);
        System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
        System.IO.Stream stream = response.GetResponseStream();
        System.IO.StreamWriter writer = new System.IO.StreamWriter(FullPath);
        System.IO.BinaryReader br = new System.IO.BinaryReader(stream);
        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(writer.BaseStream);
        int Length = 256; Byte[] buffer; buffer = br.ReadBytes(Length);
        while (buffer.Length > 0)
        {
            bw.Write(buffer);
            buffer = br.ReadBytes(Length);
        }
        bw.Flush();
        bw.Close();
        br.Close();
    }
    public static byte[] Compress(byte[] data)
    {
        MemoryStream ms = new MemoryStream();
        DeflateStream stream = new DeflateStream(ms, CompressionMode.Compress);
        stream.Write(data, 0, data.Length);
        stream.Close();
        return ms.ToArray();
    }

   

    public static byte[] DeCompress(byte[] data)
    {
        MemoryStream ms = new MemoryStream();
        ms.Write(data, 0, data.Length);
        ms.Position = 0;
        DeflateStream stream = new DeflateStream(ms, CompressionMode.Decompress);
        MemoryStream temp = new MemoryStream();
        byte[] buffer = new byte[1024];
        int read;

        while (true)
        {
            read = stream.Read(buffer, 0, buffer.Length);
            if (read > 0)
            { temp.Write(buffer, 0, buffer.Length); }
            else
            { break; }
        }

        stream.Close();
        return temp.ToArray();
    }

    public static int FileCountFiltered(string dir, string validExtensions)
    {
        if (validExtensions.Length == 0)
            validExtensions = "*.*";

        int Total = 0;
        string[] extFilter = validExtensions.Split(new char[] { ';' });

        ArrayList files = new ArrayList();
        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        foreach (string extension in extFilter)
        {
            Total += dirInfo.GetFiles(extension).GetLength(0);
        }
        return Total;
    }
    public static List<FileInfo> GetFilesFiltered(string dir, string validExtensions)
    {
        List<FileInfo> files = new List<FileInfo>();
        string[] extFilter = validExtensions.Split(new char[] { ';' });
        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        foreach (string extension in extFilter)
        {
            FileInfo[] AltDosyalar = dirInfo.GetFiles(extension);
            foreach (FileInfo dosya in AltDosyalar)
            {
                files.Add(dosya);
            }
        }
        return files;

    }
    public static void DeleteFile(string pDosyaAdi)
    {
        bool IsDeleted = false;
        for (int i = 0; i < 1000; i++)
        {
            if (IsDeleted)
                break;
            try
            {
                File.Delete(pDosyaAdi);
                IsDeleted = true;
            }
            catch (IOException exceptionIO)
            {
            }
        }
        if (!IsDeleted)
            DeleteFile(pDosyaAdi);
    }

    public static void SetSelectedTreeviewNode(TreeNode pNode, string pValue)
    {
        foreach (TreeNode tn in pNode.ChildNodes)
        {
            if (CheckNode(tn, pValue))
                return;
            SetSelectedTreeviewNode(tn, pValue);
        }
    }
    static bool CheckNode(TreeNode pNode, string pValue)
    {
        if (pNode.Value.Equals(pValue))
        {
            pNode.Selected = true;
            pNode.Select();
            pNode.ExpandAll();
            TreeNode tnParent = pNode.Parent;
            while (tnParent != null)
            {
                tnParent.Expand();
                tnParent = tnParent.Parent;
            }
            return true;
        }
        return false;
    }

    public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        string[] searchPatterns = searchPattern.Split('|');
        List<string> files = new List<string>();
        foreach (string sp in searchPatterns)
            files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
        files.Sort();
        return files.ToArray();
    }
    public static string RenderControl(Control ctrl)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        ctrl.RenderControl(hw);
        return sb.ToString();
    }

    static HttpContext curCont = HttpContext.Current;
    public static string FileUpload(string klasor, FileUpload fup, bool rndisim)
    {
        if (fup.FileContent.Length == 0) return "";

        string filead = Path.GetFileName(fup.PostedFile.FileName);
        if (rndisim)
        {
            Random rnd = new Random();
            filead = rnd.Next(0, 9999).ToString() + "-" + filead;
        }

        fup.PostedFile.SaveAs(curCont.Server.MapPath(klasor) + "\\" + filead);

        return filead;
    }

    public static void SaveFile(string folder, string file, string data, bool randomname)
    {
        if (randomname)
        {
            Random rnd = new Random();
            file = rnd.Next(0, 9999).ToString() + "-" + file;
        }

        StreamWriter swr = new StreamWriter(curCont.Server.MapPath(folder) + "\\" + file, false, System.Text.Encoding.UTF8);
        swr.Write(data);
        swr.Flush();
        swr.Close();
    }
    public static void EasyDeleteFile(string folder, string file)
    {
        try
        {
            if (File.Exists(curCont.Server.MapPath(folder) + "\\" + file))
                File.Delete(curCont.Server.MapPath(folder) + "\\" + file);
        }
        catch { }
    }

    public static string EasyReadFile(string folder, string file)
    {
        StreamReader tr;

        tr = new StreamReader(HttpContext.Current.Server.MapPath(folder) + "\\" + file, System.Text.Encoding.Default);
        folder = tr.ReadToEnd();
        tr.Close();
        return folder;
    }

    public static string GetTimeStampForFileName()
    {
        DateTime nowAt = DateTime.Now;
        return string.Format("{0}{1}{2}{3}{4}{5}", nowAt.Day, nowAt.Month, nowAt.Year, nowAt.Hour, nowAt.Minute, nowAt.Second);
    }

}