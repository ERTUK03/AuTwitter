# AuTwitter
Twitter bot made for scheduling and automatically posting tweets

# Description
C# Twitter bot with WinForms interface made with TweetSharp and hooked up to SQLite database. Database is necessary for saving created commands. To use it you need
to provide things like API Key, Secret API Key, Access Token and Secret Access Token.

# Functionality
Currently it supports text-only tweets and tweets with text and media. You create a command by specifying it's name, content(text), optionally selecting path to media
of your choice and selecting time. Next the command is added to database and list is refreshed. When right time comes, the command gets executed and is deleted from
database.

You can also remove command immediately by writing it's name.

Also to work properly you need to be granted elevated access to Twitter API.
