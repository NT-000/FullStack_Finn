## Documentation

Table of contents

[Documentation](https://linktodocumentation)

## 1. Introduction

## Sjappa til kakkerlakk'n

A fullstack project that emulates a digital marketplace.

The system offers functionality for:

Creating, viewing, updating, and deleting ads

Managing favorites and user reviews

#### Direct messaging and real-time chat via SignalR

User registration, login with JWT-based authentication, and profile management

The project is divided into two main parts:

Backend: Built with .NET, Dapper, and SignalR for database operations and real-time communication.

Frontend: Built with Vue 3, Vite, Pinia, and Vue Router.

## 2. Installation

### 2.1 Setup Backend

Clone the repository

```bash
git clone https://github.com/NT-000/FullStack_Finn.git
cd FullStack_Finn/backend
```

Setup backend, install dependencies

```bash
dotnet restore
```

Build and run

```bash
dotnet build
dotnet run
```

### 2.2 Setup frontend

Install dependencies

```bash
cd FullStack_Finn/frontend
npm install
```

Start dev server

```bash
npm run dev
```

## 3. Backend documentation

### Controllers:

AdController.cs: Handles CRUD operations for ads.

FavoriteController.cs: Manages the user's favorites.

MessageController.cs: Retrieves conversations and interested users (important when selling) for ads.

ReviewController.cs: Handles creation and retrieval of reviews.

UserController.cs: Manages user profiles, registration, login, and logout.

ChatHub.cs: Implements the SignalR hub for real-time chat.

### Models and DTOs:

Defines data models such as Ad.cs, User.cs, Review.cs, etc., as well as DTOs for data transfer (e.g., CreateAdDto.cs,
AdUpdateDto.cs, ReviewDto.cs).

### Services:

Jwt.cs: Generates JWT tokens for authentication.

### Configuration Files:

Program.cs sets up the application, routing, authentication, and static files.

appsettings.json and launchSettings.json contain environment and startup settings.

## API Endpoints

#### Ads

| Method   | Endpoint                     | Description                                            |
|:---------|:-----------------------------|:-------------------------------------------------------|
| `GET`    | `/api/ads`                   | Retrieves all ads along with the owner info of the ad. |
| `GET`    | `/api/ads/${id}`             | Retrieves a specific ad by its ID.                     |
| `POST`   | `/api/ads/create-with-files` | Creates a new ad with images.                          |
| `PUT`    | `/api/ads/{id}`              | Updates an existing ad.                                |
| `PUT`    | `/api/ads/{id}/sold`         | Marks an ad as sold by specifying the buyer ID.        |
| `PUT`    | `/api/ads/{id}/location`     | Updates the location details of an ad.                 |
| `DELETE` | `/api/ads/{id}`              | Deletes an ad.                                         |

#### Favorites

| Method   | Endpoint                | Description                              |
|:---------|:------------------------|:-----------------------------------------|
| `GET`    | `/api/favorites`        | Retrieves the user's favorite ads.       |
| `POST`   | `/api/favorites/{adId}` | Adds an ad to the user's favorites.      |
| `DELETE` | `/api/favorites/{adId}` | Removes an ad from the user's favorites. |

### Messages

| Method | Endpoint                                | Description                                                         |
|:-------|:----------------------------------------|:--------------------------------------------------------------------|
| `GET`  | `/api/messages/conversation`            | Retrieves the conversation between two users.                       |
| `GET`  | `/api/messages/interested-users/{adId}` | Retrieves a list of users who have shown interest in a specific ad. |

### Reviews

| Method | Endpoint                 | Description                            |
|:-------|:-------------------------|:---------------------------------------|
| `GET`  | `/api/reviews/{userId}`  | Retrieves reviews for a specific user. |
| `GET`  | `/api/reviews/ad/{adId}` | Retrieves reviews for a specific ad.   |
| `POST` | `/api/reviews`           | Creates a new review.                  |

### Users

| Method | Endpoint                 | Description                                                         |
|:-------|:-------------------------|:--------------------------------------------------------------------|
| `GET`  | `/api/users`             | Retrieves a list of all users.                                      |
| `GET`  | `/api/users/profile`     | Retrieves the profile of the logged-in user.                        |
| `GET`  | `/api/users/profile/{id` | Retrieves the profile of a specified user (without sensitive data). |
| `POST` | `/api/users/register`    | Registers a new user, including profile image upload.               |
| `POST` | `/api/users/login`       | Authenticates a user and sets a JWT as an cookie.                   |
| `POST` | `/api/users/logout`      | Logs out the user by deleting the JWT cookie.                       |

## 4 .Frontend documentation

## Components and Views

### Components:

Contains reusable Vue components such as:

LoginForm.vue, SignupForm.vue – User login and registration.

AdChat.vue, AdList.vue, SingleAd.vue – For displaying chat, ads and single ad with details.

CreateReview.vue, MyProfile.vue, NavBar.vue, OtherProfile.vue – For reviews, user profiles, and navigation.

## Views:

StartPage.vue – Consists of login and sign in, redirects users that are not logged in to here.

Ads.vue, AdDetails.vue – Display lists of ads and people with search functionality and ad details.

CreateNewAd.vue – Form for creating a new ad that consists of title, description, with files.

Profile.vue – Displays user profiles based on user, used two components with v-if conditions.

Chat.vue, Inbox.vue – For real-time chat and messaging.

Map.vue – Map display with sorting of categories on the map for all ads.

PageNotFound.vue - 404 page, fallback if URL is not valid.

## Composables

getRoute.js: Retrieves lists of data based on dynamic API routes.

useFormatDate.js: Formats dates for display.

useLogin.js and useLogout.js: Handle login and logout with API calls.

useStarRating.js: Generates HTML for star ratings.

useUser.js: Fetches and stores user profile data.

## Pinia Stores

adStore.js: Manages ads, including fetching, updating, deleting, and marking ads as sold.

chatStore.js: Manages real-time chat using SignalR, message transmission, and conversation loading.

favStore.js: Manages favorite ads with functions to add, remove, and check favorites.

useUserStore.js: Stores the global user state and handles fetching/updating user data.

## 5. Authentication and Security

#### JWT

Used for authentication. The token is generated in Jwt.cs and validated in Program.cs using JWT middleware.
After login, the JWT is stored as an HttpOnly cookie.

## SignalR

SignalR enables real-time messages between the users without the need for refreshing the page.

#### Backend:

ChatHub.cs is a SignalR-hub that handles messaging logic.
When a user connects they are added in a SignalR-group based on their userId.
When SendMessage() is called:
1.Message is stored in the database.
2.The message is distributed to both sender and receiver.

#### Frontend:

chatStore.js creates a connection when entering the chat.
Listening to .on('ReceiveMessage') and new messages are added to the messages array in chatStore.
Messages are sent by .invoke('SendMessage'), to the hub on the backend.

## 6. Tech Stack

### Backend:

.NET (ASP.NET Core).

Dapper

SQL Server

SignalR

JWT Authentication

BCrypt.Net

### Frontend:

Vue 3

Vite

Pinia

Vue Router

Axios

Leaflet

FontAwesome

