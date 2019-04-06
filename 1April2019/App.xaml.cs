using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Speech;
using System.Speech.Synthesis;
using System.IO;

using _1April2019.Resource;
using _1April2019.Payload;

using System.Windows;

namespace _1April2019.Program
{
    public partial class App : Application
    {
        // Variables for the instance of the keyboard hook and the list of keys that have been pressed.
        private readonly KeyboardHook keyboardHook = new KeyboardHook();
        private readonly List<string> keys = new List<string>();

        // An instance for the first payload.
        private readonly ReversingScreen payloadOne = new ReversingScreen(false);

        #region Application startup listener
        /**
         * <summary>
         * This event activates as soon as the program is executed.
         * </summary>
         **/
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            // Gets the current system date and time.
            DateTime date = DateTime.Now;

            try
            {
                // This verifies if the time bomb setting is enabled in the App.config file.
                if (ConfigurationManager.AppSettings["TimeBombEnabled"] == "true")
                {
                    // This check ensures that the program is only run on 1/04/2019 and before midday.
                    if (date.Year != 2019 || date.Month != 4 || date.Day != 1 || date.Hour > 12)
                    {
                        throw new Exception("This application may only be run on April Fools of 2019 before midday.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "April Fools by Brandon", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.None);
                Environment.Exit(0);
            }

            // Begins a new thread to register any keyboard actions.
            new Thread(() =>
            {
                keyboardHook.KeyDown += new RawKeyEventHandler(Keyboard_KeyDown);
                Console.WriteLine("The keyboard hook is active.");
            }).Start();

            // Activates the first payload.
            PayloadPointer.ActivatePayload(Checkpoint.REVERSING_SCREEN);
            Console.WriteLine("Reversing screen payload activated.");
        }
        #endregion

        #region Application exit function
        /**
         * <summary>
         * This is a public function to allow other classes to exit the program while avoiding Windows thread security exceptions.
         * </summary>
         **/
        public static void Close()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region Keyboard key press listener
        /**
         * <summary>
         * This event activates whenever a key on the keyboard is pressed globally across Windows.
         * </summary>
         **/
        private void Keyboard_KeyDown(object sender, RawKeyEventArgs args)
        {
            // Registers the key that was pressed.
            Console.WriteLine(args.Key.ToString() + " was pressed.");
            keys.Add(args.Key.ToString());

            // Checks if 3 keys have been pressed in a consecutive manner, and if so, exit the program if it's the correct emergency
            // shutdown combination (LeftCtrl + LeftAlt + F5).
            if (keys.Count == 3)
            {
                if (keys[0] == "LeftCtrl" && keys[1] == "LeftAlt" && keys[2] == "F5")
                {
                    Console.WriteLine("Stop combination detected. Exiting now.");
                    Application.Current.Shutdown();
                }
                else
                {
                    Console.WriteLine("Three keys were pressed, however not the correct combination. The array was cleared.");
                    keys.Clear();
                    return;
                }
            }
        }
        #endregion

        #region Application exit listener
        /**
         * <summary>
         * This event activates when the program is closing.
         * </summary>
         **/
        private void App_OnExit(object sender, ExitEventArgs e)
        {
            // Reverts the display back to correct orientation and stops the input device hooks.
            Display.ResetAllRotations();
            keyboardHook.Dispose();

            // Aborts all threads relating to payload executions if they are active.
            try
            {
                if (payloadOne.IsRunning())
                {
                    payloadOne.StopPayload();
                }
            }
            // The following #pragma lines are used to suppress the informational C# 'Variable is declared, but never used' and the 'Variable is declared, but never assigned to' warning.
            #pragma warning disable CS0168
            #pragma warning disable IDE0059
            catch (Exception ex)
            #pragma warning restore IDE0059
            #pragma warning restore CS0168
            {
                return;
            }

            // Notifies the user of the exiting success.
            MessageBox.Show("The program exited successfully. Happy April Fools! Everything has been cleared and no damage has been inflicted onto your system or your data.", "April Fools by Brandon", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.None);

            // Attempts to delete the created temporary directory and startup entry.
            try
            {
                DeleteDirectory(Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\AppData\Local\1April2019");
                File.Delete(Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\activation.vbs");
            }
            catch (Exception)
            {
                return;
            }
            finally
            {
                // Finally, it terminates the program's process tree and exits the .NET environment.
                Process.GetCurrentProcess().Kill();
                Environment.Exit(0);
            }
        }
        #endregion

        #region Directory delete function
        /**
         * <summary>
         * Depth-first directory recursive deletion.
         * </summary>
         **/
        private static void DeleteDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(path, false);
        }
        #endregion
    }
}
