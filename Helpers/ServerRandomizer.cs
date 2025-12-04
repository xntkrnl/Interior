using System;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Networking.QoS;

namespace Interior.Helpers
{
    internal class ServerRandomizer : NetworkBehaviour
    {
        private Random rnd;

        private static ServerRandomizer? _instance;
        internal static ServerRandomizer? Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ServerRandomizer();
                    _instance.rnd = new Random(StartOfRound.Instance.randomMapSeed + 69);
                    //only host will decide our random so doesnt matter if clients also write its own seed (even if they are latejoiners)
                }

                return _instance;
            }

            private set;
        }
    }
}
