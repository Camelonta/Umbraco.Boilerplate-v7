======= UTVECKLA I BOILERPLATE =======
 
 SASS:  
	- Ladda ned en extension för Visual studio som kompilerar sass-filerna när du sparar (Web Compiler t.ex. https://visualstudiogallery.msdn.microsoft.com/3b329021-cd7a-4a01-86fc-714c2d05bb6c)

======================================






======= ANVÄNDA BOILERPLATE TILL ETT PROJEKT =======

1. Kopiera hela Boilerplatemappen och klistra in den där du vill ha den

2. Döp om allt (mappen + projektet)
	- Gör search n replace på "Camelonta.Boilerplate" -> "Kunden", därefter "Boilerplate" -> "Kunden"

3. Byt portnummer (högerklicka på projektet i Visual studio -> Web -> Project Url (skriv in något fint portnummer som rimmar med ditt projekt)

4. Fixa URL:en i robots.txt

5. Byt ut slutet av titeln i Meta.cs -> PageTitle ("- Boilerplate")

6. Fixa en snygg defaultbild för sociala medier (se: _Layout -> <meta property="og:image">). Finns i img/default-social-share.jpg (1200 x 630) (Jesse kan kirra biffen om du vill)

7. Favicons: Fixa en ikon av t.ex. logotypen som är 260 x 260. Generera därefter alla ikoner så att de HAMNAR I ROTEN(!) http://realfavicongenerator.net/ (i ditt lokala projekt kan du ha dem i img/favicons. Men LIVE = roten). Lägg html i Partials/_Favicons


SIST: Ta bort denna README :)

====================================================