using System;

namespace Interior.Helpers
{
    internal class Randomizer
    {
        internal Random rnd {  get; private set; }
        internal static Randomizer? Instance
        {
            get
            {
                if (field == null)
                    Instance = new Randomizer();

                return field;
            }

            private set;
        }

        private Randomizer()
        {
            rnd = new Random(StartOfRound.Instance.randomMapSeed + 69);
        }
    }
}
