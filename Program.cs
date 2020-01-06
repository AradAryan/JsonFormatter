using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JsonFormatter
{
    class Program
    {
        #region Fields 
        /// <summary>
        /// Get First Json File Directory
        /// </summary>
        static private string firstDirectory = "http://localhost/GetJson.json";

        /// <summary>
        /// Get Second Json File Directory For Making Changes on it
        /// </summary>
        static private string secondDirectory = @"C:\Users\javad\OneDrive\Desktop\Context.txt";
        //  @"C:\Users\faranam\Desktop\JsonFormatter\JsonFormatter\OutPut\FormattedJson.json";
        #endregion Fiels

        #region Sending HTTP Request
        /// <summary>
        ///Send Get Request to Specified URL and Return Context OF URL
        /// </summary>
        static private string SendHttpRequest(string url)
        {
            HttpWebRequest webRequest =
                (HttpWebRequest)WebRequest.Create(url);
            //exception Handling
            try
            {
                HttpWebResponse webResponse =
                (HttpWebResponse)webRequest.GetResponse();

                Stream responseStream =
                    webResponse.GetResponseStream();

                StreamReader streamReader =
                    new StreamReader(responseStream);

                string context = streamReader.ReadToEnd();
                return context;
            }
            catch (Exception)
            {
                return "404";
            }
        }
        #endregion Sending HTTP Request

        #region Show as Default View
        /// <summary>
        /// Write Json File in Console Screen Without AnyChanges
        /// </summary>
        static private void ShowNormal()
        {
            Console.Write(File.ReadAllText(firstDirectory));
        }
        #endregion Show as Default View

        #region Show Well Formatting
        /// <summary>
        /// Store Json As Well Formatted File
        /// </summary>
        static void ShowWellFormatting()
        {
            //Checking File Existence
            string context =
                SendHttpRequest(firstDirectory);
            if (context == "404")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Requested File Not Found #Error Code 404");
                return;
            }

            var output = new StringBuilder();
            var tabcount = 0;
            int temp = context.Length;
            for (int i = 0; i < temp; i++)
            {

                // output.Append(context.First());
                // output.Append(Environment.NewLine);
                // output.Append('\t', tabcount);
                if (context.StartsWith("{") || context.StartsWith("["))
                {
                    if (context.StartsWith("["))
                    {
                        output.Append(" ");
                    }
                    output.Append(context.First());
                    tabcount++;
                    context = context.Remove(0, 1);
                    output.Append(Environment.NewLine);
                    output.Append('\t', tabcount);
                    continue;
                }

                if (context.StartsWith("}") || context.StartsWith("]"))
                {
                    output.Append(Environment.NewLine);
                    tabcount--;
                    output.Append('\t', tabcount);
                    output.Append(context.First());
                    context = context.Remove(0, 1);
                    continue;
                }

                if (context.StartsWith(","))
                {
                    output.Append(context.First());
                    context = context.Remove(0, 1);
                    output.Append(Environment.NewLine);
                    output.Append('\t', tabcount);
                    continue;
                }
                else
                {
                    if (context.Length == 0)
                    {
                        //Console.WriteLine(i);
                        break;
                    }
                    else
                    {
                        output.Append(context.First());
                        context = context.Remove(0, 1);
                    }
                }
                //Console.WriteLine(i);
            }
            File.WriteAllText(secondDirectory, output.ToString());
            //Console.WriteLine(output.ToString());
        }
        #endregion Show Well Formatting

        #region Main
        static void Main(string[] args)
        {
            ShowWellFormatting();

            #region Ending
            Console.WriteLine("Jobs Done");
            Console.ReadKey();
            #endregion Ending
        }
        #endregion Main
    }
}
