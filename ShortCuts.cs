using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Drawing.Drawing2D;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Imaging;

namespace ToolBox
{
    public static class ExtentionMethods
    {
        public static string CreateURLName(this string URLNameText)
        {
            string URLName = URLNameText.ToLower();
            URLName = URLName.Replace("c", "c");
            URLName = URLName.Replace("c", "c");
            URLName = URLName.Replace("ç", "c");
            URLName = URLName.Replace("Ç", "c");
            URLName = URLName.Replace("š", "s");
            URLName = URLName.Replace("ş", "s");
            URLName = URLName.Replace("Ş", "s");
            URLName = URLName.Replace("ğ", "g");
            URLName = URLName.Replace("Ğ", "g");
            URLName = URLName.Replace("ö", "o");
            URLName = URLName.Replace("Ö", "o");
            URLName = URLName.Replace("ı", "i");
            URLName = URLName.Replace("İ", "i");
            URLName = URLName.Replace("ü", "u");
            URLName = URLName.Replace("Ü", "u");
            URLName = URLName.Replace("-", " ");
            URLName = Regex.Replace(URLName, "[^a-z0-9\\s-]", " ");
            URLName = Regex.Replace(URLName, "\\s+", " ").Trim();
            URLName = URLName.Replace(" ", "-");
            return URLName.ToLower().ToString();
        }


        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static string StripHtml(this string input)
        {
            // Will this simple expression replace all tags???
            var tagsExpression = new Regex(@"<.*?>");
            return tagsExpression.Replace(input, " ");
        }

        public static string Encrypt(this string clearText)
        {
            string EncryptionKey = "DDME523419790201";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(this string cipherText)
        {
            string EncryptionKey = "DDME523419790201";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string ToRelativeDate(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} saniye önce", timeSpan.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Minutes > 1 ? String.Format("{0} dakika önce", timeSpan.Minutes) : "bir dakika önce";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Hours > 1 ? String.Format("{0} saat önce", timeSpan.Hours) : "bir saat önce";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Days > 1 ? String.Format("{0} gün önce", timeSpan.Days) : "dün";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Days > 30 ? String.Format("{0} ay önce", timeSpan.Days / 30) : "bir ay önce";

            return timeSpan.Days > 365 ? String.Format("{0} yıl önce", timeSpan.Days / 365) : "bir yıl önce";
        }


    }

    public static class ShortCuts
    {
        public static List<string> copulatives = new List<string>() { "ile", "ve", "veya", "ya da", "yada", ".", ",", "de", "ancak", "ya", "sanki", "hem", "bile", "çünkü", "cunku", "ki", "ama", "bile", "oysa", "ise", "var", "ha", "gene", "her", "&amp;", "hakkında", "yani", "olup", "burada", "bulabilirsiniz", "seks", "kalkmıyor", "sex" };

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try { stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None); }
            catch (IOException) { return true; }
            finally { if (stream != null) stream.Close(); }
            return false;
        }

        public class ListToDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }

        public static string SQLinjection(string inputParameter, string StringType = "other")
        {
            if (!string.IsNullOrEmpty(inputParameter))
            {
                string[,] StrWordList = new string[,]
                {
                    { "SELECT","&#83;elect"},
                    { "select","&#115;elect"},
                    { "DROP","&#68;rop"},
                    { "drop","&#100;rop"},
                    //{ ";","&#59;"},
                    { "--","&#45;&#45;"},
                    { "INSERT","&#73;nsert"},
                    { "insert","&#105;nsert"},
                    { "DELETE","&#68;&#69;&#76;&#69;&#84;&#69;"},
                    { "delete","&#100;&#101;&#108;&#108;&#116;&#101;"},
                    { "xp_","&#120p;&#95;"},
                    { "sp_","&#115;&#95;"},
                    { "UNION","&#85;nion"},
                    { "union","&#85;nion"},
                    { "'","&#39;"},
                };

                if (StringType == "other")
                {
                    for (int i = 0; i < StrWordList.GetLength(0); i++)
                    {
                        inputParameter = Regex.Replace(inputParameter, StrWordList[i, 0], StrWordList[i, 1]);
                    }
                }
                else if (StringType == "url")
                {
                    foreach (string str in inputParameter.Split('-'))
                    {
                        for (int i = 0; i < StrWordList.GetLength(0); i++)
                        {
                            inputParameter.Replace(str, Regex.Replace(str, StrWordList[i, 0], StrWordList[i, 1]));
                        }
                    }
                }
            }
            return inputParameter;
        }

        private static string ClearKeyword(string _Keyword)
        {
            string tmpKey = "";
            if (_Keyword.Contains(','))
            {
                foreach (string str in _Keyword.Split(','))
                {
                    if (tmpKey == "")
                        tmpKey = str.Trim().Trim(',').ToLower();
                    else tmpKey += "," + str.Trim().Trim(',').ToLower();
                }
                _Keyword = tmpKey;
            }
            var result = _Keyword.ToLower().Trim(',').Trim().Split(',');

            var clearedKeyword = (from v in result select v).Distinct().ToList();

            _Keyword = "";

            foreach (string sg in clearedKeyword)
            {
                string clearText = sg.Trim().Trim(',');

                var findBadWord = copulatives.Find(t => t.Contains(clearText));

                if (findBadWord == null)
                {
                    if (_Keyword == "")
                        _Keyword = clearText;
                    else _Keyword += ", " + clearText;
                }
            }
            return _Keyword;
        }

        public static string CheckMetaKeywordFormat(string keywords, int Lenght = 200)
        {
            try
            {
                string tempTitle = "";

                if (keywords.Length > Lenght)
                {
                    foreach (string str in keywords.Split(','))
                    {
                        if (tempTitle == "")
                            tempTitle = str.Trim();
                        else
                        {
                            if (tempTitle.Count() < Lenght && (tempTitle.Count() + str.Count()) < Lenght)
                                tempTitle += ", " + str.Trim();
                            else { break; }
                        }
                    }
                    return tempTitle;
                }
                else return keywords;
            }
            catch { return keywords; }
        }

        public static string CheckMetaTitleFormat(string Title, int Lenght = 300)
        {
            try
            {
                string tempTitle = "";

                if (Title.Length > Lenght)
                {
                    foreach (string str in Title.Split(' '))
                    {
                        if (tempTitle == "")
                            tempTitle = str;
                        else
                        {
                            if (tempTitle.Count() < Lenght && (tempTitle.Count() + str.Count()) < Lenght)
                                tempTitle += " " + str;
                            else { tempTitle += "..."; break; }
                        }
                    }

                    return (tempTitle.Length > 47 ? tempTitle : tempTitle + " - Hemen Sağlık");
                }
                else return (Title.Length > 47 ? Title : Title + " - Hemen Sağlık");
            }
            catch { return Title; }
        }

        public static string CheckMetaDescriptionFormat(string Description, int Lenght = 120)
        {
            try
            {
                string tempDescription = "";

                if (Description.Length > Lenght)
                {
                    foreach (string str in Description.Split(' '))
                    {

                        if (tempDescription == "")
                            tempDescription = str;
                        else
                        {
                            if (tempDescription.Count() < Lenght && (tempDescription.Count() + str.Count()) < Lenght)
                                tempDescription += " " + str;
                            else { tempDescription += "..."; break; }
                        }
                    }
                    return tempDescription;
                }
                else return Description;
            }
            catch { return Description; }
        }


        public static List<string> FindAllURL(string HTMLSource)
        {
            List<string> HrefList = new List<string>();
            Regex r = default(Regex);
            Match m = default(Match);
            r = new Regex(@"((https?:\/\/|www\.)([-\w\.]+)+(:\d+)?(\/([\w\/_\.]*(\?\S+)?)?)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            m = r.Match(HTMLSource);
            while (m.Success)
            {
                //Response.Write("<br />Found href " & m.Groups(1).Value & " at " & m.Groups(1).Index.ToString())
                HrefList.Add(m.Groups[1].Value.ToString());
                m = m.NextMatch();
            }
            return HrefList;
        }

        public static List<string> FindAllHref(string HTMLSource)
        {
            List<string> HrefList = new List<string>();
            Regex r = default(Regex);
            Match m = default(Match);
            r = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            m = r.Match(HTMLSource);
            while (m.Success)
            {
                //Response.Write("<br />Found href " & m.Groups(1).Value & " at " & m.Groups(1).Index.ToString())
                HrefList.Add(m.Groups[1].Value.ToString());
                m = m.NextMatch();
            }
            return HrefList;
        }

        public static int GenerateRandomRangeNumber(int minValue, int maxValue)
        {
            Random _random = new Random();
            return _random.Next(minValue, maxValue);
        }

        public static string GenerateRandomStringWithNumeric(int _length)
        {
            Random _random = new Random();
            //string _letters = "abcdefghijklmnoprstuvyzxwq1234567890_";
            string _letters = "ABCDEFGHiJKLMNOPQRSTUVWXYZ1234567890_";
            char[] _buffer = new char[_length];
            for (int i = 0; i < _length; i++)
            {
                _buffer[i] = _letters[_random.Next(_letters.Length)];
            }
            return new string(_buffer);
        }

        public static string GenerateRandomStringOnlyLetters(int _length)
        {
            Random _random = new Random();
            string _letters = "ABCDEFGHiJKLMNOPQRSTUVWXYZ";
            char[] _buffer = new char[_length];
            for (int i = 0; i < _length; i++)
            {
                _buffer[i] = _letters[_random.Next(_letters.Length)];
            }
            return new string(_buffer);
        }

        public static string GenerateRandomStringWithDefinedChars(int _length, string _letters)
        {
            Random _random = new Random();
            char[] _buffer = new char[_length];
            for (int i = 0; i < _length; i++)
            {
                _buffer[i] = _letters[_random.Next(_letters.Length)];
            }
            return new string(_buffer);
        }

        public static string PasswordEncrypt(string cleanString)
        {
            byte[] bytes = new UnicodeEncoding().GetBytes(cleanString);
            return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes));
        }

        /// <summary>
        /// Send SMTP Mail
        /// </summary>
        /// <returns>boolean</returns>
        public static bool SendSmtpMail(string toMail, string toName, string fromMail, string fromName, string subject, string mailbody, bool isHtml,
            string smtpServer, int smtpPort, string smtpUsername, string smtpPassword, bool SSL = false)
        {
            System.Net.Mail.MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(toMail.Trim(), toName, System.Text.Encoding.UTF8));
            mail.From = new MailAddress(fromMail.Trim(), fromName, System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = mailbody;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = isHtml;
            mail.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new System.Net.NetworkCredential(smtpUsername.Trim(), smtpPassword);
            smtp.Port = smtpPort;
            smtp.Host = smtpServer;
            smtp.EnableSsl = SSL;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                return false;
            }


            return true;
        }

        public static byte[] ImageCrop(string Img, double Width, double Height, double X, double Y)
        {
            try
            {
                using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(Img))
                {
                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Convert.ToInt32(Width), Convert.ToInt32(Height)))
                    {
                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                        using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                        {
                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, Convert.ToInt32(Width), Convert.ToInt32(Height)), Convert.ToInt32(X), Convert.ToInt32(Y), Convert.ToInt32(Width), Convert.ToInt32(Height), System.Drawing.GraphicsUnit.Pixel);
                            System.IO.MemoryStream ms = new System.IO.MemoryStream();
                            bmp.Save(ms, OriginalImage.RawFormat);
                            return ms.GetBuffer();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
        }

        public static string WebRequestGetData(string GetURL)
        {
            string ResultData = "";
            try
            {
                System.Net.WebResponse objResponse;
                System.Net.WebRequest objRequest = System.Net.HttpWebRequest.Create(GetURL);
                objRequest.GetResponse();

                objResponse = objRequest.GetResponse();
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    ResultData = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            catch { }

            return ResultData;
        }

        public static string GetGoogleSuggestKeywords(string query, string lang = "tr")
        {
            string result = "";
            WebRequest istek = HttpWebRequest.Create("http://clients1.google.com/complete/search?hl=" + lang + "&output=toolbar&q=" + query);
            WebResponse cevap = istek.GetResponse();
            StreamReader donenBilgiler = new StreamReader(cevap.GetResponseStream(), Encoding.GetEncoding("iso-8859-9"));
            string html = donenBilgiler.ReadToEnd();
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(html);
            XmlNodeList nodeList = xdoc.SelectNodes("toplevel/CompleteSuggestion/suggestion");
            foreach (XmlNode item in nodeList)
            {
                if (result == "")
                    result = item.Attributes["data"].InnerText;
                else
                    result += ";" + item.Attributes["data"].InnerText;

            }
            return result;
        }

        public static bool CheckBirthDateIsLegal(int year, int month, int day)
        {
            if (DateTime.Now.Year - year < 18)
            {
                return false;
            }
            return CheckDateIsValid(year, month, day);
        }

        public static bool CheckDateIsValid(int year, int month, int day)
        {
            try
            {
                DateTime date = new DateTime(year, month, day);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static string StripHTML(string htmlString)
        {
            string pattern = "<(.|\\n)*?>";
            return Regex.Replace(htmlString, pattern, string.Empty);
        }

        /// <summary>
        /// Removes the given parameter from the given querystring
        /// </summary>
        /// <param name="QueryString"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string QueryStringRemoveParameter(string QueryString, string parameter)
        {
            string newQueryString = string.Empty;
            if (QueryString.Length > 0)
            {
                if (QueryString.Substring(0, 1) == "?")
                    QueryString = QueryString.Replace("?", "");

                if (QueryString.IndexOf("&") > -1)
                {
                    string[] degiskenler = QueryString.Split('&');
                    for (int i = 0; i < degiskenler.Length; i++)
                    {
                        if (!(degiskenler[i].Contains(parameter + "=")))
                        {
                            if (string.IsNullOrEmpty(newQueryString))
                                newQueryString = degiskenler[i];
                            else
                                newQueryString += "&" + degiskenler[i];
                        }
                    }
                }
                else
                {
                    if (!(QueryString.Contains(parameter + "=")))
                    {
                        newQueryString = QueryString;
                    }
                }
            }
            return newQueryString;
        }

        public static string SQLinjection(string inputParameter)
        {
            if (!string.IsNullOrEmpty(inputParameter))
            {
                string[,] StrWordList = new string[,]
                {
                    { "SELECT","&#83elect"},
                    { "select","&#83elect"},
                    { "DROP","&#68rop"},
                    { "drop","&#68rop"},
                    { ";","&#59"},
                    { "--","&#45-"},
                    { "INSERT","&#73nsert"},
                    { "insert","&#73nsert"},
                    { "DELETE","&#120p&#95"},
                    { "delete","&#120p&#95"},
                    { "xp_","&#120p&#95"},
                    { "sp_","&#115;&#95"},
                    { "UNION","&#85nion"},
                    { "union","&#85nion"},
                    { "'","&#39"},
                    { "<","&#60;"},
                    { ">","&#62;"}
                };

                for (int i = 0; i < StrWordList.GetLength(0); i++)
                {
                    inputParameter = Regex.Replace(inputParameter, StrWordList[i, 0], StrWordList[i, 1]);
                }
            }
            return inputParameter;
        }

        /// <summary>
        /// Creates a file name by given parameters. Checks if the file exist in directory then adds a number last of filename.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CreateFileName(string folder, string fileName)
        {
            if (System.IO.Directory.Exists(folder))
            {
                int fileNo = 0;
                bool hasFile = true;
                string newfileName = string.Empty;
                while (hasFile)
                {
                    if (fileNo == 0)
                        newfileName = string.Format(Path.GetFileNameWithoutExtension(folder + "/" + fileName) + Path.GetExtension(folder + "/" + fileName));
                    else
                        newfileName = string.Format(Path.GetFileNameWithoutExtension(folder + "/" + fileName) + "-{0}" + Path.GetExtension(folder + "/" + fileName), fileNo);

                    if (File.Exists(folder + "/" + newfileName))
                        fileNo++;
                    else
                        hasFile = false;
                }
                return newfileName;
            }
            else
                return null;
        }

        public static bool CanvasThumbnailImage(string FullPathFileName, string FullPathTarget, int CanvasSize = 300)
        {
            //Resize
            System.Drawing.Image Img = System.Drawing.Image.FromFile(FullPathFileName);
            try
            {
                int ThumbnailSize = CanvasSize;
                int Width = 0;
                int Height = 0;
                if (Img.Width > Img.Height)
                {
                    Width = ThumbnailSize;
                    Height = Img.Height * ThumbnailSize / Img.Width;
                }
                else
                {
                    Width = Img.Width * ThumbnailSize / Img.Height;
                    Height = ThumbnailSize;
                }

                int xPosition = 0;
                int yPosition = 0;
                xPosition = (CanvasSize - Width) / 2;
                yPosition = (CanvasSize - Height) / 2;

                Bitmap Photo = new Bitmap(CanvasSize, CanvasSize);

                Graphics gfx = Graphics.FromImage(Photo);
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.FillRectangle(Brushes.White, 0, 0, CanvasSize, CanvasSize);
                gfx.DrawImage(Img, xPosition, yPosition, Width, Height);

                ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters EncoderParameters = null;
                EncoderParameters = new EncoderParameters(1);
                //EncoderParameters.Param[0] = new EncoderParameter(System.Text.Encoder, 90L);

                Photo.Save(FullPathTarget, Img.RawFormat);

                EncoderParameters.Dispose();
                gfx.Dispose();
                Img.Dispose();
                Photo.Dispose();
            }
            catch
            {
                throw;
            }

            return true;

        }

        /// <summary>
        /// Creates new image by given values
        /// </summary>
        /// <param name="FullPathFileName"></param>
        /// <param name="FullPathTarget"></param>
        /// <param name="CanvasSize">New Size of big side of given image</param>
        /// <returns></returns>
        public static bool CreateImageWithNewSize(string FullPathFileName, string FullPathTarget, int CanvasSize)
        {
            System.Drawing.Image Img = System.Drawing.Image.FromFile(FullPathFileName);
            try
            {
                int Width = 0;
                int Height = 0;
                if (Img.Width > Img.Height)
                {
                    Width = CanvasSize;
                    Height = Img.Height * CanvasSize / Img.Width;
                }
                else
                {
                    Width = Img.Width * CanvasSize / Img.Height;
                    Height = CanvasSize;
                }

                Bitmap Photo = new Bitmap(Width, Height);

                Graphics gfx = Graphics.FromImage(Photo);

                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gfx.CompositingQuality = CompositingQuality.HighQuality;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.DrawImage(Img, 0, 0, Width, Height);

                ImageCodecInfo[] Info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters EncoderParameters = null;
                EncoderParameters = new EncoderParameters(1);

                Photo.Save(FullPathTarget, Img.RawFormat);

                EncoderParameters.Dispose();
                gfx.Dispose();
                Img.Dispose();
                Photo.Dispose();

            }
            catch
            {
                throw;
            }
            return true;
        }

        /// <summary>
        /// Checks is valid postal code or not
        /// </summary>
        /// <param name="inputCode"></param>
        /// <returns></returns>
        public static bool isValidPostalCode(string inputCode)
        {
            string strRegex = @"(^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$)";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputCode))
                return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaCodeDigit"></param>
        /// <param name="pFirst3Digit"></param>
        /// <param name="p4Digit"></param>
        /// <param name="external"></param>
        /// <returns>example : (212)444-1234-ext.12345;</returns>
        public static string GetPhoneNumber(string areaCodeDigit, string pFirst3Digit, string p4Digit, string external)
        {
            if (string.IsNullOrEmpty(areaCodeDigit) || string.IsNullOrEmpty(pFirst3Digit) || string.IsNullOrEmpty(p4Digit)
                || string.IsNullOrEmpty(external))
                return null;
            string phoneNumber = "(" + areaCodeDigit + ")" + pFirst3Digit + "-" + p4Digit;
            if (!string.IsNullOrEmpty(external))
                phoneNumber = phoneNumber + "-ext." + external;
            return phoneNumber;
        }

        public static bool IsIPAddress(string var)
        {
            try
            {
                IPAddress num = System.Net.IPAddress.Parse(var);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsNumeric(string var)
        {
            try
            {
                int num = int.Parse(var);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsAlphaNumeric(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return false;
            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!(char.IsLetter(inputStr[i])) && (!(char.IsNumber(inputStr[i]))))
                    return false;
            }
            return true;
        }

        public static Boolean isAlphaNumericRegEx(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }

        public static bool IsLocalIpAddress(string host)
        {
            try
            {
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                foreach (IPAddress hostIP in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch { }
            return false;
        }

        public static string GetLocalHostName()
        {
            try
            {
                return Dns.GetHostName().ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetIPAddress()
        {
            List<string> ipList = new List<string>();
            IPHostEntry host = default(IPHostEntry);
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    ipList.Add(localIP);
                }
            }
            string ret = string.Join(",", ipList.ToArray());
            return ret;
        }

        public static bool CheckStringOutOfRange(string text, int range)
        {
            if (text.Length > range)
                return false;
            return true;
        }

        public static bool CheckNumberInput(string txt)
        {
            int i = 0;
            return int.TryParse(txt, out i);
        }

        public static bool CheckDecimalInput(string txt)
        {
            decimal i = 0;
            return decimal.TryParse(txt, out i);
        }

    }
}