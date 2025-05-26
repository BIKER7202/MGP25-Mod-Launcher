# Information

A very simplistic program to allow the patching of MotoGP 25 in order to use mods, with the ability to launch either the patched version (to use mods) or the default version without mods (to play online).

The code isn't *brilliant*, but it does the job for this task. Obviously if you spot any bugs within the code itself, let me know and I'll address them. I may also update it in the future to make it less rigid to allow it to work with multiple games (I'm assuming 26 will need a similar tool).

Feel free to build upon this, adapt it for other games, port it to a different language, anything really!

## Usage

For most, just download the files from the releases section and follow the tutorial below:
<Link to Youtube>

Of course if you run into any issues, let me know either here or on in my Discord Server (more likely to get a fast response there).
Discord: https://discord.gg/WYMDRm2Hvv


## Other Info

Language used - C# with Windows Forms

Why did I choose that? - I know C# and honestly it seemed unnecessary to use anything lower level than that for a project so simple, especially for the UI side, I did not want to spend hours fighting the Win32 api.

Why did I use *mostly* static classes? - It seemed right as the properties were not being modified by the other classes at runtime, asside from Settings (which is why it was not static).

What's the naming convension?

It's based on Hungarian Notation, except it includes scope first before the data type. It is what I use at work so it's my go to now.

This is what each thing represents:

l - local (scope)

b - boolean

c - character/string

i - integer

o - object
