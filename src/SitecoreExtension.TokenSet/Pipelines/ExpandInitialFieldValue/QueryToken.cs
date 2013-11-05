namespace SitecoreExtension.TokenSet.Pipelines.ExpandInitialFieldValue
{
    using System;
    using System.Text.RegularExpressions;

    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.ExpandInitialFieldValue;

    using Debug = System.Diagnostics.Debug;

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

            if (args.SourceField.Value.Length == 0 || args.Result.IndexOf("$query", StringComparison.OrdinalIgnoreCase) < 0)
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
                var fieldname = match.Groups[3].Value;

                if (query.Length > 0 && fieldname.Length > 0)
                {
                    var resultItem = this.GetItemFromRelativeQuery(query, args.TargetItem);
                    if (resultItem != null)
                    {
                        args.Result = string.IsNullOrEmpty(resultItem[fieldname]) ? 
                            args.Result.Replace(token, string.Empty) : 
                            args.Result.Replace(token, resultItem[fieldname]);
                    }
                }
            }
            else
            {
                Log.Error("Invalid $query() token. Expected format: $query(<query>|<fieldname>)", this);
            }
        }

        /// <summary>
        /// Gets the item from a query relativ to the provided item. Fastquery and Sitecore queries are supported.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="item">The Sitecore item.</param>
        /// <returns>the matching item or null</returns>
        private Item GetItemFromRelativeQuery(string query, Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            try
            {
                var queryResultItem = query.StartsWith("fast:", StringComparison.OrdinalIgnoreCase) ? 
                    item.Database.SelectSingleItem(query) : 
                    Sitecore.Data.Query.Query.SelectItems(query, item)[0];
                return queryResultItem;
            }
            catch (Exception)
            {
                Log.Error("Failed to execute query [" + query + "]", this);
                return null;
            }
        }
    }
}