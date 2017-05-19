﻿======= DEVELOP IN BOILERPLATE =======
 
 SASS:  
	- Download an extension that compiles the SASS-files when you save then (Web Compiler ex. https://visualstudiogallery.msdn.microsoft.com/3b329021-cd7a-4a01-86fc-714c2d05bb6c)

======================================




**********************************************************************************************************
				---  CAMELONTA STYLEGUIDE FOR CSS: https://github.com/Camelonta/CSS ---
**********************************************************************************************************




======= USE BOILERPLATE FOR A PROJEKT =======

1. Copy the Boilerplate to a new repository

2. Rename everything (the folder + project + Assembly name + Default namespace in project properties)
	- Search n replace on "Boilerplate.Web" -> "CustomerNameSpace.Web" (ex. "MyCustomer.Web"), (the same for *.Core + *.Tests), and "Boilerplate" -> "The Customers Acctual Name"

3. Change port (right-click on the project in Visual Studio -> Web -> Project Url  (needs to be more than 1024, to be able to open it as a user that is not Admin)

4. Change the URL in robots.txt

5. Make a nice default image for social media (ex: _Layout -> <meta property="og:image">). Exists in img/default-social-share.jpg (1200 x 630)

6. Favicons: Make an icon of ex the logo (260 x 260). Generate all icons in the site root http://realfavicongenerator.net/ (save them in img/favicons for your local project but copy them to the siteroot for production). HTML for this goes in Partials/_Favicons

Last but not least: Remove this README :)

OBS: Remove the folder "boilerplate" from media before production (contains some images for testing).

====================================================




===== UPDATING UMBRACO-VERSION =======
 
--- Make shure this is still in web.config ---
 < system.webServer>
 <!-- CACHE-stuff -->
  < staticContent>
 <clientCache cacheControlMode="UseMaxAge" cacheControlMaxAge="7.00:00:00" />
  </staticContent>

<urlCompression doDynamicCompression="true" doStaticCompression="true" dynamicCompressionBeforeCache="true"/>
===============================




======= REGULAR PROBLEMS =======

- Ambigious reference: Remove dll: Boilerplate.Web in your new bin-folder

===============================