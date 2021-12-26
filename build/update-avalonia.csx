// This script copies the files we use from Avalonia into the Modern.WindowKit.Backend.Mac repo.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// Paths to the local repositories
// This assumes the repositories are siblings, but you can change that here if needed
// - /code
//   - /Avalonia
//   - /Modern.WindowKit.Backend.Mac

// Paths relative to the script location
string avalonia_repo_path = Path.Combine ("..", "..", "Avalonia");
string modern_forms_repo_path = "..";

string avalonia_path = Path.Combine (avalonia_repo_path);
string modern_forms_path = modern_forms_repo_path;

// Mac Native Bits
var native_dir = Path.Combine (avalonia_path, "native", "Avalonia.Native");
var mfmb_dir = modern_forms_path;

DirectoryCopy (Path.Combine (native_dir, "inc"), Path.Combine (mfmb_dir, "inc"), true);
DirectoryCopy (Path.Combine (native_dir, "src", "OSX"), Path.Combine (mfmb_dir, "src", "native"), true);

CopyFile ("src/Avalonia.Native/avn.idl", "src/avn.idl");

private void CopyFile (string src, string dst)
{
    var full_src = Path.Combine (avalonia_path, src);
    var full_dst = Path.Combine (modern_forms_path, dst);

    var text = File.ReadAllText (full_src);

    var dest_lines = File.Exists (full_dst) ? CommentDiffs (text, full_dst) : new[] { text };
    File.WriteAllLines (full_dst, dest_lines, Encoding.UTF8);
}

// We prefer to comment unused stuff so we can tell stuff we've disabled versus new stuff in diffs
private string Comment (string text, string str) => text.Replace (str, "//" + str);

// Try to remove stuff we've commented from the new files for easier diffs
private string[] CommentDiffs (string text, string dest)
{
    var dest_lines = File.ReadAllLines (dest);

    var src_lines = new List<string> ();
    using var sw = new StringReader (text);
    string s;

    while ((s = sw.ReadLine ()) != null)
        src_lines.Add (s);

    for (var i = 0; i < Math.Min (src_lines.Count, dest_lines.Length); i++) {
        if (StripWhitespace ("//" + src_lines[i]) == StripWhitespace (dest_lines[i]))
            src_lines[i] = dest_lines[i];
    }

    return src_lines.ToArray ();
}

private string StripWhitespace (string str) => str.Replace (" ", "").Replace ("\t", "");

private static void DirectoryCopy (string sourceDirName, string destDirName, bool copySubDirs)
{
    // Get the subdirectories for the specified directory.
    var dir = new DirectoryInfo (sourceDirName);

    if (!dir.Exists)
    {
        throw new DirectoryNotFoundException(
            "Source directory does not exist or could not be found: "
            + sourceDirName);
    }

    var dirs = dir.GetDirectories ();
    
    // If the destination directory doesn't exist, create it.
    if (!Directory.Exists (destDirName))
    {
        Directory.CreateDirectory (destDirName);
    }
    
    // Get the files in the directory and copy them to the new location.
    var files = dir.GetFiles ();
    
    foreach (FileInfo file in files)
    {
        var temppath = Path.Combine (destDirName, file.Name);
        file.CopyTo (temppath, true);
    }

    // If copying subdirectories, copy them and their contents to new location.
    if (copySubDirs)
    {
        foreach (var subdir in dirs)
        {
            var temppath = Path.Combine (destDirName, subdir.Name);
            DirectoryCopy (subdir.FullName, temppath, copySubDirs);
        }
    }
}