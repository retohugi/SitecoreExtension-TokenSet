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
            Sitecore.Diagnostics.Log.Info("init QueryToken", this);
            Assert.ArgumentNotNull(args, "args");

            if (args.SourceField.Value.Length == 0 || args.Result.IndexOf("$query", System.StringComparison.Ordinal) < 0)
            {
                return;
            }

            var query = string.Empty;
            var resultFieldname = string.Empty;
            var fieldValue = args.Result;
            
            const string Pattern = @".*\$query\((.*[^\|])\|(\w*).*";
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(fieldValue);

            if (match.Success)
            {
                query = match.Groups[1].Value;
                resultFieldname = match.Groups[2].Value;
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