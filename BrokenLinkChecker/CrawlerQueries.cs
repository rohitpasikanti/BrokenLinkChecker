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
using System.Collections.ObjectModel;
using System.Text;
using System.Data.SQLite;
using System.Data;
using MySql.Data.Common;
using MySql.Data.MySqlClient;

namespace BrokenLinkChecker
{
    public class CrawlerDetail
    {

        public string ProjectName { get; set; }
        public string WebsiteUrl { get; set; }
        public int BrokenLinks { get; set; }
        public int TotalCrawled { get; set; }
        public System.DateTime CrawlDate { get; set; }
        public int CrawlID { get; set; }

    }
}

namespace BrokenLinkChecker
{

    public class CrawlerQueries
    {


        private static string ConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=Mydb";
        
        
        public static void SaveCrawlerDetail(CrawlerDetail detail)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("INSERT INTO CrawlerDetail (WebsiteUrl, BrokenLinks, TotalCrawled, CrawlDate) VALUES (@WebsiteUrl, @BrokenLinks, @TotalCrawled, @CrawlDate)");

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query.ToString(), conn))
                {
                    cmd.Parameters.AddWithValue("@WebsiteUrl", detail.WebsiteUrl);
                    
                    cmd.Parameters.AddWithValue("@BrokenLinks", detail.BrokenLinks);
                    cmd.Parameters.AddWithValue("@TotalCrawled", detail.TotalCrawled);
                    cmd.Parameters.AddWithValue("@CrawlDate", detail.CrawlDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static Collection<CrawlerDetail> GetCrawlerDetails(string websiteUrl)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT RowID, CrawlDate FROM CrawlerDetail WHERE WebsiteUrl=@WebsiteUrl");
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlDataAdapter dap = new MySqlDataAdapter(query.ToString(), conn);
                dap.SelectCommand.Parameters.AddWithValue("@WebsiteUrl", websiteUrl);
                dap.Fill(dt);
            }

            Collection<CrawlerDetail> details = new Collection<CrawlerDetail>();
            foreach (DataRow row in dt.Rows)
            {
                CrawlerDetail item = new CrawlerDetail();
                System.DateTime dat = default(System.DateTime);
                if (System.DateTime.TryParse(row["CrawlDate"] + "", out dat) == true)
                {
                    item.CrawlDate = dat;
                }
                item.CrawlID = int.Parse(row["RowID"] + "");
            }

            return details;
        }

        public static Collection<CrawlerDetail> GetCrawlerDetails()
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT RowID, * FROM CrawlerDetail ORDER BY CrawlDate DESC");
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                MySqlDataAdapter dap = new MySqlDataAdapter(query.ToString(), conn);
                dap.Fill(dt);
            }

            Collection<CrawlerDetail> details = new Collection<CrawlerDetail>();
            foreach (DataRow row in dt.Rows)
            {
                CrawlerDetail item = new CrawlerDetail();
                System.DateTime dat = default(System.DateTime);
                if (System.DateTime.TryParse(row["CrawlDate"] + "", out dat) == true)
                {
                    item.CrawlDate = dat;
                }
                item.CrawlID = int.Parse(row["RowID"] + "");
                int d = 0;
                if (int.TryParse(row["TotalCrawled"] + "", out d) == true)
                {
                    item.TotalCrawled = d;
                }
                if (int.TryParse(row["BrokenLinks"] + "", out d) == true)
                {
                    item.BrokenLinks = d;
                }

                item.WebsiteUrl = row["WebsiteUrl"] + "";
                
                details.Add(item);
            }

            return details;
        }

    }
}