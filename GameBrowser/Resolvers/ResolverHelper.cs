using MediaBrowser.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    class ResolverHelper
    {
        public static int? GetTgdbId(string consoleType)
        {
            return TgdbId.ContainsKey(consoleType) ? TgdbId[consoleType] : 0;
        }

        public static Dictionary<string, int> TgdbId = new Dictionary<string, int>
        {
                                                        {"Arcade", 23},
                                                        {"Atari 2600", 22},
                                                        {"Atari 5200", 26},
                                                        {"Atari 7800", 27},
                                                        {"Atari Jaguar", 28},
                                                        {"Atari Jaguar CD", 29},
                                                        {"Atari XE", 30},
                                                        {"Bally Astrocade", 4968},
                                                        {"ColecoVision", 31},
                                                        {"Commodore Amiga CD32", 4947},
                                                        {"Fairchild Channel F", 4928},
                                                        {"GCE Vectrex", 4939},
                                                        {"Magnavox Odyssey", 4961},
                                                        {"Magnavox Odyssey 2", 4927},
                                                        {"Mattel Intellivision", 32},
                                                        {"Microsoft Xbox", 14},
                                                        {"Microsoft Xbox 360", 15},
                                                        {"Microsoft Xbox One", 4920},
                                                        {"NEC PC-FX", 4930},
                                                        {"NEC TurboGrafx 16", 34},
                                                        {"NEC TurboGrafx CD", 4955},
                                                        {"Nintendo 64", 3},
                                                        {"Nintendo Famicom Disk System", 4936},
                                                        {"Nintendo GameCube", 2},
                                                        {"Nintendo NES", 7},
                                                        {"Nintendo SNES", 6},
                                                        {"Nintendo Virtual Boy", 4918},
                                                        {"Nintendo Wii", 9},
                                                        {"Nintendo Wii U", 38},
                                                        {"Panasonic 3DO", 25},
                                                        {"Philips CD-i", 4917},
                                                        {"Sega 32X", 33},
                                                        {"Sega CD", 21},
                                                        {"Sega Dreamcast", 16},
                                                        {"Sega Master System", 35},
                                                        {"Sega Mega Drive", 36},
                                                        {"Sega Saturn", 17},
                                                        {"Sega SG-1000 & SG-1000II", 4949},
                                                        {"SNK Neo-Geo AES", 24},
                                                        {"SNK Neo-Geo CD", 4956},
                                                        {"Sony Playstation", 10},
                                                        {"Sony Playstation 2", 11},
                                                        {"Sony Playstation 3", 12},
                                                        {"Sony Playstation 4", 4919},
                                                        {"Atari Lynx", 4924},
                                                        {"Bandai Wonderswan", 4925},
                                                        {"Bandai Wonderswan Color", 4926},
                                                        {"Nintendo 3DS", 4912},
                                                        {"Nintendo DS", 8},
                                                        {"Nintendo Game Boy", 4},
                                                        {"Nintendo Game Boy Advance", 5},
                                                        {"Nintendo Game Boy Color", 41},
                                                        {"Sega Game Gear", 20},
                                                        {"SNK Neo-Geo Pocket", 4922},
                                                        {"SNK Neo-Geo Pocket Color", 4923},
                                                        {"Sony PSP", 13},
                                                        {"Sony PSVita", 39},
                                                        {"Apple iOS", 4915},
                                                        {"Google Android", 4916},
                                                        {"Commodore Vic-20", 4945},
                                                        {"Microsoft MSX", 4929},
                                                        {"Microsoft Windows", 1},
                                                        {"NEC PC-88", 4933},
                                                        {"NEC-PC-98", 4934},
                                                        {"Sharp X68000", 4931},
                                                        {"Sinclair ZX Spectrum", 4913}
                                                    };

        public static string AttemptGetGamePlatformTypeFromPath(IFileSystem fileSystem, string path)
        {
            var system = Plugin.Instance.Configuration.GameSystems.FirstOrDefault(s => fileSystem.ContainsSubPath(s.Path, path) || string.Equals(s.Path, path, StringComparison.OrdinalIgnoreCase));

            return system != null ? system.ConsoleType : null;
        }

        public static string GetGameSystemFromPath(IFileSystem fileSystem, string path)
        {
            var platform = AttemptGetGamePlatformTypeFromPath(fileSystem, path);

            if (string.IsNullOrEmpty(platform))
            {
                return null;
            }

            return GetGameSystemFromPlatform(platform);
        }

        public static string GetGameSystemFromPlatform(string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                throw new ArgumentNullException("platform");
            }

            switch (platform)
            {
                case "Arcade":
                    return "Arcade";

                case "Amstrad GX4000":
                    return "GX4000";

                case "Atari 2600":
                    return "Atari2600";

                case "Atari 5200":
                    return "Atari5200";

                case "Atari 7800":
                    return "Atari7800";

                case "Atari Jaguar":
                    return "AtariJaguar";

                case "Atari Jaguar CD":
                    return "AtariJaguarCD";

                case "Atari XE":
                    return "AtariXE";

                case "Bally Astrocade":
                    return "Astrocade";

                case "ColecoVision":
                    return "ColecoVision";

                case "Commodore Amiga CD32":
                    return "AmigaCD32";

                case "Entex Adventure Vision":
                    return "AdventureVision";

                case "Fairchild Channel F":
                    return "ChannelF";

                case "GCE Vectrex":
                    return "Vectrex";

                case "Magnavox Odyssey":
                    return "Odyssey";

                case "Magnavox Odyssey 2":
                    return "Odyssey2";

                case "Mattel Intellivision":
                    return "Intellivision";

                case "Microsoft Xbox":
                    return "Xbox";

                case "Microsoft Xbox 360":
                    return "Xbox360";

                case "Microsoft Xbox One":
                    return "XboxOne";

                case "NEC PC-FX":
                    return "PCFX";

                case "NEC SuperGrafx":
                    return "SuperGrafx";

                case "NEC TurboGrafx 16":
                    return "TurboGrafx16";

                case "NEC TurboGrafx CD":
                    return "TurboGrafxCD";

                case "Nintendo 64":
                    return "Nintendo64";

                case "Nintendo Famicom Disk System":
                    return "FamicomDiskSystem";

                case "Nintendo GameCube":
                    return "GameCube";

                case "Nintendo NES":
                    return "NES";

                case "Nintendo SNES":
                    return "SNES";

                case "Nintendo Switch":
                    return "Switch";

                case "Nintendo Virtual Boy":
                    return "VirtualBoy";

                case "Nintendo Wii":
                    return "Wii";

                case "Nintendo Wii U":
                    return "WiiU";

                case "Panasonic 3DO":
                    return "3DO";

                case "Philips CD-i":
                    return "CD-i";

                case "Sega 32X":
                    return "Sega32X";

                case "Sega CD":
                    return "SegaCD";

                case "Sega CD 32X":
                    return "SegaCD32X";

                case "Sega Dreamcast":
                    return "Dreamcast";

                case "Sega Master System":
                    return "MasterSystem";

                case "Sega Mega Drive":
                    return "MegaDrive";

                case "Sega Saturn":
                    return "Saturn";

                case "Sega SG-1000 & SG-1000II":
                    return "SG1000&SG1000II";

                case "SNK Neo-Geo AES":
                    return "NeoGeoAES";

                case "SNK Neo-Geo CD":
                    return "NeoGeoCD";

                case "Sony Playstation":
                    return "Playstation";

                case "Sony Playstation 2":
                    return "Playstation2";

                case "Sony Playstation 3":
                    return "Playstation3";

                case "Sony Playstation 4":
                    return "Playstation4";

                case "WoW Action Max":
                    return "ActionMax";

                case "Atari Lynx":
                    return "Lynx";

                case "Bandai Wonderswan":
                    return "Wonderswan";

                case "Bandai Wonderswan Color":
                    return "WonderswanColor";

                case "Nintendo 3DS":
                    return "3DS";

                case "Nintendo DS":
                    return "DS";

                case "Nintendo Game Boy":
                    return "GameBoy";

                case "Nintendo Game Boy Advance":
                    return "GameBoyAdvance";

                case "Nintendo Game Boy Color":
                    return "GameBoyColor";

                case "Sega Game Gear":
                    return "GameGear";

                case "SNK Neo-Geo Pocket":
                    return "NeoGeoPocket";

                case "SNK Neo-Geo Pocket Color":
                    return "NeoGeoPocketColor";

                case "Sony PSP":
                    return "PSP";

                case "Sony PSVita":
                    return "PSVita";

                case "Apple iOS":
                    return "iOS";

                case "Google Android":
                    return "Android";

                case "Microsoft Windows 10 UWP":
                    return "Windows10UWP";

                case "Commodore Vic-20":
                    return "Vic20";

                case "Fujitsu FM Towns":
                    return "FMTowns";

                case "Fujitsu FM-7":
                    return "FM7";

                case "Microsoft MS-DOS":
                    return "DOS";

                case "Microsoft MSX":
                    return "MSX";

                case "Microsoft MSX-2":
                    return "MSX2";

                case "Microsoft Windows":
                    return "Windows";

                case "NEC PC-60":
                    return "PC60";

                case "NEC PC-80":
                    return "PC80";

                case "NEC PC-88":
                    return "PC88";

                case "NEC-PC-98":
                    return "PC98";

                case "Sega SC-3000":
                    return "SC3000";

                case "Sharp X1":
                    return "SharpX1";

                case "Sharp X68000":
                    return "SharpX68000";

                case "Sinclair ZX Spectrum":
                    return "ZXSpectrum";
            }
            return null;
        }

        public static string GetDisplayMediaTypeFromPlatform(string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                throw new ArgumentNullException("platform");
            }

            switch (platform)
            {
                case "Arcade":
                    return "ArcadeGame";

                case "Amstrad GX4000":
                    return "GX4000Game";

                case "Atari 2600":
                    return "Atari2600Game";

                case "Atari 5200":
                    return "Atari5200Game";

                case "Atari 7800":
                    return "Atari7800Game";

                case "Atari Jaguar":
                    return "AtariJaguarGame";

                case "Atari Jaguar CD":
                    return "AtariJaguarCDGame";

                case "Atari XE":
                    return "AtariXEGame";

                case "Bally Astrocade":
                    return "AstrocadeGame";

                case "ColecoVision":
                    return "ColecoVisionGame";

                case "Commodore Amiga CD32":
                    return "AmigaCD32Game";

                case "Entex Adventure Vision":
                    return "AdventureVisionGame";

                case "Fairchild Channel F":
                    return "ChannelFGame";

                case "GCE Vectrex":
                    return "VectrexGame";

                case "Magnavox Odyssey":
                    return "OdysseyGame";

                case "Magnavox Odyssey 2":
                    return "Odyssey2Game";

                case "Mattel Intellivision":
                    return "IntellivisionGame";

                case "Microsoft Xbox":
                    return "XboxGame";

                case "Microsoft Xbox 360":
                    return "Xbox360Game";

                case "Microsoft Xbox One":
                    return "XboxOneGame";

                case "NEC PC-FX":
                    return "PCFXGame";

                case "NEC SuperGrafx":
                    return "SuperGrafxGame";

                case "NEC TurboGrafx 16":
                    return "TurboGrafx16Game";

                case "NEC TurboGrafx CD":
                    return "TurboGrafxCDGame";

                case "Nintendo 64":
                    return "Nintendo64Game";

                case "Nintendo Famicom Disk System":
                    return "FamicomDiskSystemGame";

                case "Nintendo GameCube":
                    return "GameCubeGame";

                case "Nintendo NES":
                    return "NESGame";

                case "Nintendo SNES":
                    return "SNESGame";

                case "Nintendo Switch":
                    return "SwitchGame";

                case "Nintendo Virtual Boy":
                    return "VirtualBoyGame";

                case "Nintendo Wii":
                    return "WiiGame";

                case "Nintendo Wii U":
                    return "WiiUGame";

                case "Panasonic 3DO":
                    return "3DOGame";

                case "Philips CD-i":
                    return "CD-iGame";

                case "Sega 32X":
                    return "Sega32XGame";

                case "Sega CD":
                    return "SegaCDGame";

                case "Sega CD 32X":
                    return "SegaCD32XGame";

                case "Sega Dreamcast":
                    return "DreamcastGame";

                case "Sega Master System":
                    return "MasterSystemGame";

                case "Sega Mega Drive":
                    return "MegaDriveGame";

                case "Sega Saturn":
                    return "SaturnGame";

                case "Sega SG-1000 & SG-1000II":
                    return "SG1000&SG1000IIGame";

                case "SNK Neo-Geo AES":
                    return "NeoGeoAESGame";

                case "SNK Neo-Geo CD":
                    return "NeoGeoCDGame";

                case "Sony Playstation":
                    return "PlaystationGame";

                case "Sony Playstation 2":
                    return "Playstation2Game";

                case "Sony Playstation 3":
                    return "Playstation3Game";

                case "Sony Playstation 4":
                    return "Playstation4Game";

                case "WoW Action Max":
                    return "ActionMaxGame";

                case "Atari Lynx":
                    return "LynxGame";

                case "Bandai Wonderswan":
                    return "WonderswanGame";

                case "Bandai Wonderswan Color":
                    return "WonderswanColorGame";

                case "Nintendo 3DS":
                    return "3DSGame";

                case "Nintendo DS":
                    return "DSGame";

                case "Nintendo Game Boy":
                    return "GameBoyGame";

                case "Nintendo Game Boy Advance":
                    return "GameBoyAdvanceGame";

                case "Nintendo Game Boy Color":
                    return "GameBoyColorGame";

                case "Sega Game Gear":
                    return "GameGearGame";

                case "SNK Neo-Geo Pocket":
                    return "NeoGeoPocketGame";

                case "SNK Neo-Geo Pocket Color":
                    return "NeoGeoPocketColorGame";

                case "Sony PSP":
                    return "PSPGame";

                case "Sony PSVita":
                    return "PSVitaGame";

                case "Apple iOS":
                    return "iOSGame";

                case "Google Android":
                    return "AndroidGame";

                case "Microsoft Windows 10 UWP":
                    return "Windows10UWPGame";

                case "Commodore Vic-20":
                    return "Vic20Game";

                case "Fujitsu FM Towns":
                    return "FMTownsGame";

                case "Fujitsu FM-7":
                    return "FM7Game";

                case "Microsoft MS-DOS":
                    return "DOSGame";

                case "Microsoft MSX":
                    return "MSXGame";

                case "Microsoft MSX-2":
                    return "MSX2Game";

                case "Microsoft Windows":
                    return "WindowsGame";

                case "NEC PC-60":
                    return "PC60Game";

                case "NEC PC-80":
                    return "PC80Game";

                case "NEC PC-88":
                    return "PC88Game";

                case "NEC-PC-98":
                    return "PC98Game";

                case "Sega SC-3000":
                    return "SC3000Game";

                case "Sharp X1":
                    return "SharpX1Game";

                case "Sharp X68000":
                    return "SharpX68000Game";

                case "Sinclair ZX Spectrum":
                    return "ZXSpectrumGame";
            }
            return null;
        }
    }
}
