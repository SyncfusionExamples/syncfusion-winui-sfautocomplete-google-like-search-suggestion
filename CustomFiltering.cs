using Syncfusion.UI.Xaml.Core;
using Syncfusion.UI.Xaml.Editors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoogleSearchDemo
{
    public class CustomFiltering : NotificationObject, IAutoCompleteFilterBehavior
    {
        public CustomFiltering()
        {
            GetGoogleSuggestions("test");
        }

        public Task<object> GetMatchingItemsAsync(SfAutoComplete source, AutoCompleteFilterInfo filterInfo)
        {
            return GetGoogleSuggestions(filterInfo.Text);
        }

        private async Task<object> GetGoogleSuggestions(string query)
        {
            if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query))
            {
                return new List<string>();
            }

            string xmlSuggestions;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var searchQuery = String.Format("https://www.google.com/complete/search?output=toolbar&q={0}", query);
                    xmlSuggestions = await client.GetStringAsync(searchQuery);
                }
                catch
                {
                    return null;
                }
            }

            XDocument doc = XDocument.Parse(xmlSuggestions);
            var suggestions = doc.Descendants("CompleteSuggestion")
                                    .Select(
                                        item => item.Element("suggestion").Attribute("data").Value);

            return suggestions.ToList();
        }
    }
}
