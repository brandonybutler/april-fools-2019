Dim message, sapi, objShell, profilePath
Dim userProfile, appDataDirectory

Set objShell = WScript.CreateObject("WScript.Shell")

' Get the path to the user's C: folder.
userProfile = objShell.ExpandEnvironmentStrings("%USERPROFILE%")
' Declare the location of the program's AppData folder.
appDataDirectory = userProfile + "\AppData\Local\1April2019"

' The script will run as a background process constantly searching for the time to activate.
While True
    ' Modify the parameter specified within the TimeValue() to change the activation time.
    If Time() = TimeValue("8:37am") Then
        ' Once the time is reached, say "I don't feel so good" and then execute the program.
        message="I don't feel so good."
        Set sapi=CreateObject("sapi.spvoice")
        sapi.Speak message

        objShell.Exec(appDataDirectory + "\1April2019\bin\Debug\1April2019.exe")
        Set objShell = Nothing
    End If
Wend
