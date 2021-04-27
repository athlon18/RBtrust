# [RBTrust][0]

[![Download][1]][2]
[![Discord][3]][4]

🌎 **English**, [中文][101]

**RBTrust** is a Trust plugin + OrderBot scripts for [RebornBuddy][5]. It automatically runs Trust dungeons.

## Features

- Automatically completes Trust dungeons:

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

## Installation

### Prerequisites

- [RebornBuddy][5] with active license (paid)
- (Optional) Better combat routine, such as [Magitek][6] (free)
- (Optional) Self-repair plugin, such as [AutoRepair][7] (free)

### Setup

0. Fully delete old versions of RBTrust in the `RebornBuddy\Plugins\` folder.
1. Download the [latest version][2].
2. On the `.zip` file, right click > `Properties` > `Unblock` > `Apply`.
3. Unzip all contents into `RebornBuddy\Plugins\` so it looks like this:

```
RebornBuddy
└── Plugins
    └── RBTrust
        ├── Plugins\
        ├── Profiles\
        ├── Quest Behaviors\
        ├── RBTrust.sln
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
2. Click `Load Profile` and navigate to `RebornBuddy\Plugins\RBTrust\Profiles`.
3. Select the `.xml` script for the desired dungeon.
4. Back in RebornBuddy, click `Start`.

For live volunteer support, join the [RBTrust Discord][4]. When asking for help, tell us:

- what you tried to do
- what went wrong
- attach relevant logs from the `RebornBuddy\Logs` folder

No need to ask if anyone's around or for permission to ask -- just go for it!

## Troubleshooting

### How can I stop dying to a certain boss?

Maybe you can, maybe you can't.

RBTrust has limited combat abilities, so some classes struggle with certain bosses. Some things to try:

- Upgrade your gear and food to better survive big hits.
- Adjust your combat routine to better use damage mitigation, heals, life steal, etc.
- Change class (not a real solution)

Worst case scenario: out-level and skip that dungeon by grinding the previous one, or kill the boss manually if needed for MSQ progression.

### When starting a script, why does it says the "Trust" plugin isn't installed?

The RBTrust folder might not have been fully extracted or put in the correct place.

Check your Plugins tab to see if the "Trust" plugin is listed and enable if it is. If the plugin isn't there, try closing RebornBuddy and cleanly [reinstalling](#Installation) RBTrust.

[0]: https://github.com/athlon18/RBtrust "RBTrust on GitHub"
[1]: https://img.shields.io/badge/-DOWNLOAD-success
[2]: https://github.com/athlon18/RBtrust/archive/refs/heads/master.zip "Download"
[3]: https://img.shields.io/badge/DISCORD-7389D8?logo=discord&logoColor=ffffff&labelColor=6A7EC2
[4]: https://discord.gg/XtAneKksv4 "Discord"
[5]: https://www.rebornbuddy.com/ "RebornBuddy"
[6]: https://discord.gg/rDsFbKr "Magitek Discord"
[7]: https://github.com/nt153133/AutoRepair "AutoRepair"
[100]: ./README.md "English"
[101]: ./README.zh.md "中文"
