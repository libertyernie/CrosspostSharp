CrosspostSharp 5.0
==================

Source: https://github.com/libertyernie/CrosspostSharp

------------------

CrosspostSharp is a Windows desktop application (written mostly in C#) that
loads individual submissions from art sites and lets you post them to your
other accounts, maintaining the title, description, and tags.

There are currently no plans to post builds of CrosspostSharp 5.0+ to GitHub
(given the other, more fully-featured crossposting applications that are out
there, I don't think this application is all that useful to anyone who's not
already a .NET developer).

Supported Sites
---------------

Use the Tools menu to add and remove accounts.

* DeviantArt (uses [DeviantArtFs](https://github.com/IsaacSchemm/DeviantArtFs))
* Fur Affinity (uses [FAExport](https://faexport.spangle.org.uk/) for viewing
  posts and [FurAffinityFs](https://github.com/IsaacSchemm/FurAffinityFs) for
  upload)
* Furry Network
* Inkbunny
* Mastodon (uses [Pleronet](https://github.com/Solexid/Pleronet))
* Pixelfed (uses [Pleronet](https://github.com/Solexid/Pleronet))
* Weasyl

You can also open or save local files in PNG or JPEG format.

Credentials
-----------

Credentials are stored in the file CrosspostSharp.json. This includes tokens
that give CrosspostSharp access to your accounts. Make sure you keep
this file safe!

Compiling from Source
---------------------

This project can be built with Visual Studio 2022.

The file OAuthConsumer.cs is missing from the CrosspostSharp3 project. Get your own
OAuth keys, then put something like the following into OAuthConsumer.cs:

    namespace CrosspostSharp {
        public static class OAuthConsumer {
            public static class DeviantArt {
                public static string CLIENT_ID = "client_id goes here";
                public static string CLIENT_SECRET = "client_secret goes here";
            }
        }
    }
