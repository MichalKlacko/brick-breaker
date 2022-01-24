using UnityEngine;


    public class SoundEmitter
    {
        public AudioSource source;
        public SoundDef soundDef;
        public bool playing;
        public int repeatCount;
        public int seqId;

        internal void Kill()
        {
            source.Stop();
            repeatCount = 0;
        }
    };
