# MicMuter
A Beat Saber mod that automatically mutes a selected microphone while playing a song.

Compatible with Beat Saber 1.14.0

## Installation
* Required dependency (can be installed using Mod Assistant)
    * BeatSaberMarkupLanguage v1.5.2+
* Mod installation
    * Download the latest ZIP from the [Releases](https://github.com/TechDiem/BeatSaberMicMuter/releases) page and extract it to your Beat Saber folder. After that, you should find a `MicMuter.dll` in your `Beat Saber\Plugins` folder and a `CSCore.dll` in your `Beat Saber\Libs` folder.
    * You should see a MicMuter entry in the Mod Settings menu when installed.

## Configuration
You can configure the plugin using the  `Settings (⚙) -> Mod Settings -> MicMuter` menu.
* Enabled: Enable automatic mic muting/unmuting
* Unmute on pause: Select whether the microphone should be activated while in the pause menu
* Microphone: Select your microphone device.

## Disclaimer
This is my first Beat Saber mod and I have little experience in c#, so bugs may occur. I will try to improve the quality of the code in the future.

## Credits 
* [CSCore](https://github.com/filoe/cscore): Library used to access the CoreAudioApi to control the microphone