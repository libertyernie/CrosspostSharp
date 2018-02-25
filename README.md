CrosspostSharp 2.2.2
====================

Source: https://github.com/libertyernie/CrosspostSharp

--------------------

CrosspostSharp is a Windows desktop application that loads individual
submissions from art sites and lets you post them to your other
accounts, maintaining the title, description, and tags.

Only visual submissions (artwork) are supported at this time.

Before trying this application, you should try PostyBirb
(http://postybirb.weebly.com/); it has more features and supports more sites.

Requirements:

* Internet Explorer 11
* .NET Framework 4.6.1 or higher

Supported Sites
---------------

* DeviantArt / sta.sh
* Flickr
* FurAffinity (read-only)
* Inkbunny
* Pixiv (read-only)
* Tumblr
* Twitter
* Weasyl (read-only)

Coming soon:
* FurryNetwork

You can also read from a local folder and save images to local files.

Additionally, you can install Java and grab [efc.jar]
(https://github.com/libertyernie/Furry-Crossposter/releases),
which will add support for posting to Weasyl, FurAffinity, and other furry sites.
efc.jar is a fork of another crossposting app, with the ability to import from
CrosspostSharp added and certain other things removed. See its page for more
information.

Logging In
----------

To use this application, you must connect it to one or more of your accounts.
Information for all accounts will be stored in the file CrosspostSharp.json,
so keep that file secure!

In most cases, you'll use the "Sign In" button in Tools -> Options to launch a
browser window, which will let you sign in using OAuth. Once signed in, the
OAuth token will be stored in CrosspostSharp.json.

When you're logged into a source site, your three most recent
submissions will appear along the left side of the window. Click the thumbnail
to load the full image and fill in the information boxes.

#### DeviantArt

You can upload images to DeviantArt or just to Sta.sh, and you can also use
either one as a source site.

#### Flickr

If you have script errors on the page while trying to sign in, close and then
restart CrossposterSharp.

#### Pixiv

Your Pixiv username and password will be stored in plaintext in CrosspostSharp.json.

#### Tumblr

Only photo posts will be shown. Reblogs will be included, but CrosspostSharp won't
allow you to post them to other sites.

#### Twitter

Retweets, and tweets without photos, will be omitted.

The browser window uses Internet Explorer internally, so you may see a notice
from Twitter that you have logged in with Internet Explorer from a new device.

#### Weasyl

You can obtain a Weasyl API key at https://www.weasyl.com/control/apikeys.
Copy the API key from the page and paste it into the Options dialog in
CrosspostSharp (Tools -> Options).

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
* Append link (link will be appended to the tweet; this also lets CrosspostSharp find
  the tweet later and show you a link)

Settings
--------

Settings are stored in the file CrosspostSharp.json.

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
  * RefreshToken: Gives CrosspostSharp access to your DeviantArt account. CrosspostSharp
  will acquire a new token and update CrosspostSharp.json every time it is launched.

* Pixiv
  * Username
  * Password

* Tumblr
  * Blog: name of the blog to post to.
  * TokenKey: The key of your OAuth token for Tumblr authentication.
  * TokenSecret: The secret part of the Tumblr OAuth token. The key and secret
    together are how CrosspostSharp accesses your Tumblr account.
  * AutoSidePadding: If this is enabled and you post an image to Tumblr that
    is taller than it is wide, CrosspostSharp will add transparent padding to the
    left and right sides of the image so it doesn't appear too large on the
    Tumblr dashboard.

* Twitter
  * TokenKey: Gives CrosspostSharp access to your Twitter account (with TokenSecret).
  * TokenSecret: Gives CrosspostSharp access to your Twitter account (with TokenKey).

* Flickr
  * TokenKey: Gives CrosspostSharp access to your Flickr account (with TokenSecret).
  * TokenSecret: Gives CrosspostSharp access to your Flickr account (with TokenKey).

* Inkbunny
  * Sid: The "sid" value used in the Inkbunny API.
  * UserId: Your Inkbunny user ID.

Compiling from Source
---------------------

This project can be built with Visual Studio 2015 or 2017.

The file OAuthConsumer.cs is missing from the CrosspostSharp project. Get your own
OAuth keys, then put the following into OAuthConsumer.cs:

    namespace CrosspostSharp {
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
