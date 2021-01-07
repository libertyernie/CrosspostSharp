CrosspostSharp 4.0
==================

Source: https://github.com/libertyernie/CrosspostSharp

------------------

CrosspostSharp is a Windows desktop application (written mostly in C#, with a bit of C++) that
loads individual submissions from art sites and lets you post them to your
other accounts, maintaining the title, description, and tags.

In most cases, only photo posts are supported. However, Twitter and Tumblr
text posts and DeviantArt status updates will be recognized as simple text
posts, and you will be able to crosspost them easily without uploading a
photo.

Supported Sites
---------------

Use the Tools menu to add and remove accounts. Multiple accounts per site are
supported on all sites except DeviantArt and Sta.sh.

On Tumblr and Furry Network, CrosspostSharp lets you use read from and post to
multiple blogs/characters using a single account.

* DeviantArt / Sta.sh
* Fur Affinity
* Furry Network
* Inkbunny
* Mastodon
* Tumblr
* Twitter
* Weasyl

You can also open or save local files in PNG or JPEG format.

Credentials
-----------

Credentials are stored in the file CrosspostSharp.json. This includes tokens
are used to give CrosspostSharp access to your accounts. Make sure you keep
this file safe!

Compiling from Source
---------------------

This project can be built with Visual Studio 2019.

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
        }
    }
