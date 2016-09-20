======= UTVECKLA I BOILERPLATE =======
 
 SASS:  
	- Ladda ned en extension för Visual studio som kompilerar sass-filerna när du sparar (Web Compiler t.ex. https://visualstudiogallery.msdn.microsoft.com/3b329021-cd7a-4a01-86fc-714c2d05bb6c)

======================================




======= ANVÄND BOILERPLATE TILL ETT PROJEKT =======

1. Kopiera hela Boilerplatemappen och klistra in den där du vill ha den

2. Döp om allt (mappen + projektet + Assembly name + Default namespace i projektproperties)
	- Gör search n replace på "Boilerplate.Web" -> "KundensNameSpace.Web" (ex. "SaljarnasForbund.Web"), (OBS! Samma för *.Core), därefter "Boilerplate" -> "Kundens riktiga namn" (ex. "Säljarnas förbund")

3. Byt portnummer (högerklicka på projektet i Visual studio -> Web -> Project Url (skriv in något fint portnummer som rimmar med ditt projekt). OBS! Sätt portnumret högre än 1024, annars kan man inte öppna projektet utan att vara Admin.

4. Fixa URL:en i robots.txt

5. Fixa en snygg defaultbild för sociala medier (se: _Layout -> <meta property="og:image">). Finns i img/default-social-share.jpg (1200 x 630) (Jesse kan kirra biffen om du vill)

6. Favicons: Fixa en ikon av t.ex. logotypen som är 260 x 260. Generera därefter alla ikoner så att de HAMNAR I ROTEN(!) http://realfavicongenerator.net/ (i ditt lokala projekt kan du ha dem i img/favicons. Men LIVE = roten). Lägg html i Partials/_Favicons

SIST: Ta bort denna README :)

OBS: Ta helst bort mappen "boilerplate" från mediearkivet när du går live.

====================================================




===== VID UPPDATERING AV UMBRACO-VERSION =======
 
--- Se till att detta finns i web.config ---
 < system.webServer>
 <!-- CACHE-stuff -->
  < staticContent>
 <clientCache cacheControlMode="UseMaxAge" cacheControlMaxAge="7.00:00:00" />
  </staticContent>

<urlCompression doDynamicCompression="true" doStaticCompression="true" dynamicCompressionBeforeCache="true"/>
===============================




======= VANLIGA PROBLEM =======

- Ambigious reference: Ta bort dll:en för Boilerplate.Web i din nya bin-katalog (krockar med din dll)

===============================
