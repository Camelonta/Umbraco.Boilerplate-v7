------- NYA SÄTTET ------- 
1. Kopiera hela Boilerplatemappen och klistra in den där bra du vill ha den

2. Döp om allt (mappen + projektet)

3. Byt portnummer (högerklicka på projektet i Visual studio -> Web -> Project Url (skriv in något fint portnummer)






------- GAMLA SÄTTET: ------- 

TIPS: Checka in ofta så man kan gå tillbaks ifall något går fel

OBS! Om du lägger till test-projekt och kör Visual Studio 2013 så kan det hamna på fel ställe. 
   I så fall: Kopiera hela mappen och ändra till rätt sökväg i .sln-filen. Stäng projektet och öppna det på nytt.


1. Skapa ett nytt projekt (ASP.NET Web Application). Välj empty, kryssa inte i några checkboxar (blir konflikt vid installation av Umbraco ifall man tar MVC)

2. Installera "Camelonta Umbraco Metapaket" via NuGet
(Om ej newtonsoft-dll kommer med så uppdatera/installera om umbraco)

3. Starta projektet och kör Umbraco installationen OBS: Välj Customize för att slippa deras standard-mall

4.a Starta Boilerplate-projektet. Gå till "Umbraco admin -> Utvecklare -> Paket -> Skapade paket -> Boilerplate". Tryck först "Publicera" uppe till höger, sen download. 
4.b Paketet finns nu som zip på disk. I ditt nya projekt, gå till "Ubraco admin -> Utvecklare -> Paket" och tryck på "Installera lokalt paket". Där väljer du filen du just skapade.

5. Inkludera allt i App_Start, Views, Classes, css, img och scripts i ditt projekt

6. Byt namn på ditt namespace (Refactor)
Dubbelkolla att alla camelontas paket är nyaste versionen.

7. I Umbracos UI: Peka ut "Home" (eller egen förstasida) i rotnoden (sidträdet) -> fliken Properties -> umbracoInternalRedirectId på Site-noden

8. Properties för projektet -> Web -> Kryssa ur "Enable Edit and Continue". Det gör att applikationen fortfarande "lever" när du slutat debugga

9. Kopiera Config/umbracoSettings.config/requestHandler så att det blir snyggare omskrivning av åäö i url:er


-- FELSÖKNING --

- "Failed to retrieve data for content id" (när du installerat boilerplatepaket och ska gå till en sida) 
   Antagligen datatyperna "Camelonta - Top links" och "Camelonta - Slider" som inte har någon struktur i sin Archetype. Skapa dom.