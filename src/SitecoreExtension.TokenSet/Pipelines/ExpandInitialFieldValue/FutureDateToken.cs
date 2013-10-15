namespace SitecoreExtension.TokenSet.Pipelines.ExpandInitialFieldValue
{
    using System;
    using System.Text.RegularExpressions;

    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.ExpandInitialFieldValue;


    /// <summary>
    /// Pipeline processor for the ExpandInitialFieldValue pipeline that allows me to
    /// set a date in the future for an item.
    /// </summary>
    public class FutureDateToken : ExpandInitialFieldValueProcessor
    {
        /// <summary>
        /// Processes the specified args.
        /// </summary>
        /// <remarks>
        /// The format of the replacement string is the following:
        /// $futureDate(years,months,days)
        /// where:
        /// - years = years to add
        /// - months = months to add
        /// - days = days to add
        /// For example:
        /// $futureDate(1,0,0)   = Adds DateTime.Now + 1 year
        /// $futureDate(0,6,0)   = Adds DateTime.Now + 6 months
        /// $futureDate(1,6,12)  = Adds DateTime.Now + 1 year, 6 months, 12 days
        /// </remarks>
        /// <param name="args">The args.</param>
        public override void Process(ExpandInitialFieldValueArgs args)
        {
            Sitecore.Diagnostics.Log.Info("init futureDateToken", this);
            Assert.ArgumentNotNull(args, "args");

            if (args.SourceField.Value.Length == 0 || args.Result.IndexOf("$futureDate", System.StringComparison.Ordinal) < 0)
            {
                return;
            }

            var query = string.Empty;
            var resultFieldname = string.Empty;
            var fieldValue = args.Result;

            const string Pattern = @"\$futureDate\((\d),(\d),(\d)\)";
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);
            var match = regex.Match(fieldValue);

            if (match.Success)
            {
                var years = match.Groups[1].Value;
                var months = match.Groups[2].Value;
                var days = match.Groups[3].Value;

                args.Result =
                    DateUtil.ToIsoDate(
                        DateTime.Now.AddYears(this.GetInt(years, 0))
                                .AddMonths(this.GetInt(months, 0))
                                .AddDays(this.GetInt(days, 0)));
            }
            else
            {
                Sitecore.Diagnostics.Log.Error(
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