# Beat Saber MicMuter
A Beat Saber mod for muting your micrphone in some ways:
* Automatic mute when ingame in singleplayer/multiplayer
* Push-to-talk
* Floating mute button that is always visible except during play.

Compatible with **Beat Saber 1.18.0**

## Installation
* Required dependencies (can be installed using Mod Assistant)
    * BeatSaberMarkupLanguage v1.5.10+
    * BS_Utils v1.11.0+
* Mod installation
    * Download the latest DLL from the [Releases](https://github.com/techdiem/BeatSaberMicMuter/releases/latest) page and copy it into your Plugins folder.
    * You should see a MicMuter entry in the Mod Settings menu when installed.

## Configuration
Please restart the game once after the first start with this mod installed, else the config may not be applied.
You can configure the plugin using the  `Settings (⚙) -> Mod Settings -> MicMuter` menu.

* Microphone: Select your microphone here. Make sure that every name only exists once.
### Submenu: Automatic mute
* Enabled in singleplayer/party/campaign: Enable automatic muting in every gamemode except multiplayer (singleplayer, party, campaign)
* Enabled in multiplayer: Enable automatic muting in multiplayer (online)
* Unmute on pause: Unmute the microphone while in the pause menu (only singleplayer)
### Submenu: Push-to-talk
* Enable Push-to-talk: Toggle microphone mute with a controller button
* Button: Select the button or button combo that should be used to unmute the mic
* Invert (Push-to-mute): Mute instead of unmute on button press
### Submenu: Floating mute button
* Enable floating mute button: Show a little mute button everywhere instead of ingame
* Show movement handle: Show a little white handle to the left of the button, grab it and change the position

You can also use all three features simultaneously.


## Credits 
* [CSCore](https://github.com/filoe/cscore): Library used to access the CoreAudioApi to control the microphone