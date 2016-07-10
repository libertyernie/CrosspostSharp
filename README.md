WeasylSync 1.1
==============

Download (1.0): https://dl.dropboxusercontent.com/u/57430384/WeasylSync%201.0.zip  
Mirror (1.0): http://www.mediafire.com/download/4tbcnkx4hn8zawf/WeasylSync_1.0.zip  
Source: https://github.com/libertyernie/WeasylSync

--------------------

WeasylSync is a Windows desktop application that loads individual submissions
from Weasyl and lets you post them to your Tumblr and Inkbunny accounts,
maintaining the title, description, and tags.

Only visual submissions (artwork) are supported at this time.

Requirements:

* Windows Vista or higher
* .NET Framework 4.5 or higher

Authentication
--------------

To use this application, you must connect it to both your Weasyl and Tumblr
accounts. Information for both will be stored in the file WeasylSync.json, so
keep that file secure!

### Weasyl

You can obtain a Weasyl API key at https://www.weasyl.com/control/apikeys.
Copy the API key from the page and paste it into the Options dialog in
WeasylSync (Tools -> Options).

You can revoke an API key at any time by going to the same page.

### Tumblr

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign into Tumblr using OAuth. Once signed in, the OAuth token
will be stored in WeasylSync.json with the other settings.

If you are already logged into Tumblr in Internet Explorer, you can
right-click the Sign In button to use your existing IE session and aboid
having to re-enter your username and password.

### Inkbunny

To upload to Inkbunny, you need to log in each time you run the program. Click
on the "click to log in" text next to the Inkbunny label on the bottom of the
window; when Inkbunny finishes logging in, additional controls will appear.

Posting to Tumblr
-----------------

When you're logged into Weasyl with your API key, your four most recent
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
* [X] Tags for this submission on Weasyl
* [X] Other tags (default value is in settings)
* [X] #weasylXXXXXX tag
* Post to Tumblr button

When you post to Tumblr, the header, body and footer templates will be put
together, and occurences of {TITLE} or {URL} will be replaced with the
appropriate values. This will form the photo caption / text.

The tags will be combined and separated by space. The #weasylXXXXXX tag can be
added to tie your Tumblr post back to the original on Weasyl, so WeasylSync
can find it later and offer to update an existing post instead of adding a new
one. To update a post previously made with WeasylSync, load the corresponding
Weasyl submission (the post URL will appear at the bottom of the window) and
click "Post to Tumblr."

If a checkbox is not checked, the content in the adajcent box will not be used.

Settings
--------

Settings are stored in the file WeasylSync.json.

* Defaults
  * HeaderHTML: default header template.
  * FooterHTML: default footer template.
  * Tags: default tags for the second tags box, in case you want to use the
    same tags on every post.
  * IncludeWeasylTag: determines whether the #weasylXXXXXX tag is included by
    default.

* Weasyl
  * API key: discussed above. Your Weasyl username will be detected automatically.

* Tumblr
  * Blog: name of the blog to post to.
  * TokenKey: The key of your OAuth token for Tumblr authentication.
  * TokenSecret: The secret part of the Tumblr OAuth token. The key and secret
    together are how WeasylSync accesses your Tumblr account.
  * AutoSidePadding: If this is enabled and you post an image to Tumblr that
    is taller than it is wide, WeasylSync will add transparent padding to the
    left and right sides of the image so it doesn't appear too large on the
    Tumblr dashboard.
  * FindPreviousPost: determines whether WeasylSync will search your Tumblr
    for posts with a matching #weasylXXXXXX tag when you load a Weasyl
    submission. If you disable this feature, WeasylSync will not offer to update
    existing posts.

Compiling from Source
---------------------

This project was built with Visual Studio 2015.

The file OAuthConsumer.cs is missing from the WeasylSync project. Get your own
Tumblr OAuth keys at https://www.tumblr.com/oauth/apps, then put the following
into OAuthConsumer.cs:

namespace WeasylSync {
	public static class OAuthConsumer {
		public static string CONSUMER_KEY = "consumer key goes here";
		public static string CONSUMER_SECRET = "secret key goes here";
	}
}

Notes
-----

* Although Weasyl generally offers submission info without any authentication,
  the API key is required in this program because it allows you to fetch info
  on submissions with a rating above "general".
