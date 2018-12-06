using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace MedieFilePlayTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();

            Console.WriteLine("Press Any Key to Exit...");
            Console.ReadKey();
        }

        private void Run()
        {
            Console.WriteLine("Initializing...");
            Core.Initialize();
            LibVLC vlc = new LibVLC();
            string path = "\\Circle of Life.mp4";

            Console.WriteLine("Createing Player...");
            MediaPlayer player = new MediaPlayer(vlc);
            Media file = new Media(vlc, path);
            TimeSpan sleep = new TimeSpan(0,0,10);

            Console.WriteLine("Playing File...");
            player.ToggleFullscreen();
            player.Play(file);

            while (file.State != VLCState.Ended)
            {
                Thread.Sleep(sleep);
            }

            player.Dispose();
        }
    }
}
