using GameBrowser.Library.Utils;
using MediaBrowser.Common.IO;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    /// <summary>
    /// Class GameResolver
    /// </summary>
    public class GameResolver : ItemResolver<Game>
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public GameResolver(ILogger logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Run before any core resolvers
        /// </summary>
        public override ResolverPriority Priority
        {
            get
            {
                return ResolverPriority.First;
            }
        }

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>Game.</returns>
        protected override Game Resolve(ItemResolveArgs args)
        {
            var collectionType = args.GetCollectionType();

            if (!string.Equals(collectionType, CollectionType.Games, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            var platform = ResolverHelper.AttemptGetGamePlatformTypeFromPath(_fileSystem, args.Path);

            if (string.IsNullOrEmpty(platform)) return null;

            if (args.IsDirectory)
            {
                return GetGame(args, platform);
            }

            // For MAME we will allow all games in the same dir
            if (string.Equals(platform, "Arcade"))
            {
                var extension = Path.GetExtension(args.Path);

                if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    // ignore zips that are bios roms.
                    if (MameUtils.IsBiosRom(args.Path)) return null;

                    var game = new Game
                    {
                        Name = MameUtils.GetFullNameFromPath(args.Path, _logger),
                        Path = args.Path,
                        GameSystem = "Arcade",
                        DisplayMediaType = "Arcade",
                        IsInMixedFolder = true
                    };
                    return game;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified path is game.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="consoleType">The type of gamesystem this game belongs too</param>
        /// <returns>A Game</returns>
        private Game GetGame(ItemResolveArgs args, string consoleType)
        {
            var validExtensions = GetExtensions(consoleType);

            var gameFiles = args.FileSystemChildren.Where(f =>
            {
                var fileExtension = Path.GetExtension(f.FullName);

                return validExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);

            }).ToList();

            if (gameFiles.Count == 0)
            {
                _logger.Error("gameFiles is 0 for " + args.Path);
                return null;
            }

            var game = new Game
            {
                Path = gameFiles[0].FullName,
                GameSystem = ResolverHelper.GetGameSystemFromPlatform(consoleType)
            };

            game.IsPlaceHolder =
                string.Equals(game.GameSystem, "windows", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(game.GameSystem, "dos", StringComparison.OrdinalIgnoreCase);

            game.DisplayMediaType = ResolverHelper.GetDisplayMediaTypeFromPlatform(consoleType);

            if (gameFiles.Count > 1)
            {
                game.MultiPartGameFiles = gameFiles.Select(i => i.FullName).ToList();
                game.IsMultiPart = true;
            }

            return game;
        }


        private IEnumerable<string> GetExtensions(string consoleType)
        {
            switch (consoleType)
            {
                case "Arcade":
                    return new[] { ".lnk", ".zip", ".rar", ".7z" };

                case "Amstrad GX4000":
                    return new[] { ".crt" };

                case "Atari 2600":
                    return new[] { ".bin", ".a26" };

                case "Atari 5200":
                    return new[] { ".a52" };

                case "Atari 7800":
                    return new[] { ".a78" };

                case "Atari Jaguar":
                    return new[] { ".jag", ".j64" };

                case "Atari Jaguar CD":
                    return new[] { ".cdi" };

                case "Atari XE":
                    return new[] { ".rom" };

                case "Bally Astrocade":
                    return new[] { ".bin" };

                case "ColecoVision":
                    return new[] { ".rom", ".col" };

                case "Commodore Amiga CD32":
                    return new[] { ".cue" };

                case "Entex Adventure Vision":
                    return new[] { ".bin" };

                case "Fairchild Channel F":
                    return new[] { ".bin" };

                case "GCE Vectrex":
                    return new[] { ".bin" };

                case "Magnavox Odyssey":
                    return new[] { ".bin" };

                case "Magnavox Odyssey 2":
                    return new[] { ".bin" };

                case "Mattel Intellivision":
                    return new[] { ".int" };

                case "Microsoft Xbox":
                    return new[] { ".null" }; //Falta extensão

                case "Microsoft Xbox 360":
                    return new[] { ".null" }; //Falta extensão

                case "Microsoft Xbox One":
                    return new[] { ".null" }; //Falta extensão

                case "NEC PC-FX":
                    return new[] { ".cue" };

                case "NEC SuperGrafx":
                    return new[] { ".pce" };

                case "NEC TurboGrafx 16":
                    return new[] { ".pce" };

                case "NEC TurboGrafx CD":
                    return new[] { ".cue" };

                case "Nintendo 64":
                    return new[] { ".n64", ".v64", ".z64" };

                case "Nintendo Famicom Disk System":
                    return new[] { ".fds" };

                case "Nintendo GameCube":
                    return new[] { ".iso", ".gcm" };

                case "Nintendo NES":
                    return new[] { ".nes" };

                case "Nintendo SNES":
                    return new[] { ".smc", ".sfc" };

                case "Nintendo Switch":
                    return new[] { ".null" }; // Falta extensão

                case "Nintendo Virtual Boy":
                    return new[] { ".vb" };

                case "Nintendo Wii":
                    return new[] { ".iso" };

                case "Nintendo Wii U":
                    return new[] { ".null" };   // Falta extensão

                case "Panasonic 3DO":
                    return new[] { ".cue" };

                case "Philips CD-i":
                    return new[] { ".chd" };

                case "Sega 32X":
                    return new[] { ".bin", ".32x" };

                case "Sega CD":
                    return new[] { ".cue" };

                case "Sega CD 32X":
                    return new[] { ".cue" };

                case "Sega Dreamcast":
                    return new[] { ".cdi", ".gdi" };

                case "Sega Master System":
                    return new[] { ".sms", ".bin" };

                case "Sega Mega Drive":
                    return new[] { ".smd", ".bin" };

                case "Sega Saturn":
                    return new[] { ".cue", ".mds" };

                case "Sega SG-1000 & SG-1000II":
                    return new[] { ".sg" };

                case "SNK Neo-Geo AES":
                    return new[] { ".zip", ".rar", ".7z" };

                case "SNK Neo-Geo CD":
                    return new[] { ".cue" };

                case "Sony Playstation":
                    return new[] { ".cue", ".ccd" };

                case "Sony Playstation 2":
                    return new[] { ".cue", ".iso" };

                case "Sony Playstation 3":
                    return new[] { ".null" }; // Falta Extensão

                case "Sony Playstation 4":
                    return new[] { ".null" }; // Falta Extensão

                case "WoW Action Max":
                    return new[] { ".lnk" };

                case "Atari Lynx":
                    return new[] { ".lnx" };

                case "Bandai Wonderswan":
                    return new[] { ".ws" };

                case "Bandai Wonderswan Color":
                    return new[] { ".wsc" };

                case "Nintendo 3DS":
                    return new[] { ".null" }; // Falta Extensão

                case "Nintendo DS":
                    return new[] { ".nds" };

                case "Nintendo Game Boy":
                    return new[] { ".gb" };

                case "Nintendo Game Boy Advance":
                    return new[] { ".gba" };

                case "Nintendo Game Boy Color":
                    return new[] { ".gbc" };

                case "Sega Game Gear":
                    return new[] { ".gg" };

                case "SNK Neo-Geo Pocket":
                    return new[] { ".ngp" };

                case "SNK Neo-Geo Pocket Color":
                    return new[] { ".ngp", ".ngc", ".npc" };

                case "Sony PSP":
                    return new[] { ".iso", ".cso" };

                case "Sony PSVita":
                    return new[] { ".null" }; // Falta Extensão

                case "Apple iOS":
                    return new[] { ".null" }; // Falta Extensão

                case "Google Android":
                    return new[] { ".null" }; // Falta Extensão

                case "Microsoft Windows 10 UWP":
                    return new[] { ".null" }; // Falta Extensão

                case "Commodore Vic-20":
                    return new[] { ".prg" };

                case "Fujitsu FM Towns":
                    return new[] { ".null" }; // Falta Extensão

                case "Fujitsu FM-7":
                    return new[] { ".cue" }; // Falta Extensão

                case "Microsoft MS-DOS":
                    return new[] { ".cue" }; // Falta Extensão

                case "Microsoft MSX":
                    return new[] { ".rom", ".mx1", ".col", ".dsk" };

                case "Microsoft MSX-2":
                    return new[] { ".rom", ".mx2", ".col", ".dsk" };

                case "Microsoft Windows":
                    return new[] { ".lnk" }; // Falta Extensão

                case "NEC PC-60":
                    return new[] { ".null" }; // Falta Extensão

                case "NEC PC-80":
                    return new[] { ".null" }; // Falta Extensão

                case "NEC PC-88":
                    return new[] { ".null" }; // Falta Extensão

                case "NEC-PC-98":
                    return new[] { ".fdi", ".hdi" };

                case "Sega SC-3000":
                    return new[] { ".sc", "sms" };

                case "Sharp X1":
                    return new[] { ".null" }; // Falta Extensão

                case "Sharp X68000":
                    return new[] { ".null" }; // Falta Extensão

                case "Sinclair ZX Spectrum":
                    return new[] { ".z80", ".tap", ".tzx", ",sna", ".dsk", ".rom", ".slt", ".zxs" };

                default:
                    return new string[] { };
            }

        }
    }
}
