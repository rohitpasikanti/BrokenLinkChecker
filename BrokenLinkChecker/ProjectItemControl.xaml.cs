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
using System.Threading;
using System.Web;

namespace BrokenLinkChecker
{
    /// <summary>
    /// Interaction logic for ProjectItemControl.xaml
    /// </summary>
    public partial class ProjectItemControl : UserControl
    {


        CrawlDetail mCrawlDetail = new CrawlDetail();
        string mWebsiteContent;
        public string ProjectName { get; set; }

        public ProjectItemControl()
        {
            InitializeComponent();
            this.DataContext = mCrawlDetail;
            ResultDataGrid.AutoGenerateColumns = false;

        }

        private void btnStartSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mCrawlDetail = new CrawlDetail();
                this.DataContext = mCrawlDetail;

                string url = txtSearchUrl.Text;
                MyBrowser.Navigate(url);

                tbkCurrentStatus.Text = "Running";
                Thread th = new Thread(new ThreadStart(() =>
                {
                    WebCrawler spider = new WebCrawler(url, url, 100);
                //spider.WebPageManager.WebPageContentHandler = [Delegate].Combine(spider.WebPageManager.WebPageContentHandler, New WebPageContentDelegate(AddressOf spider_WebPageContentHandler))
                spider.WebPageManager.WebPageTaskCompleted = new WebPageContentDelegate(spider_WebPageContentHandler);
                    spider.Execute();
                    var lst = spider.WebPages.Values;
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        tbkCurrentStatus.Text = "Completed";

                        CrawlerDetail result = new CrawlerDetail();
                        result.ProjectName = ProjectName;
                        result.CrawlDate = DateAndTime.Now;
                        result.WebsiteUrl = url;
                        result.TotalCrawled = mCrawlDetail.TotalCrawled;
                        result.BrokenLinks = mCrawlDetail.TotalBrokenLink;
                        CrawlerQueries.SaveCrawlerDetail(result);

                    }));
                }));
                th.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }



        private void spider_WebPageContentHandler(WebPageStatus state)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                CrawlPageDetail detail = new CrawlPageDetail();
                detail.WebsiteUrl = state.Uri.ToString() + "";
                detail.SerialNumber = mCrawlDetail.Pages.Count + 1;
                detail.Status = state.StatusCode;
                mCrawlDetail.Pages.Add(detail);

                mCrawlDetail.TotalCrawled += 1;
                if (state.TaskCompleted == false && state.TaskStarted == true)
                {
                    detail.IsSuccess = false;
                    mCrawlDetail.TotalBrokenLink += 1;
                }
                else
                {
                    detail.IsSuccess = true;
                }

                Uri homeUri = new Uri(txtSearchUrl.Text);
                if (detail.IsSuccess == true && state.OriginalUrl.ToLowerInvariant() == homeUri.ToString().ToLowerInvariant())
                {
                    mWebsiteContent = state.Content;
                }

                if (detail.IsSuccess == false)
                {
                    UpdateWebsiteBrowser(state.Uri.ToString());
                }

            }));
        }

        private void UpdateWebsiteBrowser(string link)
        {
            
            int i = 0;
            if (MyBrowser.Document != null)
            {
                for (i = 0; i <= ((dynamic)MyBrowser.Document).links.length - 1; i++)
                {
                    //If MyBrowser.Document.links.Item(i).href = link Then

                    if (string.Compare(HttpUtility.HtmlDecode(((dynamic)MyBrowser.Document).links.Item(i).href), HttpUtility.HtmlDecode(link), true) == 0)
                    {
                        ((dynamic)MyBrowser.Document).links.Item(i).Style.backgroundColor = "red";
                        //Dim innerHtml = MyBrowser.Document.links.Item(i).innerHtml
                        //MyBrowser.Document.links.Item(i).innerHtml = "<div style='background-color:darkmagenta;'>" + innerHtml + "</div>"
                    }
                }
            }

        }

        private void btnGoToHome_Click(object sender, RoutedEventArgs e)
        {
            var win = new System.Windows.Window();
            win.Content = new HomeControl();
            win.Show();
        }
    }
}
