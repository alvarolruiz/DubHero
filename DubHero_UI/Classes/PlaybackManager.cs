using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DubHero_UI.Classes
{
    public class PlaybackManager
    {
        private Playback midiPlayer;
        private MidiFile midiFile;
        public AnimationCallback AnimationCallback;

        public PlaybackManager(string midiPath)
        {
            midiFile = MidiFile.Read(midiPath);
            this.midiPlayer = midiFile.GetPlayback();
            this.midiPlayer.NoteCallback += NoteEvent;

        }

        public void StartReading()
        {
            midiPlayer.MoveToStart();
            midiPlayer.Start();
        }

        public void PauseReading()
        {
            midiPlayer.Stop();
        }

        public void ResumeReading()
        {
            midiPlayer.Start();
        }


        private NotePlaybackData NoteEvent(NotePlaybackData rawData, long rawTime, long rawLength, TimeSpan playbackTime)
        {
            var readNote = new GameNote(rawData.NoteNumber);
            AnimationCallback(readNote);
            return null;
        }

        public Playback ReproductorMidi { get => midiPlayer; set => midiPlayer = value; }
    }


}
