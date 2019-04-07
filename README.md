# April Fools 2019
Some features have been excluded from the source that I have on my end due to various privacy protection reasons. Please note
that this program is merely intended as an April Fools joke and is not intended to be malicious or cause damage in any way.

### Installation
1. Copy the GitHub repository files onto a blank external USB device.
2. Insert the USB device into the computer you wish to prank.
3. Edit line 14 of `activation.vbs` script so that the value in between `TimeValue()` is your desired activation time.
4. Double-click the `program.vbs` script. This will copy all required files into the user's temporary directory and execute
the activation timer.
5. Eject the USB device if the success prompt appears. Despite the claims of Windows that the USB device is still in use, it's safe.
6. Wait until the program activates. The initial payload will last 60 seconds and the program will self-destruct afterwards.

### Cleanup Instructions
If some residue remains afterwards, the instructions to clean up are as follows:
1. Press the `Win`+`R` keys on the keyboard simultaneously and type `%appdata%` into the input area of the prompt that appears. Then,
press Enter on the keyboard or the OK button.
2. Windows Explorer should appear. In the top address bar, click onto `AppData` to go backward one folder.
3. Double-click `Local`, and delete a folder named `1April2019`.
4. Return to the `AppData` folder.
5. Navigate to `Roaming\Microsoft\Windows\Start Menu\Programs\Startup` and delete a file named `activation.vbs`.
6. Optionally, you may want to empty the recycle bin however this is not required.

# Licensing
You may use this code for your own purposes, however, in this situation, you are responsible for your own applications and I assume
no liability or accountability if your program causes damage or does not function as intended. This is free source code, so please
do not sell this program or its source code for financial gain.

### External content
This program contains the Seinfeld theme as part of its resources. I did not create this peice of music, so the credit for the creation
of the song goes to the respective composer.

# Developmental progress
Please note however that the program is in development stages and is not stable, I am aware that the program has some issues and I will
proactively try to resolve them.
