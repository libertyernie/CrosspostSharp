namespace PillowfortFs

type UploadableFile = {
    data: byte[]
    content_type: string
    filename: string
}

type Media = PhotoMedia of string

type PrivacyLevel =
    | Public = 0
    | Followers = 1
    | Private = 2

type PostRequest = {
    title: string
    content: string
    tags: seq<string>
    privacy: PrivacyLevel
    rebloggable: bool
    commentable: bool
    nsfw: bool
    media: Media option
}

// for .NET interoperability
module PillowfortMediaBuilder =
    let None: Media option = None
    let Photo photo_url: Media option = Some (PhotoMedia photo_url)