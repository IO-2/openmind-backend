# REST-API Documentation
## Requests
If any request ends up with errors, they will be sent by plain text
Access token should be sent in headers for requests with **[Token required]** mark
Header example:
```swift
{
	"version": "1.0",
	"Authorization": "bearer {ACCESS TOKEN HERE}"
}
```

- [Media](###Media)
- [Users](###Users)
- [Courses](###Courses)

### Media
#### Base URL: `https://openmind.ru.com/media/`
#### `/get-info` [GET]
Request **(application/json)**
```swift
{
	"id": Int,
	"locale": String
}
```
Response **(application/json)**

```swift
{
	"title": String,
	"locale": String,
	"text": String,
	"type": Int,
	"category": Int,
	"uploadedTime": Long,
	[UPDATE]
	"imageUrl": String
}
```

Media types
> 1 - Checklist
> 2 - Longread
> 3 - About cool guys
[DEPRECATED]
#### `/get-file` [GET]
Request **(application/json)**

```swift
{
	"id": Int,
	"locale": String
}
```

Response **(image/png OR image/jpeg OR image/jpg)**

> Image file

#### `/delete` [DELETE] [Token required] [Admin access]
Request **(application/json)**

```swift
{
	"id": Int
}
```

Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/create` [POST] [Token required] [Admin access]
Request **(application/form-data)**

```swift
{
	"title": String,
	"file": Image,
	"text": String,
	"locale": String,
	"type": Int,
	"category": Int
}
```

Categories
> 1 - ...
> 2 - ...
> 3 - ...
> 4 - ...

Media types
> 1 - Checklist
> 2 - Longread
> 3 - About cool guys

Response **(application/pdf)**

```swift
{
	"success": Bool
}
```

#### `/get-info-all` [GET]
Sorts by date descending
Contains 8 media elements from every category

Request **(application/json)**

```swift
{
	"locale": String
}
```

Response **(application/json)**

```swift
[
	{
		"id": Int,
		"title": String,
		"type": Int,
		"category": Int,
		"uploadedTime": Long,
		[UPDATE]
		"imageUrl": String
	}
]
```

Media types
> 1 - Checklist
> 2 - Longread
> 3 - About cool guys

#### `/get-info-by-category` [GET]
Sorts by date descending
Every page contains up to 20 media elements

Request **(application/json)**

```swift
{
	"locale": String,
	"category": Int,
	"page": Int
}
```

Response **(application/json)**

```swift
[
	{
		"id": Int,
		"title": String,
		"type": Int,
		"category": Int,
		"uploadedTime": Long,
		[UPDATE]
		"imageUrl": String
	}
]
```

Media types
> 1 - Checklist
> 2 - Longread
> 3 - About cool guys

### Users
#### Base URL: `https://openmind.ru.com/users/`
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

```swift
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

Error codes
> 455 - email format is incorrect
> 456 - email is already exists (on register)
> 457 - password format is incorrect (on register)

#### `/login` [POST]

Request **(application/json)**

```swift
{
	"email": String,
	"password": String
}
```

Response **(application/json)**

```swift
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

Error codes
> 455 - email format is incorrect
> 458 - incorrect password
> 459 - user does not exists

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

```swift
{
	"token": "string of nonsense",
	"refreshToken": "another string of nonsense"
}
```

#### `/get` [GET] [Token required]

Request **(application/json)**
```swift
{
	"locale": String
}
```

Response **(application/json)**

```swift
{
	"name": String,
	"email": String,
	"dreamingAbout": String,
	"inspirer": String,
	"whyInspired": String,
	"subscriptionEndDate": Long,
	"successes": {
		"1": Float,
		"2": Float,
		"3": Float,
		"4": Float
	},
	[UPDATE]
	"avatarUrl": String
}
```

> 1 - IT
> 2 - SMM
> 3 - Design
> 4 - Content

#### `/delete` [DELETE] [Token required]

Request **(application/json)**


Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/upload-avatar` [POST] [Token required]

Request **(application/form-data)**
> ImageFile file

Response **(application/json)**

```swift
{
	"success": Bool
}
```
*[DEPRECATED]*
#### `/get-avatar` [PUT] [Token required]

Request **(application/json)**

Response **(image/jpg OR image/png OR image/jpeg)**

> ImageFile image

#### `/add-progress` [POST] [Token required]

Request **(application/json)**
```swift
{
	"sectionNumber": Int,
	"progress": Int
}
```

**progress** - how many courses did user complete

Response **(application/json)**

```swift
{
	"success": Bool
}
```

### Courses
#### Base URL: `https://openmind.ru.com/courses/`
#### `/create-course` [POST] [Token required] [Admin access]

Request **(application/form-data)**
```swift
{
	"title": String,
	"image": Image,
	"videoUrl": String,
	"description": String,
	"lessonsDescription": String,
	"lessonsAmount": Int,
	"whatWillBeLearned": String,
	"speakerPicture": Image,
	"speakerDescription": String,
	"speakerName": String,
	"section": Int,
	"courseDuration": Int,
	"locale": String,
	[UPDATE]
	"imageUrl": String
}
```

**Image** - is jpeg, png or jpg file

Response **(application/json)**

```swift
{
	"success": true,
	"id": 1
}
```

By that id, it needs to route `/create-benefiters` ,  `/create-cards` and `/create-course-lesson`  section to add additional information on course

#### `/create-benefiters` [POST] [Token required] [Admin access]

Request **(application/json)**
```swift
[
	{
		"courseId": Int,
		"benefiterNumber": Int,
		"title": String,
		"text": String,
		"locale": String	
	}
]
```

Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/create-cards` [POST] [Token required] [Admin access]

Request **(application/json)**
```swift
[
	{
		"courseId": Int,
		"cardNumber": Int,
		"title": String,
		"text": String,
		"locale": String
	}
]
```

Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/create-course-lesson` [POST] [Token required] [Admin access]

Request **(application/json)**
```swift
{
	"courseId": Int,
	"title": String,
	"description": String,
	"videoUrl": String,
	"lessonNumber": Int,
	"locale": String
}
```

Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/delete-course` [DELETE] [Token required] [Admin access]

Request **(application/json)**
```swift
{
	"id": Int
}
```

Response **(application/json)**

```swift
{
	"success": Bool
}
```

#### `/get` [GET]

Request **(application/json)**

```swift
{
	"locale": String,
	"page": Int,
	"query": String
}
```

Response **(application/json)**

```swift
[
	{
		"id": Int,
		"title": String,
		"section": Int,
		[UPDATE]
		"imageUrl": String
	}
]
```
*[DEPRECATED]*
#### `/get-course-picture` [GET]

Request **(application/json)**

```swift
{
	"id": Int
}
```

Response **(image/jpeg OR image/jpg OR image/png)**

> Image 

#### `/get-info` [GET]

Request **(application/json)**

```swift
{
	"id": Int
}
```

Response **(application/json)**

```swift
{
	"id": Int,
	"title": String,
	"videoUrl": String,
	"uploadedTime": Long,
	"description": String,
	"lessonsDescription": String,
	"lessonsAmount": Int,
	"whatWillBeLearned": String,
	"section": Int,
	"courseDuration": Int,
	"cards": [
		{
			"id": Int,
			"cardNumber": Int,
			"title": String,
			"text": String
		}
	],
	"benefiters": [
		{
			"id": Int,
			"benefiterNumber": Int,
			"title": String,
			"text": String
		}
	],
	"lessons": [
		{
			"title": String,
			"lessonNumber": Int
		}
	],
	[UPDATE]
	"imageUrl": String,
	"speaker": {
		"description": String,
		"name": String,
		"imageUrl": String,
	}
}
```

#### `/get-lesson` [GET] [Token required]
Get course lessons by course id
Only for subscribed

Request **(application/json)**

```swift
{
	"id": Int,
	"lessonNumber": Int
}
```

Response **(application/json)**

```swift
{
	"title": String,
	"description": String,
	"videoUrl": String,
	"lessonNumber": Int
}
```

