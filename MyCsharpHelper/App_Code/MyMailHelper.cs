using System;
using System.Collections.Generic;
using System.Collections;
using System.Net.Mail;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;



// RemoveSpecialChars fuction allow us to clear string



public static class MyMailHelper
{


 private static TimeZoneInfo EASTERN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public static bool verifyEMail(string pAddress)
    {
        string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        Regex re = new Regex(strRegex);
        if (re.IsMatch(pAddress))
            return (true);
        else
            return (false);
    }
    public static bool simple_sendmail(string sender, string mailto, string subject, string message, string host, string name, bool ishtml)
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


    public static Boolean SendEmailWithTemplate(MailAddress from, MailAddress to, Hashtable templateVars, string filePath,
         string subject)
    {
        try
        {
            Parser parser = new Parser(filePath, templateVars);

            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("mail.ekremcakmak.com");
  
            mail.From = from;
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = parser.Parse();
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            smtp.Send(mail);

            return true;
        }
        catch
        {
            return false;
        }
    }


    #region SendEmailWithTemplate
    /// <summary>

    public static Boolean SendEmailWithTemplateAttahc(MailAddress from, MailAddress to, Hashtable templateVars, string body, string filePath, string subject, string attach)
    {
        try
        {
            Parser parser = new Parser(filePath, templateVars);

            MailMessage mail = new MailMessage();
  
            SmtpClient smtp = new SmtpClient("mail.ekremcakmak.com");
                mail.From = from;
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.Attachments.Add(new Attachment(attach));
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = false;

            smtp.Send(mail);

            return true;
        }
        catch
        {
            return false;
        }
    }
 

 
    public static string SendEmailWithTemplateBody(MailAddress from, MailAddress to, Hashtable templateVars, string filePath, string subject)
    {
        string body = "";
        try
        {
            Parser parser = new Parser(filePath, templateVars);

            MailMessage mail = new MailMessage();

            SmtpClient smtp = new SmtpClient("mail.ekremcakmak.com");
            mail.From = from;
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = parser.Parse();


            body = parser.Parse();

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            return body;
        }
        catch
        {
            return body;
        }
    }
  

    public static DateTime GetCurrentDateTime()
    {
        DateTime EasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EASTERN_ZONE);
        return EasternTime;
    }
    public static string TimeAgo(DateTime dt)
    {
        if (dt > GetCurrentDateTime())
            return "about sometime from now";

        TimeSpan span = GetCurrentDateTime() - dt;

        if (span.Days > 365)
        {
            int years = (span.Days / 365);
            if (span.Days % 365 != 0)
                years += 1;
            return String.Format("about {0} {1} ago", years, years == 1 ? "year" : "years");
        }

        if (span.Days > 30)
        {
            int months = (span.Days / 30);
            if (span.Days % 31 != 0)
                months += 1;
            return String.Format("about {0} {1} ago", months, months == 1 ? "month" : "months");
        }

        if (span.Days > 0)
            return String.Format("about {0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");

        if (span.Hours > 0)
            return String.Format("about {0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");

        if (span.Minutes > 0)
            return String.Format("about {0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

        if (span.Seconds > 5)
            return String.Format("about {0} seconds ago", span.Seconds);

        if (span.Seconds <= 5)
            return "just now";

        return string.Empty;
    }
    public static string getTrGun(DateTime tarih)
    {
        string c = "";
        switch (tarih.DayOfWeek.ToString())
        {
            case "Sunday": c = "Pazar"; break;
            case "Monday": c = "Pazartesi"; break;
            case "Tuesday": c = "Salı"; break;
            case "Wednesday": c = "Çarşamba"; break;
            case "Thursday": c = "Perşembe"; break;
            case "Friday": c = "Cuma"; break;
            case "Saturday": c = "Cumartesi"; break;
            default: c = tarih.DayOfWeek.ToString(); break;
        }
        return c;
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

    public class Parser
    {
        private string _strTemplateBlock;
        private Hashtable _hstValues;
        private Hashtable _ErrorMessage = new Hashtable();
        private string _ParsedBlock;

        private Dictionary<string, Parser> _Blocks = new Dictionary<string, Parser>();

        private string VariableTagBegin = "[";
        private string VariableTagEnd = "]";

        private string ModificatorTag = ":";
        private string ModificatorParamSep = ",";

        private string ConditionTagIfBegin = "##If--";
        private string ConditionTagIfEnd = "##";
        private string ConditionTagElseBegin = "##Else--";
        private string ConditionTagElseEnd = "##";
        private string ConditionTagEndIfBegin = "##EndIf--";
        private string ConditionTagEndIfEnd = "##";

        private string BlockTagBeginBegin = "##BlockBegin--";
        private string BlockTagBeginEnd = "##";
        private string BlockTagEndBegin = "##BlockEnd--";
        private string BlockTagEndEnd = "##";

        /// <value>Template block</value>
        public string TemplateBlock
        {
            get { return this._strTemplateBlock; }
            set
            {
                this._strTemplateBlock = value;
                ParseBlocks();
            }
        }

        /// <value>Template Variables</value>
        public Hashtable Variables
        {
            get { return this._hstValues; }
            set { this._hstValues = value; }
        }

        /// <value>Error Massage</value>
        public Hashtable ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        /// <value>Blocks inside template</value>
        public Dictionary<string, Parser> Blocks
        {
            get { return _Blocks; }
        }

        /// <summary>
        /// Creates a new instance of TemplateParser
        /// </summary>

       
        public Parser()
        {
            this._strTemplateBlock = "";
        }

        public Parser(string FilePath)
        {
            ReadTemplateFromFile(FilePath);
            ParseBlocks();
        }

        public Parser(Hashtable Variables)
        {
            this._hstValues = Variables;
        }

        public Parser(string FilePath, Hashtable Variables)
        {
            ReadTemplateFromFile(FilePath);
            this._hstValues = Variables;
            ParseBlocks();
        }
    public void SetTemplateFromFile(string FilePath)
    {
        ReadTemplateFromFile(FilePath);
    }


    public void SetTemplate(string TemplateBlock)
    {
        this.TemplateBlock = TemplateBlock;
    }

 
    public string Parse()
    {
        ParseConditions();
        ParseVariables();
        return this._ParsedBlock;
    }


    public string ParseBlock(string BlockName, Hashtable Variables)
    {
        if (!this._Blocks.ContainsKey(BlockName))
        {
            throw new ArgumentException(String.Format("Could not find Block with Name '{0}'", BlockName));
        }

        this._Blocks[BlockName].Variables = Variables;
        return this._Blocks[BlockName].Parse();
    }

    public bool ParseToFile(string FilePath, bool ReplaceIfExists)
    {
        if (File.Exists(FilePath) && !ReplaceIfExists)
        {
            return false;
        }
        else
        {
            StreamWriter sr = File.CreateText(FilePath);
            sr.Write(Parse());
            sr.Close();
            return true;
        }
    }

    private void ReadTemplateFromFile(string FilePath)
    {
        if (!File.Exists(FilePath))
        {
            throw new ArgumentException("Template file does not exist.");
        }

        StreamReader reader = new StreamReader(FilePath);
        this.TemplateBlock = reader.ReadToEnd();
        reader.Close();
    }

     private void ParseBlocks()
    {

        int idxCurrent = 0;
        while ((idxCurrent = this._strTemplateBlock.IndexOf(this.BlockTagBeginBegin, idxCurrent)) != -1)
        {
            string BlockName;
            int idxBlockBeginBegin, idxBlockBeginEnd, idxBlockEndBegin;

            idxBlockBeginBegin = idxCurrent;
            idxCurrent += this.BlockTagBeginBegin.Length;

            // Searching for BlockBeginEnd Index

            idxBlockBeginEnd = this._strTemplateBlock.IndexOf(this.BlockTagBeginEnd, idxCurrent);
            if (idxBlockBeginEnd == -1) throw new Exception("Could not find BlockTagBeginEnd");

            // Getting Block Name

            BlockName = this._strTemplateBlock.Substring(idxCurrent, (idxBlockBeginEnd - idxCurrent));
            idxCurrent = idxBlockBeginEnd + this.BlockTagBeginEnd.Length;

            // Getting End of Block index

            string EndBlockStatment = this.BlockTagEndBegin + BlockName + this.BlockTagEndEnd;
            idxBlockEndBegin = this._strTemplateBlock.IndexOf(EndBlockStatment, idxCurrent);
            if (idxBlockEndBegin == -1) throw new Exception("Could not find End of Block with name '" + BlockName + "'");

            // Add Block to Dictionary

            Parser block = new Parser();
            block.TemplateBlock = this._strTemplateBlock.Substring(idxCurrent, (idxBlockEndBegin - idxCurrent));
            this._Blocks.Add(BlockName, block);

            // Remove Block Declaration From Template

            this._strTemplateBlock = this._strTemplateBlock.Remove(idxBlockBeginBegin, (idxBlockEndBegin - idxBlockBeginBegin));

            idxCurrent = idxBlockBeginBegin;
        }
    }

 
    private void ParseConditions()
    {
        int idxPrevious = 0;
        int idxCurrent = 0;
        this._ParsedBlock = "";
        while ((idxCurrent = this._strTemplateBlock.IndexOf(this.ConditionTagIfBegin, idxCurrent)) != -1)
        {
            string VarName;
            string TrueBlock, FalseBlock;
            string ElseStatment, EndIfStatment;
            int idxIfBegin, idxIfEnd, idxElseBegin, idxEndIfBegin;
            bool boolValue;

            idxIfBegin = idxCurrent;
            idxCurrent += this.ConditionTagIfBegin.Length;

            // Searching for EndIf Index

            idxIfEnd = this._strTemplateBlock.IndexOf(this.ConditionTagIfEnd, idxCurrent);
            if (idxIfEnd == -1) throw new Exception("Could not find ConditionTagIfEnd");

            // Getting Value Name

            VarName = this._strTemplateBlock.Substring(idxCurrent, (idxIfEnd - idxCurrent));

            idxCurrent = idxIfEnd + this.ConditionTagIfEnd.Length;

            // Compare ElseIf and EndIf Indexes

            ElseStatment = this.ConditionTagElseBegin + VarName + this.ConditionTagElseEnd;
            EndIfStatment = this.ConditionTagEndIfBegin + VarName + this.ConditionTagEndIfEnd;
            idxElseBegin = this._strTemplateBlock.IndexOf(ElseStatment, idxCurrent);
            idxEndIfBegin = this._strTemplateBlock.IndexOf(EndIfStatment, idxCurrent);
            if (idxElseBegin > idxEndIfBegin) throw new Exception("Condition Else Tag placed after Condition Tag EndIf for '" + VarName + "'");

            // Getting True and False Condition Blocks

            if (idxElseBegin != -1)
            {
                TrueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxElseBegin - idxCurrent));
                FalseBlock = this._strTemplateBlock.Substring((idxElseBegin + ElseStatment.Length), (idxEndIfBegin - idxElseBegin - ElseStatment.Length));
            }
            else
            {
                TrueBlock = this._strTemplateBlock.Substring(idxCurrent, (idxEndIfBegin - idxCurrent));
                FalseBlock = "";
            }

            // Parse Condition

            try
            {
                boolValue = Convert.ToBoolean(this._hstValues[VarName]);
            }
            catch
            {
                boolValue = false;
            }

            string BeforeBlock = this._strTemplateBlock.Substring(idxPrevious, (idxIfBegin - idxPrevious));

            if (this._hstValues.ContainsKey(VarName) && boolValue)
            {
                this._ParsedBlock += BeforeBlock + TrueBlock.Trim();
            }
            else
            {
                this._ParsedBlock += BeforeBlock + FalseBlock.Trim();
            }

            idxCurrent = idxEndIfBegin + EndIfStatment.Length;
            idxPrevious = idxCurrent;
        }
        this._ParsedBlock += this._strTemplateBlock.Substring(idxPrevious);
    }

    private void ParseVariables()
    {
        int idxCurrent = 0;
        while ((idxCurrent = this._ParsedBlock.IndexOf(this.VariableTagBegin, idxCurrent)) != -1)
        {
            string VarName, VarValue;
            int idxVarTagEnd;

            idxVarTagEnd = this._ParsedBlock.IndexOf(this.VariableTagEnd, (idxCurrent + this.VariableTagBegin.Length));
            if (idxVarTagEnd == -1) throw new Exception(String.Format("Index {0}: could not find Variable End Tag", idxCurrent));

            // Getting Variable Name

            VarName = this._ParsedBlock.Substring((idxCurrent + this.VariableTagBegin.Length), (idxVarTagEnd - idxCurrent - this.VariableTagBegin.Length));

            // Checking for Modificators

            string[] VarParts = VarName.Split(this.ModificatorTag.ToCharArray());
            VarName = VarParts[0];

            // Getting Variable Value
            // If Variable doesn't exist in _hstValue then
            // Variable Value equal empty string

            // [added 6/6/2006] If variable is null than it will also has empty string

            VarValue = String.Empty;
            if (this._hstValues.ContainsKey(VarName) && this._hstValues[VarName] != null)
            {
                VarValue = this._hstValues[VarName].ToString();
            }

            // Apply All Modificators to Variable Value

            for (int i = 1; i < VarParts.Length; i++)
                this.ApplyModificator(ref VarValue, VarParts[i]);

            // Replace Variable in Template

            this._ParsedBlock = this._ParsedBlock.Substring(0, idxCurrent) + VarValue + this._ParsedBlock.Substring(idxVarTagEnd + this.VariableTagEnd.Length);

            // Add Length of added value to Current index 
            // to prevent looking for variables in the added value
            // Fixed Date: April 5, 2006
            idxCurrent += VarValue.Length;
        }
    }

    private void ApplyModificator(ref string Value, string Modificator)
    {
        // Checking for parameters

        string strModificatorName = "";
        string strParameters = "";
        int idxStartBrackets, idxEndBrackets;
        if ((idxStartBrackets = Modificator.IndexOf("(")) != -1)
        {
            idxEndBrackets = Modificator.IndexOf(")", idxStartBrackets);
            if (idxEndBrackets == -1)
            {
                throw new Exception("Incorrect modificator expression");
            }
            else
            {
                strModificatorName = Modificator.Substring(0, idxStartBrackets).ToUpper();
                strParameters = Modificator.Substring(idxStartBrackets + 1, (idxEndBrackets - idxStartBrackets - 1));
            }
        }
        else
        {
            strModificatorName = Modificator.ToUpper();
        }
        string[] arrParameters = strParameters.Split(this.ModificatorParamSep.ToCharArray());
        for (int i = 0; i < arrParameters.Length; i++)
            arrParameters[i] = arrParameters[i].Trim();

        try
        {
            Type typeModificator = Type.GetType("TemplateParser.Modificators." + strModificatorName);
            if (typeModificator.IsSubclassOf(Type.GetType("TemplateParser.Modificators.Modificator")))
            {
                Modificator objModificator = (Modificator)Activator.CreateInstance(typeModificator);
                objModificator.Apply(ref Value, arrParameters);
            }
        }
        catch
        {
            throw new Exception(String.Format("Could not find modificator '{0}'", strModificatorName));
        }
    }

    private abstract class Modificator
    {
        protected Hashtable _parameters = new Hashtable();

        public Hashtable Parameters
        {
            get { return _parameters; }
        }

        public abstract void Apply(ref string Value, params string[] Parameters);
    }
}
        #endregion
