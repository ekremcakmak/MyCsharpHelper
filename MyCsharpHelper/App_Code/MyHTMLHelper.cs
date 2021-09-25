using System;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

public static class MyHTMLHelper
{

    public static string PrintHTML(this string obj)
    {
        string printHTML = obj;
        printHTML = HttpContext.Current.Server.HtmlDecode(printHTML);
        printHTML = printHTML.Replace("\r\n", "<br>");
        return printHTML;
    }

    public static int ToInt32(this Object obj)
    {
        if (obj == null)
        {
            return 0;
        }
        else
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch (FormatException fe)
            {
                fe = null;
                return 0;
            }
            catch (InvalidCastException ice)
            {
                ice = null;
                return 0;
            }
        }
    }
    public static float ToFloat(this Object obj)
    {
        float returnValue = 0;
        if (obj == null)
        {
            return returnValue;
        }
        else
        {
            try
            {
                float.TryParse(obj.ToString(), out returnValue);
                return returnValue;
            }
            catch (FormatException fe)
            {
                fe = null;
                return returnValue;
            }
            catch (InvalidCastException ice)
            {
                ice = null;
                return returnValue;
            }
        }
    }
    public static decimal ToDecimal(this Object obj)
    {
        decimal returnValue = 0;
        if (obj == null)
        {
            return returnValue;
        }
        else
        {
            try
            {
                decimal.TryParse(obj.ToString(), out returnValue);
                return returnValue;
            }
            catch (FormatException fe)
            {
                fe = null;
                return returnValue;
            }
            catch (InvalidCastException ice)
            {
                ice = null;
                return returnValue;
            }
        }
    }
    public static DateTime ToDateTime(this Object obj)
    {
        DateTime returnValue;
        if (obj == null)
        {
            return DateTime.Now;
        }
        else
        {
            try
            {
                returnValue = Convert.ToDateTime(obj);
            }
            catch (Exception ex)
            {
                ex = null;
                try
                {
                    returnValue = new DateTime(obj.ToString().Substring(0, 4).ToInt32(), obj.ToString().Substring(4, 2).ToInt32(), obj.ToString().Substring(6, 2).ToInt32(), obj.ToString().Substring(8, 2).ToInt32(), obj.ToString().Substring(10, 2).ToInt32(), obj.ToString().Substring(12, 2).ToInt32());
                }
                catch (Exception ex2)
                {
                    ex2 = null;
                    returnValue = DateTime.Now;
                }

            }
        }
        return returnValue;
    }
    public static bool ToBoolean(this Object obj)
    {
        bool returnValue = false;
        try
        {
            returnValue = Convert.ToBoolean(obj);
        }
        catch (Exception)
        {
            returnValue = false;
        }
        return returnValue;
    }

    public static string OpenTable(string pCellSpacing, string pCellPadding, string pBorder, string pStyle, string pClass)
    {
        string returnValue = "<table";
        if (!string.IsNullOrEmpty(pCellSpacing))
            returnValue = returnValue + " cellspacing=\"" + pCellSpacing + "\"";
        if (!string.IsNullOrEmpty(pCellPadding))
            returnValue = returnValue + " cellpadding=\"" + pCellPadding + "\"";
        if (!string.IsNullOrEmpty(pBorder))
            returnValue = returnValue + " border=\"" + pBorder + "\"";
        if (!string.IsNullOrEmpty(pStyle))
            returnValue = returnValue + " style=\"" + pStyle + "\"";
        if (!string.IsNullOrEmpty(pClass))
            returnValue = returnValue + " class=\"" + pClass + "\"";
        returnValue += ">";
        return returnValue;
    }
    public static string CloseTable()
    {
        return "</table>";
    }
    public static string InsertFlash(string pFilename, string pWidth, string pHeight)
    {
        return InsertFlash(pFilename, pWidth, pHeight, false);
    }
    public static string InsertFlash(string pFilename, string pWidth, string pHeight, bool pTransparent)
    {
        StringBuilder sbFlash = new StringBuilder();
        sbFlash.Append(@"
            <script type=""text/javascript"">
            AC_FL_RunContent('codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0', 'width', '%%WIDTH%%', 'height', '%%HEIGHT%%', 'src', '%%FILENAME%%', 'quality', 'high', 'pluginspage', 'http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash', 'movie', '%%FILENAME%%'%%TRANSPARENT1%%);
            </script>
            <noscript><object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0"" width=""%%WIDTH%%"" height=""%%HEIGHT%%"">
            <param name=""movie"" value=""%%FILENAME%%"" />
            %%TRANSPARENT2%%
            <param name=""quality"" value=""high"" />
            <embed src=""%%FILENAME%%"" quality=""high"" pluginspage=""http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash"" type=""application/x-shockwave-flash"" width=""%%WIDTH%%"" height=""%%HEIGHT%%""></embed>
            </object></noscript>");
        if (pWidth.ToInt32() < 0)
            pWidth = "100%";
        if (pHeight.ToInt32() < 0)
            pHeight = "100%";
        sbFlash.Replace("%%FILENAME%%", pFilename);
        sbFlash.Replace("%%WIDTH%%", pWidth);
        sbFlash.Replace("%%HEIGHT%%", pHeight);
        sbFlash.Replace("%%TRANSPARENT1%%", pTransparent ? ",'wmode','transparent'" : "");
        sbFlash.Replace("%%TRANSPARENT2%%", pTransparent ? "<param name=\"wmode\" value=\"transparent\" />" : "");

        return sbFlash.ToString();
    }
    public static string InsertVideo(string pFilename, string pWidth, string pHeight, string pCoverImage)
    {

        StringBuilder sbFlashVideo = new StringBuilder();
        sbFlashVideo.Append(@"
            <script type=""text/javascript"">
            AC_FL_RunContent('codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0', 'width', '%%WIDTH%%', 'height', '%%HEIGHT%%', 'src', '/mediaplayer', 'quality', 'high', 'pluginspage', 'http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash', 'movie', '/mediaplayer?file=%%FILENAME%%&amp;width=%%WIDTH%%&amp;height=%%HEIGHT%%%%COVERIMAGE1%%','allowfullscreen','true');
            </script>
            <noscript><object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0"" width=""%%WIDTH%%"" height=""%%HEIGHT%%"">
            <param name=""movie"" value=""%%FILENAME%%"" />
            <param name=""quality"" value=""high"" />
            <param name=""allowfullscreen"" value=""true"" />
            %%COVERIMAGE2%%
            <embed src=""%%FILENAME%%"" quality=""high"" pluginspage=""http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash"" type=""application/x-shockwave-flash"" width=""%%WIDTH%%"" height=""%%HEIGHT%%""></embed>
            </object></noscript>");
        if (pWidth.ToInt32() < 0)
            pWidth = "100%";
        if (pHeight.ToInt32() < 0)
            pHeight = "100%";
        sbFlashVideo.Replace("%%FILENAME%%", pFilename);
        sbFlashVideo.Replace("%%WIDTH%%", pWidth);
        sbFlashVideo.Replace("%%HEIGHT%%", pHeight);
        sbFlashVideo.Replace("%%COVERIMAGE1%%", pCoverImage.Trim().Length > 0 ? "&amp;image=" + pCoverImage : "");
        sbFlashVideo.Replace("%%COVERIMAGE2%%", pCoverImage.Trim().Length > 0 ? "<param name=\"image\" value=\"" + pCoverImage + "\" />" : "");

        return sbFlashVideo.ToString();
    }
    public static string InsertVideoAds(string pFilename, string pWidth, string pHeight, bool pAutoStart)
    {

        StringBuilder sbFlashVideo = new StringBuilder();
        sbFlashVideo.Append(@"
            <div class='video_rklm'>
            <script type=""text/javascript"">
            AC_FL_RunContent('codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,28,0', 'width', '%%WIDTH%%', 'height', '%%HEIGHT%%', 'src', '/FLVPlayer_Progressive.swf', 'quality', 'high', 'pluginspage', 'http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash', 'movie', '/FLVPlayer_Progressive?MM_ComponentVersion=1&amp;skinName=/Clear_Skin_1&amp;streamName=%%FILENAME%%&amp;autoPlay=%%AUTOPLAY%%&amp;autoRewind=true','allowfullscreen','false');
            </script>
            <noscript>
             <object type=""application/x-shockwave-flash"" data=""/FLVPlayer_Progressive.swf"" width=""%%WIDTH%%"" height=""%%HEIGHT%%"">
            <param name=""quality"" value=""high"" />
            <param name=""wmode"" value=""opaque"" />
            <param name=""scale"" value=""noscale"" />
            <param name=""FlashVars"" value=""MM_ComponentVersion=1&amp;skinName=/Clear_Skin_1&amp;streamName=%%FILENAME%%&amp;autoPlay=%%AUTOPLAY%%&amp;autoRewind=true"" />
            </object></noscript></div>");
        if (pWidth.ToInt32() < 0)
            pWidth = "100%";
        if (pHeight.ToInt32() < 0)
            pHeight = "100%";
        sbFlashVideo.Replace("%%AUTOPLAY%%", pAutoStart ? "true" : "false");
        sbFlashVideo.Replace("%%FILENAME%%", pFilename);
        sbFlashVideo.Replace("%%WIDTH%%", pWidth);
        sbFlashVideo.Replace("%%HEIGHT%%", pHeight);
        return sbFlashVideo.ToString();
    }
    public static string InsertImage(string pFileName, string pStyle)
    {
        if (string.IsNullOrEmpty(pFileName))
            return string.Empty;

        string imageTagFormat = "<img src=\"/Image.aspx?filename=/{0}&resize=3\" alt=\"\" {1} />";

        return string.Format(imageTagFormat, pFileName, pStyle);
    }
    public static string InsertImage(string pFileName, string pStyle, int pWidth, int pHeight)
    {
        if (string.IsNullOrEmpty(pFileName))
            return string.Empty;

        string imageTagFormat = "<img src=\"/Image.aspx?filename=/{0}&resize=99&width={1}&height={2}\" alt=\"\" {3} />";

        return string.Format(imageTagFormat, pFileName, pWidth.ToString(), pHeight.ToString(), pStyle);
    }

    public static bool ContainsCaseInsensitive(this string source, string value)
    {

        int results = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);

        return results == -1 ? false : true;

    }

    public static string SearchText(string search, string inside, int length)
    {
        search = " " + search + " ";
        inside = " " + inside + " ";
        int substrStart;
        int substrEnd;
        if (search.Length > inside.Length)
            return "";
        if (inside.Length <= length)
            return inside;
        else
        {
            int startIndex = inside.IndexOf(search);
            //MessageBox.Show(startIndex.ToString());
            int endIndex = startIndex + search.Length;
            if (startIndex - length / 2 <= 0)
            {
                substrStart = 0;
                //MessageBox.Show((startIndex - length).ToString());
            }
            else
                substrStart = startIndex - length / 2;
            if (endIndex + length / 2 >= inside.Length)
                substrEnd = inside.Length;
            else
                substrEnd = endIndex + length / 2;
            return inside.Substring(substrStart, substrEnd - substrStart);
        }
    }

    public static string Highlight(string Search_Str, string InputTxt)
    {

        return Regex.Replace(InputTxt,
                                "\\b(" + Regex.Escape(Search_Str) + ")\\b",
                                "<u>" + "$1" + "</u>",
                                RegexOptions.IgnoreCase
                            );
    }



}