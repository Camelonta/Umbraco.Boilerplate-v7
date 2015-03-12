TIPS: Checka in ofta så man kan gå tillbaks ifall något går fel

1. Skapa ett nytt projekt (ASP.NET Web Application). Välj empty, kryssa inte i några checkboxar (blir konflikt vid installation av Umbraco ifall man tar mvc)

2. Om du lägger till test-projekt och kör Visual Studio 2013 så kan det hamna på fel ställe. 
   I så fall: Kopiera hela mappen och ändra till rätt sökväg i .sln-filen. Stäng projektet och öppna det på nytt.

3. Checka in alla filer som ska checkas in. Om något går fel slipper vi göra om dessa steg.

4. Installera Umbraco CMS via NuGet

5. Checka in alla filer som ska checkas in. Om något går fel kan vi bara rensa och börja om här.

6. Starta projektet och installera OBS: Välj Customize för att slippa deras standard-mall

7. Installera Boilerplate-paketet

8. Inkludera allt i App_Start, Views, css, img och scripts i ditt projekt

9. Byt namn på ditt namespace

10. För att bundling av css/js filer ska fungera så lägg till ~/bundles på umbracoReservedPaths i web.config och installera Microsoft.AspNet.Web.Optimization från nuget

11. Sätt umbracoTimeOutInMinutes i web.config till tex 1440 (24 timmar) för att slippa bli utloggad

12. Installera önskade Camelonta-paket