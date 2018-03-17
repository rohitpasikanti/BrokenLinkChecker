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

namespace BrokenLinkChecker
{
    public class RegExUtil
    {
        #region "fields"
        #endregion
        private static Regex[] m_standardRegularExpressions;


        /// <summary> Class cannot be instantiated</summary>
        private RegExUtil()
        {
        }

        /// <summary> 
        /// Get a predefined regular expression
        /// </summary>
        /// <param name="regularExpressionId">Id of the regular expression to return
        /// </param>
        /// <returns>RegEx</returns>
        public static Regex GetRegEx(RegularExpression regularExpressionId)
        {
            if (m_standardRegularExpressions == null)
            {
                m_standardRegularExpressions = new Regex[Enum.GetNames(typeof(RegularExpression)).Length];
            }

            int index = Convert.ToInt32(regularExpressionId);

            if (m_standardRegularExpressions[index] == null)
            {
                m_standardRegularExpressions[index] = StandardRegularExpression(regularExpressionId);
            }

            return m_standardRegularExpressions[index];
        }

        /// <summary> 
        /// Get a match object based on a predefined regular expression
        /// </summary>
        /// <param name="regularExpressionId">Id of the regular expression to return</param>
        /// <param name="text">Text to match on</param>
        /// <returns>Match</returns>
        public static Match GetMatchRegEx(RegularExpression regularExpressionId, string text)
        {
            return GetRegEx(regularExpressionId).Match(text);
        }

        private static Regex StandardRegularExpression(RegularExpression regularExpressionId)
        {

            switch (regularExpressionId)
            {
                case RegularExpression.UrlExtractor:
                    if (true)
                    {
                        // Refer to http://www.standardio.org/article.aspx?id=173 for help
                        return new Regex("(?:href\\s*=)(?:[\\s\"']*)(?!#|mailto|location.|javascript|.*css|.*this\\.)(?<url>.*?)(?:[\\s>\"'])", RegexOptions.IgnoreCase);
                    }
                    break;
                case RegularExpression.SrcExtractor:
                    if (true)
                    {
                        return new Regex("(?:src\\s*=)(?:[\\s\"']*)(?<url>.*?)(?:[\\s>\"'])", RegexOptions.IgnoreCase);
                    }
                    break;
            }

            return null;
        }

    }
}
namespace BrokenLinkChecker
{

    public enum RegularExpression
    {
        UrlExtractor,
        SrcExtractor
    }
}