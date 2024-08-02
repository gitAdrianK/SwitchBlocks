# SwitchBlocks - A Jump King Mod

This mod adds custom blocks to Jump King, each with its own unique *switching-its-state* themed mechanic.
- Texture of the levers and platforms
- Sounds played when a lever is touched
- When applicable the duration of timed gimmick

## Logic
The general logic is that a block is solid when it matches the current state, as such an OFF block will be solid/function when the state currently is OFF as well.

## Auto Blocks
Automatically switches state after a set amount of time.

## Basic Blocks
Is solid if the kind of platform is equal to the state of the lever.
Switches its state when touching a lever.

## Countdown Blocks
Switches its state when touching a lever, automatically switches back after a set amount of time.

## Jump Blocks
Switches its state when the player jumps.

## Sand Blocks
Behaves like the vanilla sand block sinking the player and slowing them down. When state has been switched, it instead pushes the player up.
Switches its state when touching a lever.

## Find the mod and an example map on steam here
[Mod](https://steamcommunity.com/sharedfiles/filedetails/?id=3188962826)
[Map](https://steamcommunity.com/sharedfiles/filedetails/?id=3175561853)

## I have an idea for a block, can I ask for it to be added?
Sure, make an issue and I'll have a look at it.

## Why are the switches called levers?
switch is a programming keyword, so i would have to name them something along the lines of \_switch or switch\_ or zwitch or whatever name you can think of, in the end I have to choose something ¯\_(ツ)_/¯
