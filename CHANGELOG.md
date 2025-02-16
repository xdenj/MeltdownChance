## 2.8.2
- minor update to ensure PTEM related methods only are patched when PTEM is installed

## 2.8.1
- hotfix for log spam

## 2.8.0
- compatibility with [PizzaTowerEscapeMusic](https://thunderstore.io/c/lethal-company/p/BGN/PizzaTowerEscapeMusic/)  (no music playback when no meltdown is happening)
- compatibility with 2.7 of FacilityMeltdown

## 2.7.0
- compatibility with 2.6 of FacilityMeltdown

## 2.6.0
- compatibility with v50 of the game and 2.5.0 of FacilityMeltdown

## 2.5.2, 2.5.3
- updated dependency to FacilityMeltdown 2.4.8\
This version is not compatible with the experimental version of FacilityMeltdown for v50 on the game's beta branch\
Once v50 is out on the stable branch, we will update this mod

## 2.5.1
- fixed description in the mod's config

## 2.5.0
- introduced networking to determine if meltdown has occured on client players
- re-added message on picking up apparatice
- added new config option to configure whether or not to display a message when picking up the apparatice
- if Meltdown does not occur, only skipping relevant methods if player is host
- added check to ensure no action is taken if current level is the Company Building

## 2.4.0
- Major rewrite of the mod. No longer unpatches and re-patches FacilityMeltdown using Harmony
- disable message on picking up the apparatice for now

## 2.3.0
- display message on pickup for all players

## 2.2.4
- only display pickup message on host side

## 2.2.0 - 2.2.3
- added message on taking the Apparatice

## 2.1.0
- changed behavior to not unload entire FacilityMeltdown mod, but just a specific method instead
- added: on ship landing, check to make sure FacilityMeltdown is running. If not running, re-patch 
- moved meltdown sequence event probability calculation from base to OnShipLanded routine to run at beginning of each round

## 2.0.0

- Now with 100% more functionality! (aka, it actually does *something* now)!
- using bepinex's built-in config functionality

## 1.0.0
- initial release