# REST-API Documentation
## Requests
If any request ends up with errors, they will be sent by plain text
Access token should be sent in headers

- [Checklists](###Checklists)
- [Users](###Users)

### Checklists
#### Base URL: `http://194.67.104.135/checklists/`
#### `/get-info` [GET]
Request **(application/json)**

> int id

Response **(application/json)**

```json
{
	"title": "Time Management",
	"locale": "en"
}
```

#### `/get-file` [GET]
Request **(application/json)**

> int id

Response **(application/pdf)**

> PDF file

#### `/delete` [DELETE]
Request **(application/json)**

> int id

Response **(application/pdf)**

```json
{
	"success": true
}
```

#### `/create` [POST]
Request **(application/form-data)**

> string title
> PdfFile file

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
> string name
> string email
> string password
> string dreamingAbout
> string inspirer
> string whyInspired

Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/login` [POST]

Request **(application/json)**
> string email
> string password

Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/refresh` [POST]

Request **(application/json)**
> string token
> string refreshToken

Response **(application/json)**

```json
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/get` [GET]

Request **(application/json)**
> string token

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

#### `/delete` [DELETE]

Request **(application/json)**
> string token

Response **(application/json)**

```json
{
	"success": true
}
```

#### `/upload-avatar` [PUT]

Request **(application/form-data)**
> string token
> ImageFile file

Response **(application/json)**

```json
{
	"success": true
}
```

#### `/email-validation` [GET]

Request **(application/json)**
> string email

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

#### `/get-avatar` [PUT]

Request **(application/json)**
> string token

Response **(application/form-data)**

> ImageFile image
