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
using System.Net;
namespace BrokenLinkChecker
{

    public class WebPageStatus
    {

        private WebPageStatus()
        {
        }

        public WebPageStatus(Uri uri)
        {
            this.Uri = uri;
        }

        public WebPageStatus(string uri) : this(new Uri(uri))
        {
        }

        public string OriginalUrl { get; set; }

        public Uri Uri { get; set; }
        public bool TaskStarted { get; set; }
        public bool TaskCompleted { get; set; }
        public string TaskInformation { get; set; }
        public string Content { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }

    }
}
