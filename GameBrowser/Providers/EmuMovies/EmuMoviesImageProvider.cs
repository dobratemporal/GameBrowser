using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace GameBrowser.Providers.EmuMovies
{
    public class EmuMoviesImageProvider : IRemoteImageProvider, IHasOrder
    {
        private readonly IHttpClient _httpClient;

        public EmuMoviesImageProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(IHasImages item, CancellationToken cancellationToken)
        {
            var list = new List<RemoteImageInfo>();

            foreach (var image in GetSupportedImages(item))
            {
                var sublist = await GetImages(item, image, cancellationToken).ConfigureAwait(false);

                list.AddRange(sublist);
            }

            return list;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            // TODO: Call GetEmuMoviesToken and replace the sessionId in the incoming url with the latest value. 

            return _httpClient.GetResponse(new HttpRequestOptions
            {
                CancellationToken = cancellationToken,
                Url = url,
                ResourcePool = Plugin.Instance.EmuMoviesSemiphore
            });
        }

        public Task<IEnumerable<RemoteImageInfo>> GetImages(IHasImages item, ImageType imageType, CancellationToken cancellationToken)
        {
            var game = (Game)item;

            switch (imageType)
            {
                case ImageType.Box:
                    return FetchImages(game, EmuMoviesMediaTypes.Cabinet, imageType, cancellationToken);
                case ImageType.Screenshot:
                    return FetchImages(game, EmuMoviesMediaTypes.Snap, imageType, cancellationToken);
                case ImageType.Disc:
                    return FetchImages(game, EmuMoviesMediaTypes.Cart, imageType, cancellationToken);
                case ImageType.Menu:
                    return FetchImages(game, EmuMoviesMediaTypes.Title, imageType, cancellationToken);
                default:
                    throw new ArgumentException("Unrecognized image type");
            }
        }

        /// <summary>
        /// Fetches the images.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="type">The type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task{IEnumerable{RemoteImageInfo}}.</returns>
        private async Task<IEnumerable<RemoteImageInfo>> FetchImages(Game game, EmuMoviesMediaTypes mediaType, ImageType type, CancellationToken cancellationToken)
        {
            var sessionId = await Plugin.Instance.GetEmuMoviesToken(cancellationToken);

            var list = new List<RemoteImageInfo>();

            if (sessionId == null) return list;

            var url = string.Format(EmuMoviesUrls.Search, HttpUtility.UrlEncode(game.Name), GetEmuMoviesPlatformFromGameSystem(game.GameSystem), mediaType, sessionId);

            using (var stream = await _httpClient.Get(url, Plugin.Instance.EmuMoviesSemiphore, cancellationToken).ConfigureAwait(false))
            {
                var doc = new XmlDocument();
                doc.Load(stream);

                if (doc.HasChildNodes)
                {
                    var nodes = doc.SelectNodes("Results/Result");

                    if (nodes != null)
                    {
                        foreach (XmlNode node in nodes)
                        {
                            if (node != null && node.Attributes != null)
                            {
                                var urlAttribute = node.Attributes["URL"];

                                if (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value))
                                {
                                    list.Add(new RemoteImageInfo
                                    {
                                        ProviderName = Name,
                                        Type = type,
                                        Url = urlAttribute.Value
                                    });
                                }
                            }
                        }
                    }

                }
            }

            return list;
        }

        private string GetEmuMoviesPlatformFromGameSystem(string platform)
        {
            string emuMoviesPlatform = null;

            switch (platform)
            {
                case "Arcade":
                    emuMoviesPlatform = "MAME";

                    break;

                case "Amstrad GX4000":
                    emuMoviesPlatform = "GX4000";

                    break;

                case "Atari 2600":
                    emuMoviesPlatform = "Atari_2600";

                    break;

                case "Atari 5200":
                    emuMoviesPlatform = "Atari_5200";

                    break;

                case "Atari 7800":
                    emuMoviesPlatform = "Atari_7800";

                    break;

                case "Atari Jaguar":
                    emuMoviesPlatform = "Atari_Jaguar";

                    break;

                case "Atari Jaguar CD":
                    emuMoviesPlatform = "Atari_Jaguar_CD";

                    break;

                case "Atari XE":
                    emuMoviesPlatform = "Atari_8-Bit";

                    break;

                case "Bally Astrocade":
                    emuMoviesPlatform = "Bally_Astrocade";

                    break;

                case "ColecoVision":
                    emuMoviesPlatform = "Colecovision";

                    break;

                case "Commodore Amiga CD32":
                    emuMoviesPlatform = "Amiga_CD32";

                    break;

                case "Entex Adventure Vision":
                    emuMoviesPlatform = "Adventure_Vision";

                    break;

                case "Fairchild Channel F":
                    emuMoviesPlatform = "Channel_F";

                    break;

                case "GCE Vectrex":
                    emuMoviesPlatform = "Vectrex";

                    break;

                case "Magnavox Odyssey":
                    emuMoviesPlatform = "Magnavox_Odyssey";

                    break;

                case "Magnavox Odyssey 2":
                    emuMoviesPlatform = "Odyssey_2";

                    break;

                case "Mattel Intellivision":
                    emuMoviesPlatform = "Intellivision";

                    break;

                case "Microsoft Xbox":
                    emuMoviesPlatform = "Xbox";

                    break;

                case "Microsoft Xbox 360":
                    emuMoviesPlatform = "";

                    break;

                case "Microsoft Xbox One":
                    emuMoviesPlatform = "";

                    break;

                case "NEC PC-FX":
                    emuMoviesPlatform = "PC-FX";

                    break;

                case "NEC SuperGrafx":
                    emuMoviesPlatform = "SuperGrafx";

                    break;

                case "NEC TurboGrafx 16":
                    emuMoviesPlatform = "TurboGrafx-16";

                    break;

                case "NEC TurboGrafx CD":
                    emuMoviesPlatform = "TurboGrafx-CD";

                    break;

                case "Nintendo 64":
                    emuMoviesPlatform = "Nintendo_64";

                    break;

                case "Nintendo Famicom Disk System":
                    emuMoviesPlatform = "Famicom_Disk_System";

                    break;

                case "Nintendo GameCube":
                    emuMoviesPlatform = "GameCube";

                    break;

                case "Nintendo NES":
                    emuMoviesPlatform = "NES_Unified";

                    break;

                case "Nintendo SNES":
                    emuMoviesPlatform = "Super_Nintendo";

                    break;

                case "Nintendo Switch":
                    emuMoviesPlatform = "";

                    break;

                case "Nintendo Virtual Boy":
                    emuMoviesPlatform = "Virtual_Boy";

                    break;

                case "Nintendo Wii":
                    emuMoviesPlatform = "Nintendo_Wii";

                    break;

                case "Nintendo Wii U":
                    emuMoviesPlatform = "";

                    break;

                case "Panasonic 3DO":
                    emuMoviesPlatform = "3DO";

                    break;

                case "Philips CD-i":
                    emuMoviesPlatform = "Philips_CD-i";

                    break;

                case "Sega 32X":
                    emuMoviesPlatform = "";

                    break;

                case "Sega CD":
                    emuMoviesPlatform = "";

                    break;

                case "Sega CD 32X":
                    emuMoviesPlatform = "";

                    break;

                case "Sega Dreamcast":
                    emuMoviesPlatform = "Sega_Dreamcast";

                    break;

                case "Sega Master System":
                    emuMoviesPlatform = "Master_System";
                    break;

                case "Sega Mega Drive":
                    emuMoviesPlatform = "Genesis";
                    break;

                case "Sega Saturn":
                    emuMoviesPlatform = "Saturn";
                    break;

                case "Sega SG-1000 & SG-1000II":
                    emuMoviesPlatform = "Sega_SG-1000";
                    break;

                case "SNK Neo-Geo AES":
                    emuMoviesPlatform = "";
                    break;

                case "SNK Neo-Geo CD":
                    emuMoviesPlatform = "";
                    break;

                case "Sony Playstation":
                    emuMoviesPlatform = "Playstation";
                    break;

                case "Sony Playstation 2":
                    emuMoviesPlatform = "Playstation_2";
                    break;

                case "Sony Playstation 3":
                    emuMoviesPlatform = "";
                    break;

                case "Sony Playstation 4":
                    emuMoviesPlatform = "";
                    break;

                case "WoW Action Max":
                    emuMoviesPlatform = "Action_Max";
                    break;

                case "Atari Lynx":
                    emuMoviesPlatform = "Atari_Lynx";
                    break;

                case "Bandai Wonderswan":
                    emuMoviesPlatform = "Wonderswan";
                    break;

                case "Bandai Wonderswan Color":
                    emuMoviesPlatform = "Wonderswan_Color";
                    break;

                case "Nintendo 3DS":
                    emuMoviesPlatform = "";
                    break;

                case "Nintendo DS":
                    emuMoviesPlatform = "Nintendo_DS";
                    break;

                case "Nintendo Game Boy":
                    emuMoviesPlatform = "Game_Boy";
                    break;

                case "Nintendo Game Boy Advance":
                    emuMoviesPlatform = "GBA";
                    break;

                case "Nintendo Game Boy Color":
                    emuMoviesPlatform = "Game_Boy_Color";
                    break;

                case "Sega Game Gear":
                    emuMoviesPlatform = "Game_Gear";
                    break;

                case "SNK Neo-Geo Pocket":
                    emuMoviesPlatform = "Neo_Geo_Pocket";
                    break;

                case "SNK Neo-Geo Pocket Color":
                    emuMoviesPlatform = "Neo_Geo_Pocket_Color";
                    break;

                case "Sony PSP":
                    emuMoviesPlatform = "Sony_PSP";
                    break;

                case "Sony PSVita":
                    emuMoviesPlatform = "";
                    break;

                case "Apple iOS":
                    emuMoviesPlatform = "";
                    break;

                case "Google Android":
                    emuMoviesPlatform = "Android";
                    break;

                case "Microsoft Windows 10 UWP":
                    emuMoviesPlatform = "";
                    break;

                case "Commodore Vic-20":
                    emuMoviesPlatform = "Commodore_Vic-20";
                    break;

                case "Fujitsu FM Towns":
                    emuMoviesPlatform = "Fujitsu_FM_Towns";
                    break;

                case "Fujitsu FM-7":
                    emuMoviesPlatform = "";
                    break;

                case "Microsoft MS-DOS":
                    emuMoviesPlatform = "Microsoft_DOS";
                    break;

                case "Microsoft MSX":
                    emuMoviesPlatform = "Microsoft_MSX";
                    break;

                case "Microsoft MSX-2":
                    emuMoviesPlatform = "";
                    break;

                case "Microsoft Windows":
                    emuMoviesPlatform = "Microsoft_Windows";
                    break;

                case "NEC PC-60":
                    emuMoviesPlatform = "";
                    break;

                case "NEC PC-80":
                    emuMoviesPlatform = "";
                    break;

                case "NEC PC-88":
                    emuMoviesPlatform = "";
                    break;

                case "NEC-PC-98":
                    emuMoviesPlatform = "";
                    break;

                case "Sega SC-3000":
                    emuMoviesPlatform = "Sega_SC-3000";
                    break;

                case "Sharp X1":
                    emuMoviesPlatform = "Sharp_X1";
                    break;

                case "Sharp X68000":
                    emuMoviesPlatform = "Sharp_X68000";
                    break;

                case "Sinclair ZX Spectrum":
                    emuMoviesPlatform = "ZX_Spectrum";
                    break;
            }

            return emuMoviesPlatform;

        }

        public IEnumerable<ImageType> GetSupportedImages(IHasImages item)
        {
            return new[] { ImageType.Box, ImageType.Disc, ImageType.Screenshot, ImageType.Menu };
        }

        public string Name
        {
            get { return "Emu Movies"; }
        }

        public bool Supports(IHasImages item)
        {
            return item is Game;
        }

        public int Order
        {
            get
            {
                // Make sure it runs after games db since these images are lower resolution
                return 1;
            }
        }
    }
}
