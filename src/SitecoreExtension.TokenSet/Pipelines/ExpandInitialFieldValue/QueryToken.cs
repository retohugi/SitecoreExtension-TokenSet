namespace SitecoreExtension.TokenSet.Pipelines.ExpandInitialFieldValue
{
    using System;
    using System.Text.RegularExpressions;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.ExpandInitialFieldValue;

    /// <summary>
    /// Processor to execute queries during token replacement in standard values.
    /// </summary>
    public class QueryToken : ExpandInitialFieldValueProcessor
    {
        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(ExpandInitialFieldValueArgs args)
        {
            Assert.ArgumentNotNull(args, "args");



            if (args.SourceField.Value.Length == 0 || args.SourceField.Value.IndexOf('$') < 0)
            {
                return;
            }


            string query = string.Empty;
            string resultFieldname = string.Empty;
            string fieldValue = args.Result;
            

            if (fieldValue.IndexOf("$query", System.StringComparison.Ordinal) > -1)
            {
                string pat = @".*\$query\((.*);(\w*).*";
                var r = new Regex(pat, RegexOptions.IgnoreCase);

                var m = r.Match(fieldValue);
                if (m.Success)
                {
                    query = m.Groups[1].Value;
                    resultFieldname = m.Groups[2].Value;
                }

                if (query.Length > 0 && resultFieldname.Length > 0)
                {
                    try
                    {
                        var queryResultItem = args.TargetItem.Axes.SelectSingleItem(query);
                        if (queryResultItem != null)
                        {
                            args.Result = queryResultItem[resultFieldname];
                        }
                    }
                    catch (Exception)
                    {  
                        Sitecore.Diagnostics.Log.Error("Failed to execute query [" + query + "]", this);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Warn("Failed to start replacing values because either query or fieldname are empty.", this);
                }
            }
        }
    }
}