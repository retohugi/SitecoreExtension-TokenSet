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

            if (args.SourceField.Value.Length == 0 || args.Result.IndexOf("$query", StringComparison.Ordinal) < 0)
            {
                return;
            }

            var fieldValue = args.Result;
            
            const string Pattern = @"(\$query\((.*[^\|])\|(\w*)\))";
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(fieldValue);

            if (match.Success)
            {
                var token = match.Groups[1].Value;
                var query = match.Groups[2].Value;
                var resultFieldname = match.Groups[3].Value;


                if (query.Length > 0 && resultFieldname.Length > 0)
                {
                    try
                    {
                        var queryResultItem = args.TargetItem.Axes.SelectSingleItem(query);

                        if (queryResultItem != null)
                        {
                            args.Result = args.Result.Replace(token, queryResultItem[resultFieldname]);
                        }
                    }
                    catch (Exception)
                    {
                        Log.Error("Failed to execute query [" + query + "]", this);
                    }
                }
            }
            else
            {
                Log.Warn("Invalid $query() token", this);
            }
        }
    }
}