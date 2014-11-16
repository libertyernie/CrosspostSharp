WeasylSync
==========

https://github.com/libertyernie/WeasylSync

WeasylSync is a Windows desktop application that loads individual submissions
from Weasyl and lets you post them to your Tumblr account, maintaining the
title, description, and tags.

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
* [X] #weasyl\*\*\*\*\*\* tag

When you post to Tumblr, the header, body and footer templates will be put
together, and occurences of {TITLE} or {URL} will be replaced with the
appropriate values. This will form the photo caption / text.

The tags will be combined and separated by space. The #weasyl\*\*\*\*\*\* tag can be
added to tie your Tumblr post back to the original on Weasyl, so WeasylSync
can find it later and offer to update an existing post instead of adding a new
one. To update a post previously made with WeasylSync, load the corresponding
Weasyl submission (the post URL will appear at the bottom of the window) and
click "Post to Tumblr."

If a checkbox is not checked, the content in the adajcent box will not be used.

Settings
--------

* Weasyl
  * API key: discussed above. Your Weasyl username will be detected automatically.

* Tumblr
  * Blog: name of the blog to post to.
  * Header: default header template (HTML).
  * Footer: default footer template (HTML).
  * Tags: default tags for the second tags box, in case you want to use the
    same tags on every post.
  * TokenKey: The key of your OAuth token for Tumblr authentication.
  * TokenSecret: The secret part of the Tumblr OAuth token. The key and secret
    together are how WeasylSync accesses your Tumblr account.
  * IncludeWeasylTag: determines whether the #weasyl\*\*\*\*\*\* tag is included by
    default.
  * LookForWeasylTag: determines whether WeasylSync will search your Tumblr
    for posts with a matching #weasyl\*\*\*\*\*\* tag when you load a Weasyl
	submission. If you disable this feature, WeasylSync will not offer to update
	existing posts.

Notes
-----

* Although Weasyl generally offers submission info without any authentication,
  the API key is required in this program for two reasons: it allows you to
  fetch info on submissions with a rating above "general", and requiring the
  API key avoids Weasyl bug 471.
* The OAuth mechanism used by Tumblr (and many web sites) is technically not
  as secure in desktop and mobile applications as it is on the web:  
  http://welcome.totheinter.net/2011/01/12/stealing-passwords-is-easy-in-native-mobile-apps-despite-oauth/  
  However, there would be no easy way to avoid this problem without running an
  HTTP server on the local machine. In any case, OAuth is the only method
  Tumblr supports for posting to blogs.
