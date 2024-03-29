> ## **⚠ WARNING ⚠**
> This mod changes the *intended behaviour* of the mod [FacilityMeltdown](https://thunderstore.io/c/lethal-company/p/loaforc/FacilityMeltdown/).  As such, it is neither endorsed, nor supported by the original mod's creator.\
> If you wish to have FacilityMeltdown's vanilla experience, without the chance aspect added, only install [FacilityMeltdown](https://thunderstore.io/c/lethal-company/p/loaforc/FacilityMeltdown/) and disregard this mod.


# Meltdown Chance

Mod that allows users of [FacilityMeltdown](https://thunderstore.io/c/lethal-company/p/loaforc/FacilityMeltdown/) to set a percentage change of a meltdown happening

- set value between 0 and 100 in the mod's config (chance is based on the host's configured chance) 


> ## **ℹ️ INFO**
>Versions prior to 2.4.0 of this mod used a method to suppress meltdowns (Harmony.Unpatch()), which might not have been the most graceful way of doing things. This introduced the need to re-patch FacilityMeltdown at the end of a round, all of which may have lead to errors and oddities.\
>If this caused any problems in your game, I apologize. This is my first ever game mod.

If you encounter issues or have suggestions, visit the [Discord Post](https://discord.com/channels/1168655651455639582/1210941635421151272).


## [Changelog](https://thunderstore.io/c/lethal-company/p/den/Meltdown_Chance/changelog/)

### 2.5.0
- if Meltdown does not occur, only skipping relevant methods if player is host
- introduced networking to determine if meltdown has occured on client players
- added message on picking up apparatice back in