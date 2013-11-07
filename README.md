# Token Set - a Sitecore Extension

This extension provides additional standard value tokens for Sitecore Data Templates. Read John West's blog post for an [introduction on Standard Value Tokens](http://www.sitecore.net/Community/Technical-Blogs/John-West-Sitecore-Blog/Posts/2012/05/Expand-Standard-Values-Tokens-in-Existing-Items-with-the-Sitecore-ASPNET-CMS.aspx).

Well, it might be a bit exaggerated to call two extensions a "set", but I'm planning to add some more. Have an idea for a cool token? Let me know or even better send a pull request. :)


## Available Tokens
<table>
<tr>
  <th>Token</th>
  <th>Description</th>
</tr>
<tr>
  <td><pre>$query(<i>sitecore&nbsp;query</i>|<i>field&nbsp;name</i>)</pre></td>
  <td>
  Where <i>sitecore query</i> is a Sitecore query, executed relative to the position in the content tree where the new item is created. The token takes the first item in the result and replaces itself with the value of <i>field name</i>.<br><br>
  Usage Examples:<br>
  Provide some kind of dynamic default values authors can manage (nope, Authors should not edit _Standard Values templates):
  <pre>$query(/sitecore/content/Website/Config//*[@@name="Default config"]|Default Title)</pre>
  Or simply create dynamic defaults by copying the content from a parent items field.
<pre>$query(..|Description)</pre>
  </td>
</th>
<tr>
  <td><pre>$futureDate(<i>yy,mm,dd</i>)</pre></td>
  <td>The $futureDate token was originally created by <a href="https://twitter.com/briancaos">@briancaos</a> but slightly rewritten for the module.<br>
  It adds years, months and days to the date of the item creation.<br>
  For example:
  <ul>
   <li>$futureDate(1,0,0)   = Adds Now + 1 year</li>
   <li>$futureDate(0,6,0)   = Adds Now + 6 months</li>
   <li>$futureDate(1,6,12)  = Adds Now + 1 year, 6 months, 12 days</li>
  </ul>
  </td>
</tr>
</tr>
</table>

## Tested on
* Sitecore 7.0 (but should work with 6.x)

## Installation 
Install via NuGet Gallery
<pre>
  PM> tbd
</pre>

## Build
See [Readme.md in the /build folder](https://github.com/retohugi/SitecoreExtensions/blob/master/build/readme.md).

## Contribute
Have an idea for another handy standard value token? Let me know and create an issue on GitHub or even better, make a pull request.
