ArtSync 2.0
===========

Source: https://github.com/libertyernie/ArtSync

--------------------

WeasylSync is a Windows desktop application that loads individual submissions
from DeviantArt and lets you post them to your Tumblr and Inkbunny
accounts, maintaining the title, description, and tags.

Only visual submissions (artwork) are supported at this time.

Requirements:

* Windows Vista or higher
* .NET Framework 4.6.1 or higher

Authentication
--------------

To use this application, you must connect it to your DeviantArt
account. Information for all accounts will be stored in the file
ArtSync.json, so keep that file secure!

### DeviantArt

TBD

### Tumblr

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign into Tumblr using OAuth. Once signed in, the OAuth token
will be stored in ArtSync.json with the other settings.

### Inkbunny

To upload to Inkbunny, you need to log in each time you run the program. Click
on the "click to log in" text next to the Inkbunny label on the bottom of the
window; when Inkbunny finishes logging in, additional controls will appear.

Posting to Tumblr
-----------------

When you're logged into DeviantArt with your API key, your four most recent
submissions will appear along the left side of the window. Click the thumbnail
to load the full image and fill in the information boxes.

The general layout of the window is as follows (top to bottom). Checkboxes are
represented by the symbol [X].

* Preview button (click this button to preview the Tumblr post description as
  it will appear on the site)
* Now button (disable this if you want to specify a time/date in the past for
  the Tumblr post to be listed under)
* [X] Header template (HTML)
*     Title - any appearance of {TITLE} will be replaced with this text
* [X] Body template (HTML)
* [X] Footer template (HTML)
*     URL - any appearance of {URL} will be replaced with this text
* [X] Tags for this submission on DeviantArt
* [X] Other tags (default value is in settings)
* [X] #daXXXXXX tag
* Post to Tumblr button

When you post to Tumblr, the header, body and footer templates will be put
together, and occurences of {TITLE} or {URL} will be replaced with the
appropriate values. This will form the photo caption / text.

The tags will be combined and separated by space. The #daXXXXXX tag can be
added to tie your Tumblr post back to the original on DeviantArt, so ArtSync
can find it later and offer to update an existing post instead of adding a new
one. To update a post previously made with ArtSync, load the corresponding
DeviantArt submission (the post URL will appear at the bottom of the window)
click "Post to Tumblr." Updating only works with Tumblr, not Twitter or
Inkbunny.

If a checkbox is not checked, the content in the adajcent box will not be used.

Settings
--------

Settings are stored in the file ArtSync.json.

* Defaults
  * HeaderHTML: default header template.
  * FooterHTML: default footer template.
  * Tags: default tags for the second tags box, in case you want to use the
    same tags on every post.
  * IncludeDATag: determines whether the #daXXXXXX tag is included by
    default.

* DeviantArt
  * TBD

* Tumblr
  * Blog: name of the blog to post to.
  * TokenKey: The key of your OAuth token for Tumblr authentication.
  * TokenSecret: The secret part of the Tumblr OAuth token. The key and secret
    together are how ArtSync accesses your Tumblr account.
  * AutoSidePadding: If this is enabled and you post an image to Tumblr that
    is taller than it is wide, ArtSync will add transparent padding to the
    left and right sides of the image so it doesn't appear too large on the
    Tumblr dashboard.
  * FindPreviousPost: determines whether ArtSync will search your Tumblr
    for posts with a matching #daXXXXXX tag when you load a DeviantArt
    submission. If you disable this feature, ArtSync will not offer to update
    existing posts.

* Twitter / Inkbunny
  * No documentation yet, but it works.

Compiling from Source
---------------------

This project was built with Visual Studio 2017.

The file OAuthConsumer.cs is missing from the ArtSync project. Get your own
OAuth keys, then put the following into OAuthConsumer.cs:

namespace ArtSync {
	public static class OAuthConsumer {
		public static class Tumblr {
			public static string CONSUMER_KEY = "consumer key goes here";
			public static string CONSUMER_SECRET = "secret key goes here";
		}
		public static class Twitter {
			public static string CONSUMER_KEY = "consumer key goes here";
			public static string CONSUMER_SECRET = "secret key goes here";
		}
		public static class DeviantArt {
			public static string CLIENT_ID = "client_id goes here";
			public static string CLIENT_SECRET = "client_secret goes here";
		}
	}
}
