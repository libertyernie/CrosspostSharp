ArtSync 2.1
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

Supported Sources
-----------------

To use this application, you must connect it to one of your accounts.
Information for all accounts will be stored in the file ArtSync.json,
so keep that file secure!

When you're logged into a source site, your three most recent
submissions will appear along the left side of the window. Click the thumbnail
to load the full image and fill in the information boxes.

#### DeviantArt

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign in using OAuth. Once signed in, the OAuth token
will be stored in ArtSync.json.

You can upload images to DeviantArt or just to Sta.sh, and you can also use
either one as a source site.

#### Flickr

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign in using OAuth. Once signed in, the OAuth token
will be stored in ArtSync.json.

#### FurAffinity

Use the "Sign In" button in Tools -> Options to launch a dialog window that
asks for your username and password. This will obtain cookies that will be
stored in ArtSync.json.

#### Inkbunny

Use the "Sign In" button in Tools -> Options to launch a dialog window that
asks for your username and password. This will obtain a user ID and SID that
will be stored in ArtSync.json.

#### Tumblr

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign in using OAuth. Once signed in, the OAuth token
will be stored in ArtSync.json.

Only photo posts will be shown. Reblogs will be included, but ArtSync won't
allow you to post them to other sites.

#### Twitter

Use the "Sign In" button in Tools -> Options to launch a browser window, which
will let you sign in using OAuth. Once signed in, the OAuth token
will be stored in ArtSync.json.

Retweets, and tweets without photos, will be omitted.

The browser window uses Internet Explorer internally, so you may see a notice
from Twitter that you have logged in with Internet Explorer from a new device.

#### Weasyl

You can obtain a Weasyl API key at https://www.weasyl.com/control/apikeys.
Copy the API key from the page and paste it into the Options dialog in
ArtSync (Tools -> Options).

You can revoke an API key at any time by going to the same page.

Supported Destinations
----------------------

#### DeviantArt / sta.sh

* Title
* Artist comments (description) - if this is too small, you can make the window bigger
* Tags (separated by spaces, # is optional)
* Mature content (none, moderate, strict)
* Mature content classification (if applicable)
* Category
  * e.g. Cartoons & Comics > Digital Media > Cartoons > Vector
  * default is Scraps
* Gallery folders
  * Select one or more
  * default is Featured
* Sharing options
* License options
* Allow download
* Allow comments
* Request critique

You must agree to the submisison policy and terms of service before posting.

#### Flickr

* Title
* Description (limited HTML)
* Tags
* Permissions (public, friends, family)
* Safety level (safe / moderate / restricted)
* Content type (photo / screenshot / other)
* License (optional)
* Public/private

#### Inkbunny

* Title
* Description (BBCode)
* Keywords (separated by spaces, # is optional)
* Public
* Notify watchers
* Scraps
* Checkboxes for all four Inkbunny rating levels

#### Tumblr

* Preview button (click this button to preview the Tumblr post description as
  it will appear on the site)
* Now button (disable this if you want to specify a time/date in the past for
  the Tumblr post to be listed under)
* Header template (HTML) - default is the title, in bold text
* Body template (HTML) - default is the description
* Tags (separated by spaces, # is optional)
  * Include the tags from the original site (as shown above)
  * Additional tags to include (default: #art)
* Footer template (HTML)
  * URL - any appearance of {URL} will be replaced with this text
* Post to Tumblr button

When you post to Tumblr, the header, body and footer templates will be put
together, and occurences of {URL} will be replaced with the
appropriate value. This will form the photo caption / text.

If a checkbox is not checked, the content in the adajcent box will not be used.

#### Twitter

* Include title (repopulates tweet area)
* Include description (repopulates tweet area)
* Tweet area - the text that will be included in the tweet
* Potentially sensitive material
* Include image (image will be included as media in the tweet)
* Append link (link will be appended to the tweet; this also lets ArtSync find
  the tweet later and show you a link)

Settings
--------

Settings are stored in the file ArtSync.json.

* Defaults
  * HeaderHTML: default header template.
  * FooterHTML: default footer template.
  * Tags: default tags for the second tags box, in case you want to use the
    same tags on every post.
  * IncludeWeasylTag: determines whether the unique tag is included by
    default.

* Weasyl
  * API key: discussed above. Your Weasyl username will be detected
    automatically.

* DeviantArt
  * RefreshToken: Gives ArtSync access to your DeviantArt account. ArtSync
  will acquire a new token and update ArtSync.json every time it is launched.

* Tumblr
  * Blog: name of the blog to post to.
  * TokenKey: The key of your OAuth token for Tumblr authentication.
  * TokenSecret: The secret part of the Tumblr OAuth token. The key and secret
    together are how ArtSync accesses your Tumblr account.
  * AutoSidePadding: If this is enabled and you post an image to Tumblr that
    is taller than it is wide, ArtSync will add transparent padding to the
    left and right sides of the image so it doesn't appear too large on the
    Tumblr dashboard.

* Twitter
  * TokenKey: Gives ArtSync access to your Twitter account (with TokenSecret).
  * TokenSecret: Gives ArtSync access to your Twitter account (with TokenKey).

* Flickr
  * TokenKey: Gives ArtSync access to your Flickr account (with TokenSecret).
  * TokenSecret: Gives ArtSync access to your Flickr account (with TokenKey).

* Inkbunny
  * Sid: The "sid" value used in the Inkbunny API.
  * UserId: Your Inkbunny user ID.

Compiling from Source
---------------------

This project can be built with Visual Studio 2015 or 2017.

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
            public static class Flickr {
                public static string KEY = "consumer key goes here";
                public static string SECRET = "secret key goes here";
            }
        }
    }
