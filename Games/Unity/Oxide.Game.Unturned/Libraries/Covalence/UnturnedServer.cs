﻿using System;
using System.IO;
using System.Net;

using SDG.Unturned;
using Steamworks;

using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Game.Unturned.Libraries.Covalence
{
    /// <summary>
    /// Represents the server hosting the game instance
    /// </summary>
    public class UnturnedServer : IServer
    {
        #region Information

        /// <summary>
        /// Gets/sets the public-facing name of the server
        /// </summary>
        public string Name
        {
            get { return Provider.serverName; }
            set { Provider.serverName = value; }
        }

        /// <summary>
        /// Gets the public-facing IP address of the server, if known
        /// </summary>
        public IPAddress Address
        {
            get
            {
                var ip = SteamGameServer.GetPublicIP();
                return ip == 0 ? null : new IPAddress(ip >> 24 | ((ip & 0xff0000) >> 8) | ((ip & 0xff00) << 8) | ((ip & 0xff) << 24));
            }
        }

        /// <summary>
        /// Gets the public-facing network port of the server, if known
        /// </summary>
        public ushort Port => Provider.port;

        /// <summary>
        /// Gets the version or build number of the server
        /// </summary>
        public string Version => Provider.APP_VERSION;

        /// <summary>
        /// Gets the network protocol version of the server
        /// </summary>
        public string Protocol => Version;

        /// <summary>
        /// Gets/sets the maximum players allowed on the server
        /// </summary>
        public int MaxPlayers
        {
            get { return Provider.maxPlayers; }
            set { Provider.maxPlayers = (byte)value; }
        }

        #endregion

        #region Administration

        /// <summary>
        /// Saves the server and any related information
        /// </summary>
        public void Save() => SaveManager.save();

        #endregion

        #region Chat and Commands

        /// <summary>
        /// Broadcasts a chat message to all player clients
        /// </summary>
        /// <param name="message"></param>
        public void Broadcast(string message) => ChatManager.sendChat(EChatMode.GLOBAL, message);

        /// <summary>
        /// Runs the specified server command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public void Command(string command, params object[] args)
        {
            Commander.execute(CSteamID.Nil, $"{command} {string.Join(" ", Array.ConvertAll(args, x => x.ToString()))}");
        }

        #endregion

        #region Logging

        /// <summary>
        /// Logs a string of text to a file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="owner"></param>
        public void Log(string text, Plugin owner)
        {
            using (var writer = new StreamWriter(Path.Combine(Interface.Oxide.LogDirectory, Utility.CleanPath(owner.Filename + ".txt")), true))
                writer.WriteLine(text);
        }

        #endregion
    }
}
