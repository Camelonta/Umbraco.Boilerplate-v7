TIPS: Checka in ofta så man kan gå tillbaks ifall något går fel

OBS! Om du lägger till test-projekt och kör Visual Studio 2013 så kan det hamna på fel ställe. 
   I så fall: Kopiera hela mappen och ändra till rätt sökväg i .sln-filen. Stäng projektet och öppna det på nytt.


1. Skapa ett nytt projekt (ASP.NET Web Application). Välj empty, kryssa inte i några checkboxar (blir konflikt vid installation av Umbraco ifall man tar MVC)

2. Installera "Camelonta Umbraco Metapaket"  via NuGet

3. Starta projektet och kör Umbraco installationen OBS: Välj Customize för att slippa deras standard-mall

4. Installera Boilerplate-paketet (nedladdat från "Umbraco admin -> Utvecklare -> Skapade paket" i Boilerplate-projektet. Tips: Tryck på "Publicera" uppe till höger, innan exporten)

5. Byt namn på ditt namespace

6. Inkludera allt i App_Start, Views, Classes, css, img och scripts i ditt projekt

7. I Umbracos UI: Peka ut "Home" (eller egen förstasida) i rotnoden (sidträdet) -> fliken Properties -> umbracoInternalRedirectId på Site-noden

8. Properties för projektet -> Web -> Kryssa ur "Enable Edit and Continue". Det gör att applikationen fortfarande "lever" när du slutat debugga