# SitecoreExtensions-TokenSet

This extension provides additional standard values tokens for Sitecore Data Templates. Read John West's blog post for an [introduction on Standard Value Tokens](http://www.sitecore.net/Community/Technical-Blogs/John-West-Sitecore-Blog/Posts/2012/05/Expand-Standard-Values-Tokens-in-Existing-Items-with-the-Sitecore-ASPNET-CMS.aspx) 

## Available Tokens
### $query('`sitecore query`'|`field name`)
Where `sitecore query` is a Sitecore query, executed relative to the position in the content tree where the new item is created. The token takes the first item in the result and replaces itself with the value of "field name".  

### $futureDate(`yy,mm,dd`)
The $futureDate token was originally created by [@briancaos](https://twitter.com/briancaos) but slightly rewritten for the module.

## Tested on
* Sitecore 7.0 (but should work with 6.x)

## Installation 
Install via NuGet Gallery
<pre>
  PM> tbd
</pre>

## Build
See Readme.md in the /build folder.

