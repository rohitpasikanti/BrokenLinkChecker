using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;
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
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Mf.Util;

namespace BrokenLinkChecker
{
    class WebCrawler
    {
        private Uri mStartUri;
        private Uri mBaseUri;
        private int mMaximumUrlAllowed;
        private int mUrlCrawledCount;

        private bool mKeepWebContent;
        private Queue m_webPagesPending;
        private Hashtable mWebPages;

        private WebPageManager mWebPageManager;
        //Private Shared mValidExtensions() As String = {"html", "aspx", "php", "asp", "htm", "jsp", "shtml"}
        private static string[] mValidExtensions = {
            "html",
            "php",
            "asp",
            "htm",
            "jsp",
            "shtml",
            "php3",
            "aspx",
            "pl",
            "cfm",
            "/"
        };
        //

        public WebPageManager WebPageManager
        {
            get { return mWebPageManager; }
            set { mWebPageManager = value; }
        }

        public Uri StartUri
        {
            get { return mStartUri; }
            set { mStartUri = value; }
        }

        public Uri BaseUri
        {
            get { return mBaseUri; }
            set { mBaseUri = value; }
        }

        private int UrlCrawledCount
        {
            get { return mUrlCrawledCount; }
            set { mUrlCrawledCount = value; }
        }

        public int MaximumUrlAllowed
        {
            get { return mMaximumUrlAllowed; }
            set { mMaximumUrlAllowed = value; }
        }

        public bool KeepWebContent
        {
            get { return mKeepWebContent; }
            set { mKeepWebContent = value; }
        }

        public Hashtable WebPages
        {
            get { return mWebPages; }
            set { mWebPages = value; }
        }

        private Queue WebPagesPending
        {
            get { return m_webPagesPending; }
            set { m_webPagesPending = value; }
        }


        public WebCrawler(string startUri) : this(startUri, -1)
        {
        }
        //New

        public WebCrawler(string startUri, int maximumUrlAllowed) : this(startUri, "", maximumUrlAllowed, false, new WebPageManager())
        {
        }
        //New

        public WebCrawler(string startUri, string baseUri, int maximumUrlAllowed) : this(startUri, baseUri, maximumUrlAllowed, false, new WebPageManager())
        {
        }
        //New


        public WebCrawler(string startUri, string baseUri, int maximumUrlAllowed, bool keepWebContent, WebPageManager webPageManager)
        {
            this.StartUri = new Uri(startUri);

            // In future this could be null and will process cross-site, but for now must exist
            if ((baseUri == null || baseUri.Trim().Length == 0))
            {
                this.BaseUri = new Uri(this.StartUri.GetLeftPart(UriPartial.Authority));
            }
            else
            {
                this.BaseUri = new Uri(baseUri);
            }

            this.MaximumUrlAllowed = maximumUrlAllowed;
            this.KeepWebContent = keepWebContent;

            m_webPagesPending = new Queue();
            mWebPages = new Hashtable();

            mWebPageManager = webPageManager;
            webPageManager.WebPageContentHandler = (WebPageContentDelegate)Delegate.Combine(webPageManager.WebPageContentHandler, new WebPageContentDelegate(this.HandleLinks));
        



    }
        //New

        public void Execute()
        {
            UrlCrawledCount = 0;

            DateTime startTime = DateTime.Now;

            AddWebPage(StartUri, StartUri.AbsoluteUri);

            try
            {
                while (WebPagesPending.Count > 0 && (MaximumUrlAllowed == -1 || UrlCrawledCount < MaximumUrlAllowed))
                {
                    WebPageStatus state = (WebPageStatus)m_webPagesPending.Dequeue();
                    mWebPageManager.Process(state);
                    if (!KeepWebContent)
                    {
                        state.Content = null;
                    }
                    UrlCrawledCount += 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was some error in crawling the website. Try again later." + Constants.vbCrLf + "Error:" + ex.ToString());
            }

            DateTime endTime = DateTime.Now;
            float elasped = (endTime.Ticks - startTime.Ticks) / 10000000;
            var diffTimeInSeconds = (endTime - startTime).TotalMilliseconds;
            var diffTimeInMiliiSeconds = (endTime.Ticks - startTime.Ticks) / 10000;

        }


        public void HandleLinks(WebPageStatus state)
        {
            if (state.TaskInformation != null && !(state.TaskInformation.IndexOf("Handle Links") == -1))
            {
                int counter = 0;
                Match m = RegExUtil.GetMatchRegEx(RegularExpression.UrlExtractor, state.Content);
                while (m.Success)
                {
                    if (AddWebPage(state.Uri, m.Groups["url"].ToString()))
                    {
                        counter += 1;
                    }
                    m = m.NextMatch();
                }

            }
        }


        private bool AddWebPage(Uri l_baseUri, string newUri)
        {
            // Dim url As String = StrUtil.LeftIndexOf(newUri, "#")


            // Dim uri As New Uri(l_baseUri, url)
            Uri uri = new Uri(l_baseUri, newUri);

            if (!ValidPage(uri.LocalPath) || mWebPages.Contains(uri))
            {
                return false;
            }
            WebPageStatus state = new WebPageStatus(uri);
            state.OriginalUrl = newUri;

            if ((uri.AbsoluteUri.StartsWith(BaseUri.AbsoluteUri)))
            {
                state.TaskInformation += "Handle Links";
            }

            m_webPagesPending.Enqueue(state);
            mWebPages.Add(uri, state);

            return true;
        }


        private bool ValidPage(string path)
        {
            int pos = path.IndexOf(".");

            //.ToString( ).Equals( "/" )
          

            string uriExt = StrUtil.RightOf(path, pos).ToLower();

            // Uri ends in an extension
            string ext = null;
            foreach (string ext_loopVariable in mValidExtensions)
            {
                ext = ext_loopVariable;
                if (uriExt.Equals(ext))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
