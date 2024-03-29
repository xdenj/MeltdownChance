## 2.5.0
- if Meltdown does not occur, only skipping relevant methods if player is host
- introduced networking to determine if meltdown has occured on client players
- added message on picking up apparatice back in

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