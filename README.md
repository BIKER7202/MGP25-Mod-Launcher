# Information

A very simplistic program to allow the patching of MotoGP 25 in order to use mods, with the ability to launch either the patched version (to use mods) or the default version without mods (to play online).

The code isn't *brilliant*, but it does the job for this task. Obviously if you spot any bugs within the code itself, let me know and I'll address them.

Feel free to build upon this, adapt it for other games, port it to a different language, anything really!

## Usage

For most, just download the files from the releases section and follow the tutorial:

[![Video Tutorial](https://img.youtube.com/vi/2G6jNwbMmfo/maxresdefault.jpg)](https://youtu.be/2G6jNwbMmfo)

Of course if you run into any issues, let me know either here or on in my Discord Server (more likely to get a fast response there).

Discord: https://discord.gg/WYMDRm2Hvv


## Credits

**This program now uses the following tools to do the patching:**

[**- Asi Loader by ThirteenAG**](https://github.com/ThirteenAG/Ultimate-ASI-Loader)

[**- Universal Sig Bypasser by NoobInCoding**](https://github.com/rm-NoobInCoding/UniversalSigBypasser)


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
