using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ad.Client.ClientService;
using Domain.Core;
using Domain.Concrete;
using Repository.Core;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Threading;
using LibVLCSharp.Shared;
using Helpers;
using Repository.EntityFramework;

namespace Ad.Client
{
    class Program
    {
        private ClientServiceClient service;
        private object _lock = new object();
        private IRepository handler;
        private IClient client;
        private readonly string localSaveFile = "..\\KeyInfos.txt";
        private readonly string adSaveLocation = "..\\Ads\\";
        private List<Thread> playlistThreads = new List<Thread>();
        private LibVLC vlc;
        private TimeSpan sleepDuringPlay = new TimeSpan(0,0,10);
        private TimeSpan sleepDuringOffTime = new TimeSpan(0, 1, 0);
        private TimeSpan sleepBetweenUpdate = new TimeSpan(0, 15, 0);
        private MediaPlayer player;
        private List<int> previousPlayedAds = new List<int>();

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();

            Console.WriteLine();
            Console.WriteLine("Press Any Key to Close...");
            Console.ReadKey();
        }

        private void Run()
        {
            Init();
            Play();
        }

        #region Play
        private void Play()
        {
            bool firstPlay = true;
            while (client.State)
            {
                foreach (ClientPlaylist playlist in client.Playlists)
                {
                    foreach (Domain.Concrete.Ad ad in playlist.Playlist.Ads)
                    {
                        if (!File.Exists(Path.Combine(adSaveLocation, ad.Name + ad.FileExtension)))
                            DownLoadFileFromRemoteLocation(ad.Name + ad.FileExtension);
                    }
                }

                if (firstPlay)
                {
                    foreach (ClientPlaylist playlist in client.Playlists)
                    {
                        StartThread(playlist);
                    }

                    firstPlay = false;
                }

                Thread.Sleep(sleepBetweenUpdate);

                FindItSelf();
            }
        }

        private void StartThread(IClientPlaylist playlist)
        {
            Console.WriteLine(string.Format("Starting a Thread for playing the playlist: {0}...", playlist.Playlist.Name));
            Thread thread = new Thread(() => PlayPlaylist(playlist));
            thread.Start();
            playlistThreads.Add(thread);
        }

        private void PlayPlaylist(IClientPlaylist playlist)
        {
            while (client.State)
            {
                while (DateTime.Now.TimeOfDay >= playlist.StartTime
                       && DateTime.Now.TimeOfDay < playlist.EndTime)
                {
                    IAd ad = PickAnAd(playlist.Playlist);
                    if (ad != null) 
                    {
                        ad = PlayAd(ad);
                        bool result = handler.Update(ad);
                    }
                }

                Thread.Sleep(sleepDuringOffTime);
            }
        }

        private IAd PlayAd(IAd ad)
        {
            Console.WriteLine(string.Format("Playing File: {0}...", ad.Name));
            string path = Path.Combine(adSaveLocation, ad.Name + ad.FileExtension);

            player = new MediaPlayer(vlc);
            Media file = new Media(vlc, path);

            player.ToggleFullscreen();
            player.Play(file);

            while (file.State != VLCState.Ended)
            {
                Thread.Sleep(sleepDuringPlay);
            }

            player.Dispose();

            ad.TotalPlayCount++;

            return ad;
        }

        #region Pick An Ad
        private IAd PickAnAd(IPlaylist playlist)
        {
            Random random = new Random();
            Console.WriteLine("Picking an Ad from Playlist...");
            IAd result = null;

            switch (playlist.PlayMethod)
            {
                case Enums.PlayMethods.SinglePlaythrough:
                    result = PickSinglePlaythrough(playlist);
                    break;
                case Enums.PlayMethods.SingleRandomPlaythrough:
                    result = PickSingleRandomPlaythrough(playlist, random);
                    break;
                case Enums.PlayMethods.LoopedPlaythrough:
                    result = PickLoopedPlaythrough(playlist);
                    break;
                case Enums.PlayMethods.LoopedRandomPlaythrough:
                    result = PickLoopedRandomPlaythrough(playlist, random);
                    break;
                default:
                    break;
            }

            return result;
        }

        private IAd PickLoopedRandomPlaythrough(IPlaylist playlist, Random random)
        {
            IAd result = null;
            IPlaylistAd playAd = null;

            if (previousPlayedAds.Count == playlist.Ads.Count)
                previousPlayedAds.Clear();

            int index = 0;
            int next = previousPlayedAds.Count == 0 ? 0 : random.Next(0, playlist.Ads.Count - 1);

            while (previousPlayedAds.Contains(next + 1))
            {
                next = random.Next(0, playlist.Ads.Count - 1);
            }

            playAd = playlist.Ads.ElementAt(next);
            result = playAd.Ad;
            index += ((List<IPlaylistAd>)playlist.Ads).IndexOf(playAd) + 1;
            previousPlayedAds.Add(index);

            return result;
        }

        private IAd PickLoopedPlaythrough(IPlaylist playlist)
        {
            IAd result = null;
            IPlaylistAd playAd = null;

            if (previousPlayedAds.Count == playlist.Ads.Count)
                previousPlayedAds.Clear();

            int index = 0;
            int next =  previousPlayedAds.Count == 0 ? 0 : previousPlayedAds.Last();

            if (!(next >= playlist.Ads.Count))
            {
                playAd = playlist.Ads.ElementAt(next);
                result = playAd.Ad;
                index += ((List<IPlaylistAd>)playlist.Ads).IndexOf(playAd) + 1;
                previousPlayedAds.Add(index);
            }

            return result;
        }

        private IAd PickSingleRandomPlaythrough(IPlaylist playlist, Random random)
        {
            IAd result = null;
            IPlaylistAd playAd = null;

            if (previousPlayedAds.Count == playlist.Ads.Count)
                return result;

            int index = 0;
            int next = random.Next(0, playlist.Ads.Count - 1);

            while (previousPlayedAds.Contains(next+1))
            {
                next = random.Next(0, playlist.Ads.Count - 1);
            }

            playAd = playlist.Ads.ElementAt(next);
            result = playAd.Ad;
            index += ((List<IPlaylistAd>)playlist.Ads).IndexOf(playAd) + 1;
            previousPlayedAds.Add(index);

            return result;

        }

        private IAd PickSinglePlaythrough(IPlaylist playlist)
        {
            IAd result = null;
            IPlaylistAd playAd = null;

            int index = 0;
            int next = previousPlayedAds.Last();
            
            if (!(next >= playlist.Ads.Count))
            {
                playAd = playlist.Ads.ElementAt(next);
                result = playAd.Ad;
                index += ((List<IPlaylistAd>) playlist.Ads).IndexOf(playAd) + 1;
                previousPlayedAds.Add(index);
            }

            return result;
        }

        #endregion


        #endregion

        #region Download & Update
        private void DownLoadFileFromRemoteLocation(string fileNameAndExtension, string downloadedFileSaveLocation = @"..\Ads\")
        {
            Console.WriteLine(string.Format("Downloading File {0} from Server...", fileNameAndExtension));
            try
            {
                using (var fileStream = service.DownloadFile(fileNameAndExtension))
                {
                    if (fileStream == null)
                    {
                        Console.WriteLine("File was not recieved...");
                        return;
                    }

                    SharedCode.SaveFile(Path.Combine(downloadedFileSaveLocation, fileNameAndExtension), fileStream);
                }
                Console.WriteLine("File succesfully downloaded and copied...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("File could not be downloaded or saved. Message :" + ex.Message);
            }
        }


        #endregion

        #region Initialize
        private void Init()
        {
            Console.WriteLine("Initializing Connection to Service...");
            service = new ClientServiceClient();

            Console.WriteLine("Initializing Connection to Database...");
            try
            {
                handler = new EntityRepositoryHandler(service.GetHandlerConnectionString(), ensureDeleted: false);
            }
            catch (EndpointNotFoundException ex)
            {
                Console.WriteLine("Server is Offline, Unable to communicate with server to get Connection String...");
                Console.WriteLine(string.Format("{0}{1}{2}", Environment.NewLine + ex.Message,
                    Environment.NewLine + ex.StackTrace, Environment.NewLine));

                Console.WriteLine("Press Any Key to Close...");
                Console.ReadKey();

                throw ex;
            }

            Console.WriteLine("Initializing VLC...");
            Core.Initialize();
            vlc = new LibVLC();

            if (!File.Exists(localSaveFile))
            {
                Console.WriteLine("Creating Client in Database...");
                client = new Domain.Concrete.Client() { Ip = GetIP(), Name = GetHostName() };
                bool result = handler.Add(client, true);
                SaveKeyInfos();
            }
            FindItSelf();
        }

        private string GetHostName()
        {
            Console.WriteLine("Getting Computer Name...");
            string hostName = Dns.GetHostName();

            return hostName;
        }

        // https://www.c-sharpcorner.com/UploadFile/167ad2/get-ip-address-using-C-Sharp-code/
        private string GetIP()
        {
            Console.WriteLine("Getting Computer External Ip...");
            // Retrive the Name of HOST 
            string hostName = GetHostName();

            // Get the IP  
            string ip = Dns.GetHostEntry(hostName).AddressList[1].ToString();

            return ip;
        }


        #endregion

        #region Find It Self
        private void SaveKeyInfos()
        {
            Console.WriteLine("Saving Key Information Locally...");
            using (TextWriter write = new StreamWriter(localSaveFile, false))
            {
                StringBuilder builder = new StringBuilder();

                builder.Append(client.Id);
                builder.Append("|");
                builder.Append(client.Ip);
                builder.Append("|");
                builder.Append(client.Name);

                write.WriteLine(builder.ToString());
            }
        }

        private void FindItSelf()
        {
            Console.WriteLine("Attempting to Find itself in the Database...");
            (int id, string ip, string name) = ReadKeyInfos();

            IClient predicate = null;

            if (id != 0)
                predicate = new Domain.Concrete.Client() { Id = id };
            else
                predicate = new Domain.Concrete.Client() { Ip = ip, Name = name };

            lock (_lock)
            {
                client = handler.Find(predicate);
            }
            SaveKeyInfos();
        }

        private (int Id, string Ip, string Name) ReadKeyInfos()
        {
            Console.WriteLine("Reading Key Information from File...");
            using (var reader = new StreamReader(localSaveFile))
            {
                string[] info = reader.ReadLine().Split('|');

                int id = int.Parse(info[0]);

                return (id, info[1], info[2]);
            }
        }


        #endregion
    }
}
