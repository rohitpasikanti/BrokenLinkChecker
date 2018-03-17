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
using System.IO;
using System.Reflection;


namespace BrokenLinkChecker
{
    class CommonFunctions
    {

        /// <summary> Class cannot be instantiated</summary>
		private CommonFunctions()
        {
        }

        /// <summary> Empty String tests a String to see if it is null or empty.
        /// </summary>
        /// <param name="value">String to be tested.
        /// </param>
        /// <returns> boolean true if empty.
        /// </returns>
        public static bool EmptyString(string value)
        {
            return (value == null || value.Trim().Length == 0);
        }

        /// <summary> Empty HttpCookie tests a HttpCookie to see if it is null or empty.
        /// </summary>
        /// <param name="value">HttpCookie to be tested.
        /// </param>
        /// <returns> boolean true if empty.
        /// </returns>
        public static bool EmptyHttpCookie(System.Web.HttpCookie value)
        {
            return (value == null || value.Value == null || value.Value.Trim().Length == 0);
        }

      

    }
}
