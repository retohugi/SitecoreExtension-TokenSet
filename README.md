SitecoreExtension-DynamicKeyPlaceholder
=======================================
SitecoreExtension for adding DynamicKeyPlaceholders allowing for a spot with placeholder to be added multiple times to a page.

Appends the rendering Unique Id to the Placeholder id for allowing a unique reference for the page renderings to attach on, default implementation only allows for a single Placeholder ID to be present on a page at one time. The extension is hidden to the ediors. 

Based on prior work that turned out to be the same solution that Nick Wesselman blogged about at http://www.techphoria414.com/Blog/2012/May/Sitecore_Page_Editor_Unleashed

Now available on nuget and github.

1508 / Design in Love with Technology / http://1508.dk

## Requirements
* Sitecore 6.x 

## Installation 
Install via nuget
<pre>
  PM> Install-Package SitecoreExtension.DynamicKeyPlaceholder
</pre>

Use the Control DynamicKeyPlaceholder instead of a Sitecore Placeholder control. 

## Example of use
```
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpotsContainer.ascx.cs" Inherits="Web.UI.SpotsContainer" %>
<%@ Register TagPrefix="sce" Namespace="SitecoreExtension.DynamicKeyPlaceholder.Controls" Assembly="SitecoreExtension.DynamicKeyPlaceholder" %>
<div class="container container-wide">
    <div class="container-inner">
        <div class="spots">
            <sce:DynamicKeyPlaceholder runat="server" ID="SpotsPlaceholder" Key="SpotsPlaceholder" editable="true" />
        </div>
    </div>
</div>
```

## Notice
Once content is added into the dynamic placeholders the rendering is bound the the placeholderid combined with the unique rendering id, meaning that removing a spot with the dynamic placeholder and adding it again will create a new unique id and therefore will leave the other renderings orphans in the layout information and the user must re-add the renderings. This is sound and expected behavior :)