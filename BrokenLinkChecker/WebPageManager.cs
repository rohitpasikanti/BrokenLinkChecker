using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;


namespace BrokenLinkChecker
{
    public delegate void WebPageContentDelegate(WebPageStatus state);
}
namespace BrokenLinkChecker
{
        
    

    class WebPageManager
    {
        public bool Process(WebPageStatus state)
        {
            state.TaskStarted = true;
            state.TaskCompleted = false;

            try
            {
                Console.WriteLine("Process Uri: {0}", state.Uri.AbsoluteUri);

                WebRequest req = WebRequest.Create(state.Uri);
                WebResponse res = null;

                try
                {
                    res = req.GetResponse();

                    if (res is HttpWebResponse)
                    {
                        state.StatusCode = ((HttpWebResponse)res).StatusCode.ToString();
                        state.StatusDescription = ((HttpWebResponse)res).StatusDescription;
                    }

                    if (res is FileWebResponse)
                    {
                        state.StatusCode = "OK";
                        state.StatusDescription = "OK";
                    }

                    if (state.StatusCode.Equals("OK"))
                    {
                        StreamReader sr = new StreamReader(res.GetResponseStream());

                        state.Content = sr.ReadToEnd();

                        if ((WebPageContentHandler != null))
                        {
                            WebPageContentDelegate handler = WebPageContentHandler;
                            handler(state);
                        }
                    }

                    state.TaskCompleted = true;
                }
                catch (Exception ex)
                {
                    HandleException(ex, ref state);
                }
                finally
                {
                    if ((res != null))
                    {
                        res.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Completed: {0}", state.TaskCompleted);

            if (WebPageTaskCompleted != null)
            {
                WebPageContentDelegate taskHandler = WebPageTaskCompleted;
                taskHandler(state);
            }


            return state.TaskCompleted;
        }
        //Process

        #region "local interface"

        //
        private void HandleException(Exception ex, ref WebPageStatus state)
        {
            if (ex.ToString().IndexOf("(404)") != -1)
            {
                state.StatusCode = "404";
                state.StatusDescription = "(404) Not Found";
            }
            else if (ex.ToString().IndexOf("(403)") != -1)
            {
                state.StatusDescription = "(403) Forbidden";
            }
            else if (ex.ToString().IndexOf("(500)") != -1)
            {
                state.TaskCompleted = true;
                state.StatusCode = "OK";
                state.StatusDescription = "(500) Internal Server Error";
            }
            else if (ex.ToString().IndexOf("(502)") != -1)
            {
                state.StatusCode = "502";
                state.StatusDescription = "(502) Bad Gateway";
            }
            else if (ex.ToString().IndexOf("(503)") != -1)
            {
                state.StatusCode = "503";
                state.StatusDescription = "(503) Server Unavailable";
            }
            else if (ex.ToString().IndexOf("(504)") != -1)
            {
                state.StatusCode = "504";
                state.StatusDescription = "(504) Gateway Timeout";
            }
            else if ((ex.InnerException != null) && ex.InnerException is FileNotFoundException)
            {
                state.StatusCode = "FileNotFound";
                state.StatusDescription = ex.InnerException.Message;
            }
            else
            {
                state.StatusDescription = ex.ToString();
            }
        }
        //HandleException
        #endregion

        #region "properties"
        private WebPageContentDelegate mWebPageContentHandler = null;

        private WebPageContentDelegate mWebPageTaskCompleted = null;
        public WebPageContentDelegate WebPageContentHandler
        {
            get { return mWebPageContentHandler; }
            set { mWebPageContentHandler = value; }
        }

        public WebPageContentDelegate WebPageTaskCompleted
        {
            get { return mWebPageTaskCompleted; }
            set { mWebPageTaskCompleted = value; }
        }

        #endregion

    }
}
