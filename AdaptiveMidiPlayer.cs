using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;
using MeltySynth;

namespace AdaptiveMusicSample
{
    public class AdaptiveMidiPlayer : IDisposable
    {
        private static readonly int sampleRate = 44100;
        private static readonly int bufferLength = sampleRate / 10;

        private Synthesizer synthesizer;
        private AdaptiveMidiFileSequencer sequencer;

        private DynamicSoundEffectInstance dynamicSound;
        private byte[] buffer;

        public AdaptiveMidiPlayer(string soundFontPath)
        {
            synthesizer = new Synthesizer(soundFontPath, sampleRate);
            sequencer = new AdaptiveMidiFileSequencer(synthesizer);

            dynamicSound = new DynamicSoundEffectInstance(sampleRate, AudioChannels.Stereo);
            buffer = new byte[4 * bufferLength];

            dynamicSound.BufferNeeded += (s, e) => SubmitBuffer();
        }

        public void Play(AdaptiveMidiFile midiFile, bool loop)
        {
            sequencer.Play(midiFile, loop);

            if (dynamicSound.State != SoundState.Playing)
            {
                SubmitBuffer();
                dynamicSound.Play();
            }
        }

        public void Stop()
        {
            sequencer.Stop();
        }

        private void SubmitBuffer()
        {
            sequencer.RenderInterleavedInt16(MemoryMarshal.Cast<byte, short>(buffer));
            dynamicSound.SubmitBuffer(buffer, 0, buffer.Length);
        }

        public void Dispose()
        {
            if (dynamicSound != null)
            {
                dynamicSound.Dispose();
                dynamicSound = null;
            }
        }

        public void EnableTrack(int track)
        {
            sequencer.EnableTrack(track);
        }

        public void DisableTrack(int track)
        {
            sequencer.DisableTrack(track);
        }

        public SoundState State => dynamicSound.State;
    }
}
