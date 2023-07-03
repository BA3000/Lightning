using System;


namespace OpenBLive.Runtime.Utilities
{
    public static class CommandLineTools
    {
        private const string k_RoomIdArgs = "room_id=";

        public static int GetRoomIdViaCmdLineArgs()
        {
            var roomId = 0;
            var args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (arg.Contains(k_RoomIdArgs))
                {
                    roomId = Convert.ToInt32(arg.Substring(k_RoomIdArgs.Length, arg.Length - k_RoomIdArgs.Length));
                }
            }

            return roomId;
        }
    }
}