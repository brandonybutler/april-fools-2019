using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading;
using System.Diagnostics;
using System.Security.Permissions;

using _1April2019.Resource;
using _1April2019.Program;

namespace _1April2019.Payload
{
    public class ReversingScreen : IPayload
    {
        // Variables that declare whether the payload is currently running, the music player object and the current date and time.
        private readonly bool isRunning = false;
        private static SoundPlayer music;
        private readonly DateTime start = DateTime.Now;

        // Variables for each of the threads involved in this payload.
        private readonly Thread screenRotator = new Thread(new ThreadStart(ScreenRotation));
        private readonly Thread musicPlayer = new Thread(new ThreadStart(MusicPlayer));

        #region Class constructor and initializer
        /**
         * The thread executor and timekeeper of the ReversingScreen payload. The run parameter must be set to true if you intend to run it, however
         * it can be set to false if you are just instantiating the object.
         **/
        public ReversingScreen(bool run)
        {
            if (run)
            {
                isRunning = true;

                // Begins all threads.
                screenRotator.Start();
                musicPlayer.Start();

                // The timekeeping functionality to ensure that after 1 minute has elapsed, everything will be stopped and reverted to defaults.
                while (true)
                {
                    if ((DateTime.Now - start).TotalMinutes >= 1)
                    {
                        Console.WriteLine("One minute has passed since the payload was initially activated.");
                        StopPayload();
                        Display.ResetAllRotations();
                        isRunning = false;
                        App.Close();
                        break;
                    }
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region Screen rotating thread
        /**
         * <summary>
         * Thread responsible for screen rotation.
         * </summary>
         **/
        private static void ScreenRotation()
        {
            // While the thread is active, the screen will spin in a full circle with a 5 second delay of each other.
            while (true)
            {
                Display.Rotate(1, Display.Orientations.DEGREES_CW_270);
                Thread.Sleep(5000);
                Display.Rotate(1, Display.Orientations.DEGREES_CW_180);
                Thread.Sleep(5000);
                Display.Rotate(1, Display.Orientations.DEGREES_CW_90);
                Thread.Sleep(5000);
                Display.Rotate(1, Display.Orientations.DEGREES_CW_0);
            }
        }
        #endregion

        #region Music playing thread
        /**
         * <summary>
         * Thread responsible for playing the music.
         * </summary>
         **/
        private static void MusicPlayer()
        {
            // This will play the Seinfeld WAV from the Resource.resx file.
            music = new SoundPlayer(_1April2019.Properties.Resources.Seinfeld);
            music.Play();
        }
        #endregion

        #region Is running function
        /**
         * <summary>
         * This will return true or false depending on whether the payload is currently running.
         * </summary>
         **/
        public bool IsRunning()
        {
            return isRunning;
        }
        #endregion

        #region Stop payload function
        /**
         * <summary>
         * This will forcedly terminate the payload and all of its threads.
         * </summary>
         **/
        [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
        public void StopPayload()
        {
            if (isRunning)
            {
                screenRotator.Abort();
                musicPlayer.Abort();
                Console.WriteLine("The payload has been stopped.");
            }
            else
            {
                throw new Exception("The payload is not currently running, and therefore cannot be stopped.");
            }
        }
        #endregion
    }
}
