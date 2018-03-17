using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BrokenLinkChecker
{
    public class CrawlDetail : INotifyPropertyChanged
    {

        private string mWebsiteUrl;
        private int mTotalCrawled;
        private int mTotalBrokenLink;

        private ObservableCollection<CrawlPageDetail> mPages = new ObservableCollection<CrawlPageDetail>();

        

        public ObservableCollection<CrawlPageDetail> Pages
        {
            get { return mPages; }
            set
            {
                mPages = value;
                OnPropertyChanged("Pages");
            }
        }



        public string WebsiteUrl
        {
            get { return mWebsiteUrl; }
            set
            {
                mWebsiteUrl = value;
                OnPropertyChanged("WebsiteUrl");
            }
        }

        public int TotalCrawled
        {
            get { return mTotalCrawled; }
            set
            {
                mTotalCrawled = value;
                OnPropertyChanged("TotalCrawled");
            }
        }

        public int TotalBrokenLink
        {
            get { return mTotalBrokenLink; }
            set
            {
                mTotalBrokenLink = value;
                OnPropertyChanged("TotalBrokenLink");
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
namespace BrokenLinkChecker
{

    public class CrawlPageDetail : INotifyPropertyChanged
    {

        private string mWebsiteUrl;
        private int mSerialNumber;
        private string mStatus;

        private bool mIsSuccess;

       

        public string WebsiteUrl
        {
            get { return mWebsiteUrl; }
            set
            {
                mWebsiteUrl = value;
                OnPropertyChanged("WebsiteUrl");
            }
        }

        public bool IsSuccess
        {
            get { return mIsSuccess; }
            set
            {
                mIsSuccess = value;
                OnPropertyChanged("IsSuccess");
            }
        }


        public int SerialNumber
        {
            get { return mSerialNumber; }
            set
            {
                mSerialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        public string Status
        {
            get { return mStatus; }
            set
            {
                mStatus = value;
                OnPropertyChanged("Status");
            }
        }

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}


