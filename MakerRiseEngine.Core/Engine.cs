﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

namespace Maker.RiseEngine.Core
{
    public static class Engine
    {
        public static GraphicsDeviceManager graphics;
        public static Microsoft.Xna.Framework.Game MainGame;
        public static GraphicsDevice GraphicsDevice;
        public static string SaveLocation = "Save";
        public static GameWindow Window;
        public static System.Windows.Forms.Form GameForm;

        public static Version Version = Assembly.GetExecutingAssembly().GetName().Version;
        public static bool AsErrore = false;
        public static bool IsLoaded = false;
        public static int CurrentFrame = 0;
        public static UI.Cursor MouseCursor;

        public static Config.EngineConfig engineConfig = new Config.EngineConfig();

        public static void ReloadEngine()
        {

            EngineDebug.DebugLogs.WriteInLogs("Reloading...", EngineDebug.LogType.Info, "Engine");
            ContentEngine.ReloadContent();
            GameObjectsManager.Reload();
            Initializer();

        }

        public static void Initializer()
        {

            EngineDebug.DebugLogs.WriteInLogs("Initializing...", EngineDebug.LogType.Info, "Engine");

            if (System.IO.File.Exists("Data\\config.bin"))
                engineConfig = Storage.SerializationHelper.LoadFromBin<Config.EngineConfig>("Data\\config.bin");
            else
                Storage.SerializationHelper.SaveToBin(engineConfig, "Data\\config.bin");


            MouseCursor = new UI.Cursor();

            GameObjectsManager.InitializePlugin();


            if (GameObjectsManager.IsFullLoaded())
            {
                EngineDebug.DebugLogs.WriteInLogs("Initializing Done !", EngineDebug.LogType.Info, "Engine");
            }
            else
            {
                EngineDebug.DebugLogs.WriteInLogs("Initializing Failed !", EngineDebug.LogType.Info, "Engine");

                switch (System.Windows.Forms.MessageBox.Show("Gosh !" + Environment.NewLine + "An error that occurred during initialization of the engine.", "MakerRiseEngine " + Version, System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore, System.Windows.Forms.MessageBoxIcon.Error))
                {
                    case System.Windows.Forms.DialogResult.Abort:
                        System.Windows.Forms.Application.Exit();
                        break;
                    case System.Windows.Forms.DialogResult.Retry:
                        System.Windows.Forms.Application.Restart();
                        break;
                    case System.Windows.Forms.DialogResult.Ignore:
                        AsErrore = true;
                        break;
                    default:
                        break;
                }


            }

            IsLoaded = true;

        }

    }
}