CrosspostSharp 3
================

Source: https://github.com/libertyernie/CrosspostSharp

--------------------

CrosspostSharp is a Windows desktop application that loads individual
submissions from art sites and lets you post them to your other
accounts, maintaining the title, description, and tags.

Only visual submissions (artwork) are supported at this time.

If you're using CrosspostSharp to upload artwork, you should also try PostyBirb
(http://postybirb.weebly.com/); it has more features and supports more sites.

Requirements:

* Internet Explorer 11
* .NET Framework 4.6.1 or higher

Supported Sites
---------------

Use the Tools menu to add and remove accounts.

* DeviantArt / Sta.sh (only one account supported at a time; cannot read scraps)
* FurAffinity (read-only)
* Furry Network
* Inkbunny
* Tumblr
* Twitter
* Weasyl (read-only)

Coming later:
* Flickr
* Pixiv (read-only)

You can also use a Media RSS feed as a source or open or save local files.

Additionally, you can install Java and put [efc.jar]
(https://github.com/libertyernie/Furry-Crossposter/releases),
in the same folder as CrosspostSharp.exe, which will add support for posting
to Weasyl and FurAffinity. efc.jar is a fork of another crossposting app
designed to work with CrosspostSharp. See its page for more information.

Settings
--------

Credentials are stored in the file CrosspostSharp.json. This includes tokens
are used to give CrosspostSharp access to your accounts. Make sure you keep
this file safe! (Note that for Pixiv, your actual password will be stored in
plaintext.)

Compiling from Source
---------------------

This project can be built with Visual Studio 2015 or 2017. When you clone the
Git repository, make sure you do it recursively (or you can manually clone all
the submodules.)

The file OAuthConsumer.cs is missing from the CrosspostSharp3 project. Get your own
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
