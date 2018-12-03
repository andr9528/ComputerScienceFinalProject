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
using System.Threading;
using LibVLCSharp.Shared;

namespace Ad.Client
{
    class Program
    {
        private ClientServiceClient service = new ClientServiceClient();
        private object _lock = new object();
        private IRepository handler;
        private IClient client;
        private string localSaveFile = "..\\KeyInfos.txt";
        private readonly string adSaveLocation = "..\\Ads";
        private List<Thread> playlistThreads = new List<Thread>();
        private LibVLC vlc = new LibVLC();
        private TimeSpan sleepDuringPlay = new TimeSpan(0,0,10);
        private TimeSpan sleepDuringOffTime = new TimeSpan(0, 1, 0);
        private TimeSpan sleepBetweenUpdate = new TimeSpan(0, 15, 0);
        private MediaPlayer player;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            Init();
            Play();
        }

        private void Play()
        {
            bool firstPlay = true;
            while (client.State)
            {
                foreach (ClientPlaylist playlist in client.Playlists)
                {
                    foreach (Domain.Concrete.Ad ad in playlist.Playlist.Ads  )
                    {
                        if (!File.Exists(Path.Combine(adSaveLocation, ad.Name)))
                            Download(ad);
                    }
                }

                if (firstPlay)
                {
                    foreach (ClientPlaylist playlist in client.Playlists)
                    {
                        StartThread(playlist.Playlist);
                    }

                    firstPlay = false;
                }

                Thread.Sleep(sleepBetweenUpdate);

                FindItSelf();
            }
        }

        private void StartThread(IPlaylist playlist)
        {
            Thread thread = new Thread(() => PlayPlaylist(playlist));
            thread.Start();
            playlistThreads.Add(thread);
        }

        private void PlayPlaylist(IPlaylist playlist)
        {
            while (client.State)
            {
                while (DateTime.Now >= playlist.StartTime && DateTime.Now < playlist.EndTime)
                {
                    IAd ad = PickAnAd(playlist);
                    ad = PlayAd(ad);
                    bool result = handler.Update(ad);
                }

                Thread.Sleep(sleepDuringOffTime);
            }
        }

        private IAd PlayAd(IAd ad)
        {
            string path = Path.Combine(adSaveLocation, ad.Name);

            player = new MediaPlayer(vlc);
            Media file = new Media(vlc, path);

            Console.WriteLine("Playing File: " + ad.Name);
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

        private IAd PickAnAd(IPlaylist playlist)
        {
            throw new NotImplementedException();
        }

        private void Download(IAd ad)
        {
            //RemoteFileInfo fileinfo = service.DownloadFile(new DownloadRequest() {FileName = ad.Name});
        }

        private void Init()
        {
            handler = (IRepository) service.GetHandler();
            Core.Initialize();

            if (!File.Exists(localSaveFile))
            {
                client = new Domain.Concrete.Client() {Ip = GetIP(), Name = GetHostName()};
                bool result = handler.Add(client, true);
                SaveKeyInfos();
            }
            
            FindItSelf();
        }
        /// <summary>
        /// Saves the Id, Ip and Name of client to a local file to be used later to retrive from the database.
        /// </summary>
        private void SaveKeyInfos()
        {
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
            (int id, string ip, string name) = ReadKeyInfos();

            IClient predicate = null;

            if (id != 0)
                predicate = new Domain.Concrete.Client() {Id = id};
            else 
                predicate = new Domain.Concrete.Client() {Ip = ip, Name = name};

            lock (_lock)
            {
                client = handler.Find(predicate);
            }
            SaveKeyInfos();
        }

        private (int Id, string Ip, string Name) ReadKeyInfos()
        {
            using (var reader = new StreamReader(localSaveFile))
            {
                string[] info = reader.ReadLine().Split('|');

                int id = int.Parse(info[0]);

                return (id, info[1], info[2]);
            }
        }

        private string GetHostName()
        {
            string hostName = Dns.GetHostName();

            return hostName;
        }

        // https://www.c-sharpcorner.com/UploadFile/167ad2/get-ip-address-using-C-Sharp-code/
        private string GetIP()
        {
            // Retrive the Name of HOST 
            string hostName = Dns.GetHostName(); 
            
            // Get the IP  
            string ip = Dns.GetHostEntry(hostName).AddressList[0].ToString();

            return ip;
        }


    }
}
