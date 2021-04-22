# API Documentation
## Requests
If any request ends up with errors, they will be sent by plain text
Access token should be sent in headers for requests with **[Token required]** mark
Header example:
```json
{
	"version": "1.0",
	"Authorization": "bearer {ACCESS TOKEN HERE}"
}
```

- [Checklists](###Checklists)
- [Users](###Users)

### Checklists
#### Base URL: `http://194.67.104.135/checklists/`
#### `/get-info` [GET]
Request **(application/json)**
```swift
{
	"id": Int
}
```
Response **(application/json)**

```json
{
	"title": "Time Management",
	"locale": "en"
}
```

#### `/get-file` [GET]
Request **(application/json)**

```swift
{
	"id": Int
}
```

Response **(application/pdf)**

> PDF file

#### `/delete` [DELETE] [Token required] [Admin access]
Request **(application/json)**

```swift
{
	"id": Int
}
```

Response **(application/pdf)**

```json
{
	"success": true
}
```

#### `/create` [POST] [Token required] [Admin access]
Request **(application/form-data)**

```swift
{
	"title": String,
	"file": PdfFile
}
```

Response **(application/pdf)**

```json
{
	"success": true
}
```

### Users
#### Base URL: `http://194.67.104.135/users/`
#### `/register` [POST] 

Request **(application/json)**

```swift
{
	"name": String,
	"email": String,
	"password": String,
	"dreamingAbout": String,
	"inspirer": String,
	"whyInspired": String.
	"interests": [Int]
}
```

Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/login` [POST]

Request **(application/json)**

```swift
{
	"email": String,
	"password": String
}
```

Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/refresh` [POST] 
This request does not require access token in header, but in body along with refresh token. New pair will be given

Request **(application/json)**
```swift
{
	"token": String,
	"refreshToken": String
}
```
Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/get` [GET] [Token required]

Request **(application/json)**

Response **(application/json)**

```json
{
    "user": {
        "name": null,
        "email": "art3a@niuitmo.ru",
        "dreamingAbout": "Bed",
        "inspirer": "Tamerlan Vstaldualdibom",
        "whyInspired": "gay came out last month",
        "subscriptionEndDate": "0001-01-01T00:00:00"
    },
    "success": true,
    "errors": null
}
```

#### `/delete` [DELETE] [Token required]

Request **(application/json)**


Response **(application/json)**

```json
{
	"success": true
}
```

#### `/upload-avatar` [PUT] [Token required]

Request **(application/form-data)**
> ImageFile file

Response **(application/json)**

```json
{
	"success": true
}
```

#### `/email-validation` [GET] 

Request **(application/json)**
```swift
{
	"email": String
}

```
Response **(application/json)**

```json
{
	"success": true/false,
	"reason": 1
}
```

Reasons list
> 1 - Invalid email format
> 2 - Email already exists

#### `/get-avatar` [PUT] [Token required]

Request **(application/json)**

Response **(application/form-data)**

> ImageFile image
