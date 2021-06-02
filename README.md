# MicMuter
A Beat Saber mod that automatically mutes a selected microphone while playing a song.
Optionally it displays a mute button that is always visible except during play (check configuration).

Compatible with Beat Saber 1.16.1

## Installation
* Required dependency (can be installed using Mod Assistant)
    * BeatSaberMarkupLanguage v1.5.3+
    * BS_Utils v1.10.0+
* Mod installation
    * Download the latest DLL from the [Releases](https://github.com/TechDiem/BeatSaberMicMuter/releases/latest) page and copy it into your Plugins folder.
    * You should see a MicMuter entry in the Mod Settings menu when installed.

## Configuration
Please restart the game once after the first start with this mod installed, else the config may not be applied.
You can configure the plugin using the  `Settings (⚙) -> Mod Settings -> MicMuter` menu.
* Enabled in singleplayer/party/campaign: Enable automatic muting in every gamemode except multiplayer (singleplayer, party, campaign)
* Enabled in multiplayer: Enable automatic muting in multiplayer (online)
* Unmute on pause: Unmute the microphone while in the pause menu (only singleplayer)
* Microphone: Select your microphone here. Make sure that every name only exists once.
### Submenu: Floating mute button
* Enable floating mute button: Show a little mute button everywhere instead of ingame
* Show movement handle: Show a little white handle to the left of the button, grab it and change the position


## Credits 
* [CSCore](https://github.com/filoe/cscore): Library used to access the CoreAudioApi to control the microphone