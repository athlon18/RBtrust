# [RBtrust][0]

[![Download][1]][2]
[![Discord][3]][4]

🌎 **English** • [中文][-2]

**RBtrust** is a Trust plugin + OrderBot scripts for [RebornBuddy][5]. It automatically runs Trust dungeons.


[-2]: ./README.zh.md "中文"
[-1]: ./README.md "English"
[0]: https://github.com/athlon18/RBtrust "RBtrust on GitHub"
[1]: https://img.shields.io/badge/-Download-brightgreen
[2]: https://github.com/athlon18/RBtrust/archive/refs/heads/master.zip "Download"
[3]: https://img.shields.io/badge/Discord-7389D8?logo=discord&logoColor=ffffff&labelColor=6A7EC2
[4]: https://discord.gg/XtAneKksv4 "Discord"
[5]: https://www.rebornbuddy.com/ "RebornBuddy"

## Supported Dungeons

### 5.0 Shadowbringers

  ✔️ Lv. 71: Holminster Switch\
  ✔️ Lv. 73: Dohn Mheg\
  ✔️ Lv. 75: The Qitana Ravel\
  ✔️ Lv. 77: Malikah's Well\
  ✔️ Lv. 79: Mt. Gulg\
  ⚠️ Lv. 80: Amaurot\
  ❌ Lv. 80: The Grand Cosmos\
  ❌ Lv. 80: Anamnesis Anyder\
  ❌ Lv. 80: The Heroes' Gauntlet\
  ❌ Lv. 80: Matoya's Relict\
  ❌ Lv. 80: Paglth'an

### 6.0 Endwalker

  ❌ Lv. 81: The Tower of Zot\
  ❌ Lv. 83: The Tower of Babil\
  ❌ Lv. 85: Vanaspati\
  ❌ Lv. 87: Ktisis Hyperboreia\
  ❌ Lv. 89: The Aitiascope\
  ❌ Lv. 89: The Mothercrystal\
  ❌ Lv. 90: The Dead Ends

## Installation

### Prerequisites

- [RebornBuddy][5] with active license (paid)
- (Optional) Better combat routine, such as [Magitek][100] (free)
- (Optional) Self-repair plugin, such as [AutoRepairLisbeth][101]
- (Optional) Better food plugin, such as [Gluttony][102] (free)

[100]: https://discord.gg/rDsFbKr "Magitek Discord"
[101]: https://github.com/nt153133/LlamaPlugins "AutoRepairLisbeth"
[102]: https://github.com/domesticwarlord86/Gluttony "Gluttony"

### Automatic Setup (recommended)

Want **automatic installation and updates**, including prerequisites?

Install the [RepoBuddy][103] plugin -- `RBtrust` is configured by default!

[103]: https://github.com/Zimgineering/repoBuddy "repoBuddy"

#### Adding `RBtrust` to RepoBuddy

ℹ️ New users can skip this step.

In case your repoBuddy config is too old or otherwise missing `RBtrust`, you can add it via repoBuddy's settings menu:

- **Name:** RBtrust
- **Type:** Plugin
- **URL:** `https://github.com/athlon18/RBtrust.git/trunk`

![repBuddy Settings](https://i.imgur.com/q53jzEX.png)

OR by first closing the bot, then editing `RebornBuddy\Plugins\repoBuddy\repoBuddyRepos.xml`:

```xml
<Repo>
  <Name>RBtrust</Name>
  <Type>Plugin</Type>
  <URL>https://github.com/athlon18/RBtrust.git/trunk</URL>
</Repo>
```

### Setup

0. Fully delete old versions of RBtrust in the `RebornBuddy\Plugins\` folder.
1. Download the [latest version][2].
2. On the `.zip` file, right click > `Properties` > `Unblock` > `Apply`.
3. Unzip all contents into `RebornBuddy\Plugins\` so it looks like this:

```
RebornBuddy
└── Plugins
    └── RBtrust
        ├── Plugins\
        ├── Profiles\
        ├── Quest Behaviors\
        ├── RBtrust.sln
        ├── README.md
        └── ...
```

4. Start RebornBuddy as normal.
5. In the Plugins tab, enable the `Trust` plugin.

## Usage

⚠️ Some classes may not survive certain bosses. ⚠️ If you can't clear even after tuning combat routine settings, try running the previous dungeon until you out-level and can skip the "difficult" one.

Each dungeon is handled by a separate OrderBot script that repeats the dungeon infinitely. Graduating to the next dungeon must be done manually by changing scripts.

To load a dungeon script:

1. Start RebornBuddy and set the BotBase dropdown to `Order Bot`.
2. Click `Load Profile` and navigate to `RebornBuddy\Plugins\RBtrust\Profiles`.
3. Select the `.xml` script for the desired dungeon.
4. Back in RebornBuddy, click `Start`.

## Troubleshooting

For live volunteer support, join the [RBtrust Discord][4].

When asking for help, always include:

- which `.xml` script you loaded,
- what you tried to do,
- what went wrong,
- relevant logs from the `RebornBuddy\Logs\` folder.

No need to ask if anyone's around or for permission to ask -- just go for it!

### How can I stop dying to a certain boss?

Maybe you can, maybe you can't.

RBtrust has limited combat abilities, so some classes struggle with certain bosses. Some things to try:

- Upgrade your gear and food to better survive big hits.
- Adjust your combat routine to better use damage mitigation, heals, life steal, etc.
- Change class (not a real solution)

Worst case scenario: out-level and skip that dungeon by grinding the previous one, or kill the boss manually if needed for MSQ progression.

### When starting a script, why does it says the "Trust" plugin isn't installed?

The RBtrust folder might not have been fully extracted or put in the correct place.

Check your Plugins tab to see if the "Trust" plugin is listed and enable if it is. If the plugin isn't there, try closing RebornBuddy and cleanly [reinstalling](#installation) RBtrust.
