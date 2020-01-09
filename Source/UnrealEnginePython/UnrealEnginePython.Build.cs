// Copyright 1998-2016 Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Tools.DotNETCommon;

public class UnrealEnginePython : ModuleRules
{

    // leave this string as empty for triggering auto-discovery of python installations...
    private string pythonHome = "";
    // otherwise specify the path of your python installation
    //private string pythonHome = "C:/Program Files/Python36";
    // this is an example for Homebrew on Mac
    //private string pythonHome = "/usr/local/Cellar/python3/3.6.0/Frameworks/Python.framework/Versions/3.6/";
    // on Linux an include;libs syntax is expected:
    //private string pythonHome = "/usr/local/include/python3.6;/usr/local/lib/libpython3.6.so"

    private string[] windowsKnownPaths =
    {
        "../../../../../ThirdParty/Python3",
        "C:/Program Files/Python37",
        "C:/Program Files/Python36",
        "C:/Program Files/Python35",
        "C:/Python27",
        "C:/IntelPython35"
    };

    private string[] macKnownPaths =
    {
        "/Library/Frameworks/Python.framework/Versions/3.7",
        "/Library/Frameworks/Python.framework/Versions/3.6",
        "/Library/Frameworks/Python.framework/Versions/3.5",
        "/Library/Frameworks/Python.framework/Versions/2.7",
        "/System/Library/Frameworks/Python.framework/Versions/3.7",
        "/System/Library/Frameworks/Python.framework/Versions/3.6",
        "/System/Library/Frameworks/Python.framework/Versions/3.5",
        "/System/Library/Frameworks/Python.framework/Versions/2.7"
    };

    private string[] linuxKnownIncludesPaths =
    {
        "/usr/local/include/python3.7",
        "/usr/local/include/python3.7m",
        "/usr/local/include/python3.6",
        "/usr/local/include/python3.6m",
        "/usr/local/include/python3.5",
        "/usr/local/include/python3.5m",
        "/usr/local/include/python2.7",
        "/usr/include/python3.7",
        "/usr/include/python3.7m",
        "/usr/include/python3.6",
        "/usr/include/python3.6m",
        "/usr/include/python3.5",
        "/usr/include/python3.5m",
        "/usr/include/python2.7",
    };

    private string[] linuxKnownLibsPaths =
    {
        "/usr/local/lib/libpython3.7.so",
        "/usr/local/lib/libpython3.7m.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.7.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.7m.so",
        "/usr/local/lib/libpython3.6.so",
        "/usr/local/lib/libpython3.6m.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.6.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.6m.so",
        "/usr/local/lib/libpython3.5.so",
        "/usr/local/lib/libpython3.5m.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.5.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython3.5m.so",
        "/usr/local/lib/libpython2.7.so",
        "/usr/local/lib/x86_64-linux-gnu/libpython2.7.so",
        "/usr/lib/libpython3.7.so",
        "/usr/lib/libpython3.7m.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.7.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.7m.so",
        "/usr/lib/libpython3.6.so",
        "/usr/lib/libpython3.6m.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.6.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.6m.so",
        "/usr/lib/libpython3.5.so",
        "/usr/lib/libpython3.5m.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.5.so",
        "/usr/lib/x86_64-linux-gnu/libpython3.5m.so",
        "/usr/lib/libpython2.7.so",
        "/usr/lib/x86_64-linux-gnu/libpython2.7.so",
    };

#if WITH_FORWARDED_MODULE_RULES_CTOR
    public UnrealEnginePython(ReadOnlyTargetRules Target) : base(Target)
#else
    public UnrealEnginePython(TargetInfo Target)
#endif
    {
        // @third party code - BEGIN Bebylon - #ThirdParty-Python: WITH_KNL_PYEXT - Workaround for our deployment process
        PublicDefinitions.Add("WITH_KNL_PYEXT=1");
        // @third party code - END Bebylon


        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;
        // @third party code - BEGIN Bebylon - #ThirdParty-Python: Add UnrealEnginePython Plugin PrivatePCH file
        PrivatePCHHeaderFile = Path.Combine(ModuleDirectory, "Private/UnrealEnginePythonPrivatePCH.h");
        // @third party code - END Bebylon
        string enableUnityBuild = System.Environment.GetEnvironmentVariable("UEP_ENABLE_UNITY_BUILD");
        bFasterWithoutUnity = string.IsNullOrEmpty(enableUnityBuild);

        PublicIncludePaths.AddRange(
            new string[] {
                Path.Combine(ModuleDirectory, "Public"),
				// ... add public include paths required here ...
            }
            );


        PrivateIncludePaths.AddRange(
            new string[] {
                Path.Combine(ModuleDirectory, "Private"),
				// ... add other private include paths required here ...
                Path.Combine(EngineDirectory, "Source/Runtime/Slate/Private"),
                Path.Combine(EngineDirectory, "Source/Editor/Sequencer/Private"),
            }
            );


        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                "Sockets",
                "Networking"
				// ... add other public dependencies that you statically link with here ...
			}
            );


        PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "CoreUObject",
                "Engine",
                "InputCore",
                "Slate",
                "SlateCore",
                "MovieScene",
                "LevelSequence",
                "HTTP",
                "UMG",
                "AppFramework",
                "RHI",
                "Voice",
                "RenderCore",
                "MovieSceneCapture",
                "Landscape",
                "Foliage",
                "AIModule"
				// ... add private dependencies that you statically link with here ...
			}
            );


#if WITH_FORWARDED_MODULE_RULES_CTOR
        BuildVersion Version;
        if (BuildVersion.TryRead(BuildVersion.GetDefaultFileName(), out Version))
        {
            if (Version.MinorVersion >= 18)
            {
                PrivateDependencyModuleNames.Add("ApplicationCore");
            }
        }
#endif


        DynamicallyLoadedModuleNames.AddRange(
            new string[]
            {
				// ... add any modules that your module loads dynamically here ...
			}
            );

#if WITH_FORWARDED_MODULE_RULES_CTOR
        if (Target.bBuildEditor)
#else
        if (UEBuildConfiguration.bBuildEditor)
#endif
        {
            PrivateDependencyModuleNames.AddRange(new string[]{
                "UnrealEd",
                "LevelEditor",
                "BlueprintGraph",
                "Projects",
                "Sequencer",
                "SequencerWidgets",
                "AssetTools",
                "LevelSequenceEditor",
                "MovieSceneTools",
                "MovieSceneTracks",
                "CinematicCamera",
                "EditorStyle",
                "GraphEditor",
                "UMGEditor",
                "AIGraph",
                "RawMesh",
                "DesktopWidgets",
                "EditorWidgets",
                "FBX",
                "Persona",
                "PropertyEditor",
                "LandscapeEditor",
                "MaterialEditor"
            });
        }

        if ((Target.Platform == UnrealTargetPlatform.Win64) || (Target.Platform == UnrealTargetPlatform.Win32))
        {
            if (pythonHome == "")
            {
                pythonHome = DiscoverPythonPath(windowsKnownPaths, "Win64");
                if (pythonHome == "")
                {
                    throw new System.Exception("Unable to find Python installation");
                }
            }
            Log.TraceInformation("Using Python at: {0}", pythonHome);
            PublicSystemIncludePaths.Add(pythonHome);
            string libPath = GetWindowsPythonLibFile(pythonHome);
            PublicLibraryPaths.Add(Path.GetDirectoryName(libPath));
            //PublicAdditionalLibraries.Add(libPath);
            //string py3DllPath = Path.Combine(pythonHome, "python3.dll");
            //string py36DllPath = Path.Combine(pythonHome, "python36.dll");
            //Log.TraceInformation(py3DllPath);
            //Log.TraceInformation(py36DllPath);
            //PublicDelayLoadDLLs.Add(py3DllPath); 
            //PublicDelayLoadDLLs.Add(py36DllPath);
            //RuntimeDependencies.Add(py3DllPath);
            //RuntimeDependencies.Add(py36DllPath);
            //AdditionalLinkArguments += " /NODEFAULTLIB:\"python36_d\"";
            //AdditionalLinkArguments += " /NODEFAULTLIB:\"python3\"";
            //AdditionalLinkArguments += " /NODEFAULTLIB:\"python36\"";
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            if (pythonHome == "")
            {
                pythonHome = DiscoverPythonPath(macKnownPaths, "Mac");
                if (pythonHome == "")
                {
                    throw new System.Exception("Unable to find Python installation");
                }
            }
            Log.TraceInformation("Using Python at: {0}", pythonHome);
            PublicIncludePaths.Add(pythonHome);
            string libPath = GetMacPythonLibFile(pythonHome);
            PublicLibraryPaths.Add(Path.GetDirectoryName(libPath));
            PublicDelayLoadDLLs.Add(libPath);
        }
        else if (Target.Platform == UnrealTargetPlatform.Linux)
        {
            if (pythonHome == "")
            {
                string includesPath = DiscoverLinuxPythonIncludesPath();
                if (includesPath == null)
                {
                    throw new System.Exception("Unable to find Python includes, please add a search path to linuxKnownIncludesPaths");
                }
                string libsPath = DiscoverLinuxPythonLibsPath();
                if (libsPath == null)
                {
                    throw new System.Exception("Unable to find Python libs, please add a search path to linuxKnownLibsPaths");
                }
                PublicIncludePaths.Add(includesPath);
                PublicAdditionalLibraries.Add(libsPath);

            }
            else
            {
                string[] items = pythonHome.Split(';');
                PublicIncludePaths.Add(items[0]);
                PublicAdditionalLibraries.Add(items[1]);
            }
        }
#if WITH_FORWARDED_MODULE_RULES_CTOR
        else if (Target.Platform == UnrealTargetPlatform.Android)
        {
            PublicIncludePaths.Add(System.IO.Path.Combine(ModuleDirectory, "../../android/python35/include"));
            PublicLibraryPaths.Add(System.IO.Path.Combine(ModuleDirectory, "../../android/armeabi-v7a"));
            PublicAdditionalLibraries.Add("python3.5m");

            string APLName = "UnrealEnginePython_APL.xml";
            string RelAPLPath = Utils.MakePathRelativeTo(System.IO.Path.Combine(ModuleDirectory, APLName), Target.RelativeEnginePath);
            AdditionalPropertiesForReceipt.Add("AndroidPlugin", RelAPLPath);
        }
#endif

    }

    private bool IsPathRelative(string Path)
    {
        bool IsRooted = Path.StartsWith("\\", System.StringComparison.Ordinal) || // Root of the current directory on Windows. Also covers "\\" for UNC or "network" paths.
                        Path.StartsWith("/", System.StringComparison.Ordinal) ||  // Root of the current directory on Windows, root on UNIX-likes.
                                                                                  // Also covers "\\", considering normalization replaces "\\" with "//".
                        (Path.Length >= 2 && char.IsLetter(Path[0]) && Path[1] == ':'); // Starts with "<DriveLetter>:"
        return !IsRooted;
    }

    /// <summary>
    /// Regex that matches environment variables in $(Variable) format.
    /// </summary>
    static Regex EnvironmentVariableRegex = new Regex("\\$\\(([\\d\\w]+)\\)");

    /// <summary>
    /// Replaces the environment variables references in a string with their values.
    /// </summary>
    public string UEExpandEnvironmentVariables(string Text)
    {
        Text = Text.Replace("%GAMEDIR%", "$(GAMEDIR)");
        Text = Utils.ExpandVariables(Text, new Dictionary<string, string>(){ { "GAMEDIR", Target.ProjectFile.Directory.ToNormalizedPath() + "/" } } );
        return Text;
    }

    private string DiscoverPythonPath(string[] knownPaths, string binaryPath)
    {
        // insert the PYTHONHOME content as the first known path
        List<string> paths = new List<string>(knownPaths);

        paths.Insert(0, Path.Combine(ModuleDirectory, "../../Binaries", binaryPath));
        string environmentPath = System.Environment.GetEnvironmentVariable("PYTHONHOME");

        if (!string.IsNullOrEmpty(environmentPath))
        { paths.Insert(0, environmentPath); }

        {
            List<string> absoluteHomePaths;
            ConfigHierarchy pluginIni = ConfigCache.ReadHierarchy(ConfigHierarchyType.Engine, Target.ProjectFile.Directory, Target.Platform);
            pluginIni.GetArray("Python", "Home", out absoluteHomePaths);
            foreach (string absoluteHomePath in absoluteHomePaths)
            {
                string expandedPath = UEExpandEnvironmentVariables(absoluteHomePath);
                if (!string.IsNullOrEmpty(expandedPath))
                { paths.Insert(0, expandedPath); }
            }
        }

        {
            string relativeHomePath = "";
            ConfigHierarchy pluginIni = ConfigCache.ReadHierarchy(ConfigHierarchyType.Engine, Target.ProjectFile.Directory, Target.Platform);
            pluginIni.GetString("Python", "RelativeHome", out relativeHomePath);
            relativeHomePath = UEExpandEnvironmentVariables(relativeHomePath);
            if (!string.IsNullOrEmpty(relativeHomePath))
            { paths.Insert(0, Path.Combine(EngineDirectory, relativeHomePath)); }
        }

        // look in an alternate custom location
        environmentPath = System.Environment.GetEnvironmentVariable("UNREALENGINEPYTHONHOME");
        if (!string.IsNullOrEmpty(environmentPath))
            paths.Insert(0, environmentPath);

        foreach (string path in paths)
        {
            string actualPath = path;

            if (IsPathRelative(actualPath))
            {
                actualPath = Path.GetFullPath(Path.Combine(ModuleDirectory, actualPath));
            }

            string headerFile = Path.Combine(actualPath, "include", "Python.h");
            if (File.Exists(headerFile))
            {
                return actualPath;
            }
            // this is mainly useful for OSX
            headerFile = Path.Combine(actualPath, "Headers", "Python.h");
            if (File.Exists(headerFile))
            {
                return actualPath;
            }
        }
        return "";
    }

    private string DiscoverLinuxPythonIncludesPath()
    {
        List<string> paths = new List<string>(linuxKnownIncludesPaths);
        paths.Insert(0, Path.Combine(ModuleDirectory, "../../Binaries", "Linux", "include"));
        foreach (string path in paths)
        {
            string headerFile = Path.Combine(path, "Python.h");
            if (File.Exists(headerFile))
            {
                return path;
            }
        }
        return null;
    }

    private string DiscoverLinuxPythonLibsPath()
    {
        List<string> paths = new List<string>(linuxKnownLibsPaths);
        paths.Insert(0, Path.Combine(ModuleDirectory, "../../Binaries", "Linux", "lib"));
        paths.Insert(0, Path.Combine(ModuleDirectory, "../../Binaries", "Linux", "lib64"));
        foreach (string path in paths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }
        return null;
    }

    private string GetMacPythonLibFile(string basePath)
    {
        // first try with python3
        for (int i = 9; i >= 0; i--)
        {
            string fileName = string.Format("libpython3.{0}.dylib", i);
            string fullPath = Path.Combine(basePath, "lib", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
            fileName = string.Format("libpython3.{0}m.dylib", i);
            fullPath = Path.Combine(basePath, "lib", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        // then python2
        for (int i = 9; i >= 0; i--)
        {
            string fileName = string.Format("libpython2.{0}.dylib", i);
            string fullPath = Path.Combine(basePath, "lib", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
            fileName = string.Format("libpython2.{0}m.dylib", i);
            fullPath = Path.Combine(basePath, "lib", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        throw new System.Exception("Invalid Python installation, missing .dylib files");
    }

    private string GetWindowsPythonLibFile(string basePath)
    {
        // just for usability, report if the pythonHome is not in the system path
        string[] allPaths = System.Environment.GetEnvironmentVariable("PATH").Split(';');
        // this will transform the slashes in backslashes...
        string checkedPath = !string.IsNullOrWhiteSpace(basePath) 
            ? new System.Uri(Path.GetFullPath(System.Environment.ExpandEnvironmentVariables(basePath)).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).LocalPath
            : basePath;
        
        bool found = false;
        foreach (string item in allPaths)
        {
            string checkedItem = !string.IsNullOrWhiteSpace(item)
                ? new System.Uri(Path.GetFullPath(System.Environment.ExpandEnvironmentVariables(item)).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).LocalPath
                : item;
            if (checkedItem == checkedPath)
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            Log.TraceWarning("Your Python installation ({0}) is not in the system PATH environment variable.", checkedPath);
            Log.TraceWarning("Ensure your python paths are set in GlobalConfig (DefaultEngine.ini) so the path can be corrected at runtime.");
        }
        // first try with python3
        for (int i = 9; i >= 0; i--)
        {
            string fileName = string.Format("python3{0}.lib", i);
            string fullPath = Path.Combine(basePath, "libs", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        // then python2
        for (int i = 9; i >= 0; i--)
        {
            string fileName = string.Format("python2{0}.lib", i);
            string fullPath = Path.Combine(basePath, "libs", fileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        throw new System.Exception("Invalid Python installation, missing .lib files");
    }
}
