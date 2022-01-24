using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Profiling;


    public class SoundSystem
    {
        private int _emittersValue = 5;
        private SoundEmitter[] _emitters;
        private GameObject _sourceHolder;
        private int _sequenceId;
        private AudioListener _currentAudioListener;
        private static AudioMixerGroup[] _mixerGroups;
        private AudioMixer _audioMixer;
        
        public void Init(AudioMixer audioMixer)
        {
            _audioMixer = audioMixer;
            _sourceHolder = new GameObject("SoundSystemSources");
            // Create pool of emitters
            _emitters = new SoundEmitter[_emittersValue];
            for (var i = 0; i < _emittersValue; i++)
            {
                var emitter = new SoundEmitter {source = MakeAudioSource()};
                // emitter.fadeToKill = new Interpolator(1.0f, Interpolator.CurveType.Linear);
                _emitters[i] = emitter;
            }
            
            // Set up mixer groups
            _mixerGroups = new AudioMixerGroup[(int)SoundMixerGroup._Count];
            _mixerGroups[(int)SoundMixerGroup.Music] = _audioMixer.FindMatchingGroups("Music")[0];
            _mixerGroups[(int)SoundMixerGroup.SFX] = _audioMixer.FindMatchingGroups("SFX")[0];
        }

        public void SetCurrentAudioListener(AudioListener audioListener)
        {
            _currentAudioListener = audioListener;
        }
        
        private AudioSource MakeAudioSource()
        {
            var go = new GameObject("SoundSystemSource");
            go.transform.parent = _sourceHolder.transform;
            return go.AddComponent<AudioSource>();
        }

        private SoundEmitter AllocEmitter()
        {
            // Look for unused emitter
            foreach (var e in _emitters)
            {
                if (!e.playing)
                {
                    e.seqId = _sequenceId++;
                    return e;
                }
            }
            
            // Hunt down one emitter to kill
            SoundEmitter emitter = null;
            float distance = float.MinValue;
            var listenerPos = _currentAudioListener.transform.position;
            foreach(var e in _emitters)
            {
                var s = e.source;

                // Skip looping
                if (s.loop)
                    continue;

                // Pick closest; assuming 2d sounds very close!
                var dist = 0.0f;
                if(s.spatialBlend > 0.0f)
                {
                    dist = (s.transform.position - listenerPos).magnitude;

                    // if tracking another object assume closer
                    var t = s.transform;
                    if (t.parent != _sourceHolder.transform)
                        dist *= 0.5f;
                }

                if(dist > distance)
                {
                    distance = dist;
                    emitter = e;
                }

            }
            if(emitter != null)
            {
                emitter.Kill();
                emitter.seqId = _sequenceId++;
                return emitter;
            }
            return null;
        }

        public void Play(SoundDef soundDef)
        {
            SoundEmitter e = AllocEmitter();
            Debug.Log(_currentAudioListener);
            e.source.transform.position = _currentAudioListener.transform.position;
            e.repeatCount = Random.Range(soundDef.repeatMin, soundDef.repeatMax);
            e.playing = true;
            e.soundDef = soundDef;

            StartEmitter(e);
        }

        private void StartEmitter(SoundEmitter emitter)
        {
            var soundDef = emitter.soundDef;
            var source = emitter.source;

            StartSource(source, soundDef);
        }
        
        public static void StartSource(AudioSource source, SoundDef soundDef)
        {
            source.clip = soundDef.clips[Random.Range(0, soundDef.clips.Count)];
            
            // Map from halftone space to linear playback multiplier
            source.pitch = Mathf.Pow(2.0f, Random.Range(soundDef.pitchMin, soundDef.pitchMax) / 12.0f);
            source.minDistance = soundDef.distMin;
            source.maxDistance = soundDef.distMax;
            source.volume = AmplitudeFromDecibel(soundDef.volume);
            source.priority = soundDef.priority;
            source.loop = soundDef.loopCount < 1 ? true : false;
            source.rolloffMode = soundDef.rolloffMode;
            float delay = Random.Range(soundDef.delayMin, soundDef.delayMax);
            if(_mixerGroups != null)
                source.outputAudioMixerGroup = _mixerGroups[(int)soundDef.soundGroup];
            source.spatialBlend = soundDef.spatialBlend;
            source.panStereo = Random.Range(soundDef.panMin, soundDef.panMax);
            
            source.Play();
        }

        public void Update()
        {
            // Update running sounds
            int count = 0;
            foreach (var e in _emitters)
            {
                if (!e.playing)
                    continue;
                if (e.source == null)
                {
                    // Could happen if parent was killed. Not good, but fixable:
                    e.source = MakeAudioSource();
                    e.repeatCount = 0;
                }
                if (e.source.isPlaying)
                {
                    count++;
                    continue;
                }
                if (e.repeatCount > 1)
                {
                    e.repeatCount--;
                    StartEmitter(e);
                    continue;
                }
                // Reset for reuse
                e.playing = false;
                e.source.transform.parent = _sourceHolder.transform;
                e.source.enabled = true;
                e.source.gameObject.SetActive(true);
                e.source.transform.position = Vector3.zero;
            }
        }
        
        public static float SOUND_VOL_CUTOFF = -60.0f;
        public static float AmplitudeFromDecibel(float decibel)
        {
            if (decibel <= SOUND_VOL_CUTOFF)
            {
                return 0;
            }
            return Mathf.Pow(2.0f, decibel / 6.0f);
        }
    }
