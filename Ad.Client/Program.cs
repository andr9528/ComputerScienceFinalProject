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

        private void Play()
        {
            bool firstPlay = true;
            while (client.State)
            {
                foreach (ClientPlaylist playlist in client.Playlists)
                {
                    foreach (Domain.Concrete.Ad ad in playlist.Playlist.Ads  )
                    {
                        if (!File.Exists(Path.Combine(adSaveLocation, ad.Name + ad.FileExtension)))
                            DownLoadFileFromRemoteLocation(ad.Name + ad.FileExtension);
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
            Console.WriteLine(string.Format("Starting a Thread for playing the playlist: {0}...", playlist.Name));
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

        private IAd PickAnAd(IPlaylist playlist)
        {
            Console.WriteLine("Picking an Ad from Playlist...");

            throw new NotImplementedException();
        }

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
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                Console.WriteLine();
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
                client = new Domain.Concrete.Client() {Ip = GetIP(), Name = GetHostName()};
                bool result = handler.Add(client, true);
                SaveKeyInfos();
            }
            
            FindItSelf();
        }

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
            Console.WriteLine("Reading Key Information from File...");
            using (var reader = new StreamReader(localSaveFile))
            {
                string[] info = reader.ReadLine().Split('|');

                int id = int.Parse(info[0]);

                return (id, info[1], info[2]);
            }
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
    }
}
