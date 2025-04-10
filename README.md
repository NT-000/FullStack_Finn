## 📚 Dokumentasjon

### Sjappa til kakkelakk'n
***Et full-stack prosjekt hvor hensikten har vært å etterlikne en nettmarkedsplass, som for eksempel Finn.no.***

### Innholdsfortegnelse

- [🌐 Funksjoner](#-funksjoner)
- [⚙️ Teknologistack](#️-tech-stack)
- [🛠️ Installasjon](#-installasjon)
- [📡 API Endepunkter](#-api-endepunkter)
- [🖼️ Frontendstruktur](#-frontendstruktur)
- [🔒 Autentisering og sikkerhet](#-autentisering-og-sikkerhet)
- [💬 SignalR Sanntidschat](#-signalr-sanntidsmeldingerchat)
---
## 🌐 Funksjoner

- Opprettelse, visning, redigering og sletting av annonser.
- Søk og filtrering av annonser.
- Kartvisning av annonser med Leaflet.
- Legge til/fjerne favoritter og lage vurderinger av brukere etter "kjøp".
- Direktemeldinger som oppdateres i sanntid med SignalR.
- Registrering av nye brukere, JWT-basert innlogging og profilhåndtering.

---

## ⚙️ Tech-stack

### 🖥️ Backend
- ⚙️ **ASP.NET Core** 
- 🗄️ **Microsoft SQL Server**
- 🧵 **Dapper**
- 🔌 **SignalR**
- 🔐 **JWT-autentisering**
- 🔑 **BCrypt.Net**

### 🖼️ Frontend
- 🖌️ **Vue 3 (Composition API)**
- ⚡ **Vite**
- 📦 **Pinia**
- 🧭 **Vue Router**
- 🔗 **Axios**
- 🗺️ **Leaflet**
- 🌟 **FontAwesome**

---

## 🛠️ Installasjon

### Backend

```bash
git clone https://github.com/NT-000/FullStack_Finn.git
cd FullStack_Finn/backend
dotnet restore
dotnet build
dotnet run
```

### Frontend

```bash
cd FullStack_Finn/frontend
npm install
npm run dev
```
...Hvis du vil ha test-databasen👇
## 🗄️Opprette database

1. **Åpne SQL Server Management Studio**
2. **Koble til `localdb`**:
    - Servernavn: `(localdb)\\local`
    - Autentisering: `Windows Authentication`

3. **Opprett tom database**:
    - Høyreklikk `Databases` → `New Database...`
    - Navngi den `Finn.no` → Klikk OK

4. **Importer databasen**:
    - Klikk `File` → `Open` → `File...`
    - Velg `backend/database/Finn.no.sql`
    - Trykk `F5` for å kjøre scriptet

Dette vil:
- 📦 Opprette tabeller
- 🗂️ Fylle med testdata
- ✅ Gjøre databasen klar til bruk

| Email | Passord      |
|:------|:-------------|
| `el.kameleono@mail.com`  | `123Passord` |
Alle brukere i databasen har kryptert passord, men nesten alle har samme passord.

***123Passord***

> 💡 Du finner `Finn.no.sql` i mappa: `backend/db/Finn.no.sql`

Da bør alt være klart, enjoy!🚀

## 🔌 API Endepunkter

#### 📰 Ads 

| Metode   | Endepunkt                     | Beskrivelse                           |
|:---------|:-----------------------------|:--------------------------------------|
| `GET`    | `/api/ads`                   | Henter alle annonser med eierinfo     |
| `GET`    | `/api/ads/${id}`             | Henter en spesifikk annonse.          |
| `POST`   | `/api/ads/create-with-files` | Oppretter ny annonse med bilder.      |
| `PUT`    | `/api/ads/{id}`              | Oppdater eksisterende annonse.        |
| `PUT`    | `/api/ads/{id}/sold`         | Markerer annonse som solgt.           |
| `PUT`    | `/api/ads/{id}/location`     | Oppdater stedsinformasjon i annonsen. |
| `DELETE` | `/api/ads/{id}`              | Sletter en spesifikk annonse.         |

#### ❤️ Favorites

| Metode   | Endepunkt                | Beskrivelse                                               |
|:---------|:------------------------|:---------------------------------------------------------|
| `GET`    | `/api/favorites`        | Henter den innloggede brukerens favorittannonser         |
| `POST`   | `/api/favorites/{adId}` | Legger til annonse til favoritter hos innlogget bruker.  |
| `DELETE` | `/api/favorites/{adId}` | Sletter annonse fra den innloggede brukernes favoritter. |

#### ✉️ Messages

| Metode | Endepunkt                                | Beskrivelse                                                            |
|:-------|:----------------------------------------|:-----------------------------------------------------------------------|
| `GET`  | `/api/messages/conversation`            | Henter meldingene mellom to spesifikke brukere.                        |
| `GET`  | `/api/messages/interested-users/{adId}` | Henter en liste av brukere som har vist interesse for en gitt annonse. |

#### 📝 Reviews

| Metode | Endepunkt                 | Beskrivelse                                 |
|:-------|:-------------------------|:--------------------------------------------|
| `GET`  | `/api/reviews/{userId}`  | Henter anmeldelsene av en spesifikk bruker. |
| `GET`  | `/api/reviews/ad/{adId}` | Henter anmeldelsen av en spesifikk annonse. |
| `POST` | `/api/reviews`           | Lager en ny anmeldelse av bruker.           |

#### 👥 Users

| Metode | Endepunkt                  | Beskrivelse                                                                                   |
|:-------|:--------------------------|:----------------------------------------------------------------------------------------------|
| `GET`  | `/api/users`              | Henter alle brukere.                                                                          |
| `GET`  | `/api/users/profile`      | Henter profilen til den innloggede brukeren.                                                  |
| `GET`  | `/api/users/profile/{id}` | Henter en spesifikk annen bruker.                                                             |
| `POST` | `/api/users/register`     | Registrerer en ny bruker med navn, e-post, passord og valgfritt profilbilde.                  |
| `POST` | `/api/users/login`        | Logger inn en bruker med e-post og passord, og returnerer en JWT-token som lagres som cookie. |
| `POST` | `/api/users/logout`       | Logger ut brukeren og sletter cookie.                                                         |

## 🖼️ Frontend
### Views
- **StartPage.vue** – Startskjerm med innlogging / registrering - komponenter.
- **Ads.vue** – Annonseside med mulighet for søk på brukere, annonser og pris-intervall.
- **AdDetails.vue** - View for annonse-detaljer med SingleAd.vue som komponent.
- **CreateNewAd.vue** – Opprettelse av nye annonser fra brukere, koordinater satt i annonse blir default hentet fra posisjonen i nettleser(`navigator.geolocation`), bruker kan legge ved flere bilder i annonsen.
- **Profile.vue** – Egen/andres profil dynamisk basert på parameter.
- **Chat.vue** - View for chat med AdChat.Vue som komponent.
- **Inbox.vue** – Innboks for aktive annonser med mulighet for å se chat med brukere som har vist interesse for annonsen samt mulighet til å velge "kjøper" utefra andre brukere som har sendt melding.
- **Map.vue** – Kartvisning med mulighet for å sortere etter ulike på kategorier på kartet.
- **PageNotFound.vue** – 404.

### 🧬 Komponenter
- LoginForm.vue, SignupForm.vue - Innlogging og registrering.
- AdList.vue, SingleAd.vue, AdChat.vue - Annonsevisning og chat-funksjonalitet.
- MyProfile.vue - Innlogget bruker sin side bestående av brukerinfo og lister.

📜 Lister på brukerprofil:
  ###### Favorittliste - Brukerens valgte favoritter.
  ###### Mine kjøp - Kjøpene gjort av bruker.
  ###### Aktive annonser - Brukerens annonser som ikke er solgt.
  ###### Anmeldelser - Anmeldelser gjort av andre brukere av selger.

---
- OtherProfile.vue - Består av brukerinfo, profilbildet og aktive annonser samt anmeldelser.
- NavBar.vue - For å navigere på en enkel måte mellom sidene.
- CreateReview.vue - For å lage en ny anmeldse av bruker etter kjøp.

### 🧩 Composables
- `useUser.js` - For å hente enten innlogget bruker eller annen bruker avhengig av parameter.
- `useLogin.js`, `useLogout.js` - For innlogging og utlogging av bruker.
- `useStarRating.js` - For å konvertere rating til HTML-stjerner.
- `useCategories.js` - Brukes for kategorier somm brukes på tvers av komponenter.
- `getRoute.js` - Henter data basert på parameter som sendes med.
- `useFormatDate.js` - Formaterer dato-string og returnerer en string i brukerens språk i nettleser, i tilfellet Sjappa til Kakkelakk'n skulle bli en internasjonal hit. 

### 📦 Pinia Stores
- `useUserStore.js` – Lagring av globale brukerdata(innlogget bruker, sette og fjerne innlogget bruker)
- `adStore.js` – Oppdatering, sletting, "salg" og henting av annonser.
- `chatStore.js` – Chat og samtaler (SignalR).
- `favStore.js` – Legge til, fjerne og henting av favorittene til bruker.

---
## 🔒 Autentisering og sikkerhet

- JWT lagres som **HttpOnly-cookie** etter innlogging.
- `Jwt.cs` genererer token.
- Passord lagres kryptert med BCrypt.

---

## 📡 SignalR Sanntidsmeldinger/chat

### Backend (`ChatHub.cs`)
- Ved tilkobling blir bruker lagt i SignalR-gruppe basert på innlogget bruker-Id fra JWT-token.
- Meldinger lagres i databasen.
- Sendes i sanntid til begge parter via `ReceiveMessage`.

### Frontend (`chatStore.js`)
- Kobler til SignalR ved chat-start.
- Henter samtale via `loadConversation`.
- Sender meldinger med `SendMessage` og lytter på nye.

  #### [↩ Tilbake til innholdsfortegnelse](#-dokumentasjon) ####
---
