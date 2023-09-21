# UnrealObjectPtrTool

Alternative to [UnrealObjectPtrTool](https://docs.unrealengine.com/5.0/en-US/unreal-engine-5-migration-guide/) using C#.

Unreal Engine version seems to use Perforce and only exists in source version.


## To use

1. Navigate to your `DefaultEngine.ini` UHT config file located in your `Engine\Programs\UnrealHeaderTool\Config` directory.

2. Inside of your `DefaultEngine.ini` file, modify the following script:

```
NonEngineNativePointerMemberBehavior=AllowAndLog
```


3. Rebuild your project to ensure all the code is parsed by UHT. **You may need to delete Binaries and Intermediate folders.** 

4. Run `UnrealObjectPtrTool.exe` and follow the instructions.

## Examples

You can also provide direct path of log to the exe file.
```
UnrealObjectPtrTool.exe "d:\Repos\UnrealEngine\Engine\Programs\UnrealBuildTool\Log-backup-2023.09.21-11.38.14.txt"
```

## Additional Notes

Your UHT log may be named `Log_UHT.txt` or `UnrealHeaderTool.log` or any other file under that folder depending on how your project is compiled. You can navigate to either of the following folder directories:

```
C:\Users\USERNAME\AppData\Local\UnrealBuildTool\Log_UHT.txt
C:\Users\USERNAME\AppData\Local\UnrealHeaderTool\Saved\Logs\UnrealHeaderTool.log
Engine\Programs\UnrealBuildTool\Log_UHT.txt
Engine\Programs\UnrealBuildTool\Log.txt
```

This project reads only if no console parameters defined `C:\Users\USERNAME\AppData\Local\UnrealBuildTool\Log_UHT.txt`

# What Kind Of Log You Need to Find

Something like this should be inside of your log files for input.

```
Step - Read Manifest File
Step - Prepare Modules
Step - Prepare Headers
Step - Parse Headers
Step - Populate symbol table
Step - Resolve invalid check
Step - Bind super and bases
Step - Check for recursive structs
Step - Resolve bases
Step - Resolve properties
D:\Project\Source\<ProjectName>\Public\Characters\Components\UCustomComponent.h(77): Trace: Native pointer usage in member declaration detected [[[UParticleSystem*]]].  Consider TObjectPtr as an alternative.
D:\Project\Source\<ProjectName>\Public\Characters\Components\UCustomComponent.h(80): Trace: Native pointer usage in member declaration detected [[[UParticleSystemComponent*]]].  Consider TObjectPtr as an alternative.
D:\Project\Source\<ProjectName>\Public\Characters\Components\UCustomComponent.h(110): Trace: Native pointer usage in member declaration detected [[[AMWeapon*]]].  Consider TObjectPtr as an alternative.
D:\Project\Source\<ProjectName>\Public\Characters\Components\UCustomComponent.h(113): Trace: Native pointer usage in member declaration detected [[[UMWeaponCore*]]].  Consider TObjectPtr as an alternative.
```
