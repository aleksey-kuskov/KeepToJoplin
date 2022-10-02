#Google Keep to Joplin notes converter

Converts Google Keep notes from a given folder ("Keep" folder in Google Takeout zip file that includes *.json files) to "RAW - Joplin Export Directory" format in a given output folder, which can then be used to import into Joplin.

```
Usage: KeepToJoplin [input directory] [output directory]
```

All imported notes will be placed in a new "Keep Notes" notebook.

##WARNING
- To Do list notes will be empty. (*Search \*.json files for "listContent" to find these in source folder*)
- No markdown handling - note text is copied as is.
- Annotations are not looked at. (*wherever they are in Keep - cannot see them on mobile phone*)

##Note

After import you may need to click on some notebooks or notes for Joplin to start showing tags.

##Requirements

[.NET 6 Framework Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)

##Recommendation before importing notes

- Backup Joplin profile
- Backup synchronization local or cloud folder 
- Turn off Joplin synchronization - even with "Disabled" synchronization period Joplin still synchronizes notes!
