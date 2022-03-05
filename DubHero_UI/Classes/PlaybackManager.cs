using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.IO;
using Windows.Storage;

namespace DubHero_UI.Classes
{
    public class PlaybackManager
    {
        private Playback _midiPlayer;
        private MidiFile _midiFile;
        public AnimationCallback AnimationCallback { get; set; }

        public PlaybackManager()
        {
        }

        public async void InitReader(StorageFile midiFile)
        {
            var stream = await midiFile.OpenAsync(FileAccessMode.Read);
            _midiFile = MidiFile.Read(stream.AsStream());
            _midiPlayer = _midiFile.GetPlayback();
            // _midiPlayer.NoteCallback += NoteEvent; hacer esto en el hilo de la vista, en GameView
        }

        public void StartReading()
        {
            _midiPlayer.MoveToStart();
            _midiPlayer.Start();
        }

        public void PauseReading()
        {
            _midiPlayer.Stop();
        }

        public void ResumeReading()
        {
            _midiPlayer.Start();
        }


        private NotePlaybackData NoteEvent(NotePlaybackData rawData, long rawTime, long rawLength, TimeSpan playbackTime)
        {
            var readNote = new GameNote(rawData.NoteNumber);
            AnimationCallback(readNote);
            return null;
        }

        public Playback ReproductorMidi { get => _midiPlayer; set => _midiPlayer = value; }
    }


}
