using KeepToJoplin;
using System;
using System.IO;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Google Keep to Joplin converter");

if (args.Length != 2)
{
	Console.WriteLine("Usage: KeepToJoplin [input directory] [output directory]");
	return;
}

var inputDirectory = args[0];
var outputDirectory = args[1];

if (!Directory.Exists(inputDirectory))
{
	Console.WriteLine("Error: input directory not found");
	return;
}

var outputDirectoryInfo = new DirectoryInfo(outputDirectory);
if (!outputDirectoryInfo.Exists)
{
	Console.WriteLine("Error: output directory not found");
	return;
}
if (outputDirectoryInfo.GetFileSystemInfos().Length > 0)
{
	Console.WriteLine("Error: output directory is not empty");
	return;
}

Console.WriteLine("Starting...");

var converter = new KeepToJoplinConverter(new IdProvider(), new DateTimeProvider());
converter.OnLog = Console.WriteLine;

try
{
	converter.Convert(inputDirectory, outputDirectory);
}
catch (ConvertError e)
{
	Console.WriteLine("Error: " + e.Message);
}

Console.WriteLine("Finished.");
