namespace SitecoreExtension.TokenSet.Pipelines.ExpandInitialFieldValue
{
    using System;
    using System.Text.RegularExpressions;

    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.ExpandInitialFieldValue;

    /// <summary>
    /// Pipeline processor for the ExpandInitialFieldValue pipeline that allows to
    /// set a date in the future for an item. (Idea and parts of the code by https://twitter.com/briancaos)
    /// </summary>
    public class FutureDateToken : ExpandInitialFieldValueProcessor
    {
        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        public override void Process(ExpandInitialFieldValueArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.SourceField.Value.Length == 0 || args.Result.IndexOf("$futureDate", System.StringComparison.OrdinalIgnoreCase) < 0)
            {
                return;
            }

            var fieldValue = args.Result;

            const string Pattern = @"(\$futureDate\((\d),(\d),(\d)\))";
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(fieldValue);

            if (match.Success)
            {
                var token = match.Groups[1].Value;
                var years = match.Groups[2].Value;
                var months = match.Groups[3].Value;
                var days = match.Groups[4].Value;

                args.Result = fieldValue.Replace(
                    token,
                    DateUtil.ToIsoDate(
                        DateTime.Now.AddYears(this.GetInt(years, 0))
                                .AddMonths(this.GetInt(months, 0))
                                .AddDays(this.GetInt(days, 0))));
            }
            else
            {
                Sitecore.Diagnostics.Log.Warn(
                    "Invalid $futureDate format. Expected: $futureDate(years,months,days) (for example $futureDate(1,0,0)).",this);
            }
        }

        /// <summary>
        /// Safe string to int conversion
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        /// <param name="defaultValue">
        /// The default Value.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int GetInt(string s, int defaultValue)
        {
            int val;
            return int.TryParse(s, out val) ? val : defaultValue;
        }
    }
}