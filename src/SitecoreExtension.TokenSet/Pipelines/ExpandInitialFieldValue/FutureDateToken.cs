namespace SitecoreExtension.TokenSet.Pipelines.ExpandInitialFieldValue
{
    using System;

    using Sitecore;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.ExpandInitialFieldValue;


    /// <summary>
    /// Pipeline processor for the ExpandInitialFieldValue pipeline that allows me to
    /// set a date in the future for an item.
    /// </summary>
    public class ReplaceDateInFuture : ExpandInitialFieldValueProcessor
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
            Assert.ArgumentNotNull(args, "args");
            if (args.Result.ToLower().StartsWith("$futuredate"))
            {
                string[] yymmdd = args.Result.ToLower().Replace("$futuredate(", "").Replace(")", "").Split(',');
                if (yymmdd.Length != 3)
                {
                    throw new Exception("Invalid $futureDate format. Expected: $futureDate(years,months,days) (for example $futureDate(1,0,0)). Found: " + args.Result);
                }

                args.Result = DateUtil.ToIsoDate(DateTime.Now.AddYears(this.GetInt(yymmdd[0], 0)).AddMonths(this.GetInt(yymmdd[1], 0)).AddDays(this.GetInt(yymmdd[2], 0)));
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