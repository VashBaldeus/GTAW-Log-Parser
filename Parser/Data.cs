using System;

namespace Parser
{
    class Data
    {
        #if DEBUG
            public static readonly string serverIP = "127.0.0.1_22005";
        #else
            public static readonly string serverIP = "164.132.206.209_22005";
        #endif

        public static readonly string logLogation = $"client_resources\\{serverIP}\\.storage";
    }
}
