# PillowfortFs

This is an F# (.NET Standard) library for logging into https://pillowfort.io.

This library was developed as part of [CrosspostSharp](https://github.com/libertyernie/CrosspostSharp).

Features:
* Scraping username and avatar
* Listing posts by user
* Creating text posts
* Creating photo posts from an existing URL

Not supported:
* Audio/video posts
* Uploading photos
* Other features

This will probably break at some point as the site is updated and I can't
promise I'll try to fix it.

## Example usage

### F#

    open PillowfortFs

    let f = async {
        let! client = PillowfortClientFactory.AsyncLogin username password
        let! username = client.AsyncWhoami
        do! c.AsyncSubmitPost {
            title = "Post #1"
            content = "This is a photo post"
            tags = ["test1"; "test2"]
            privacy = PrivacyLevel.Private
            rebloggable = true
            commentable = true
            nsfw = false
            media = Some (PhotoMedia "https://www.example.com/image.png")
        }
    }

### C#

    using PillowfortFs;
    using System.Threading.Tasks;

    async Task f() {
        var client = await PillowfortClientFactory.LoginAsync(username, password);
        var username = await client.WhoamiAsync();
        await client.SubmitPostAsync(
            new PostRequest(
                title: "Post #1",
                content: "This is a photo post",
                tags: new [] { "test1", "test2" },
                privacy: PrivacyLevel.Private,
                rebloggable: true,
                commentable: true,
                nsfw: false,
                media: PillowfortMediaBuilder.Photo("https://www.example.com/image.png")));
    }

Visual Basic usage is similar to C#.
