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
        private Playback reproductorMidi;
        private MidiFile midiFile;
        private SevenBitNumber note;

        public PlaybackManager()
        {
            midiFile = MidiFile.Read("Assets/sevenNationArmyMap.mid");
            this.reproductorMidi = midiFile.GetPlayback();
            this.reproductorMidi.NoteCallback += EventoNota;


            //(rawNoteData, rawTime, rawLength, playbackTime) =>

            // new NotePlaybackData(
            // rawNoteData.NoteNumber,
            // rawNoteData.Velocity,     // leave velocity as is
            // rawNoteData.OffVelocity,  // leave off velocity as is
            // rawNoteData.Channel);
            // reproductorMidi.EventPlayed += ReproductorMidi_EventPlayed;
            // reproductorMidi.TrackNotes = true;


        }

        public void empezarMidi()
        {

            reproductorMidi.Start();

        }


        private NotePlaybackData EventoNota(NotePlaybackData rawData, long rawTime, long rawLength, TimeSpan playbackTime)
        {


            return rawData;

        }


        private void ReproductorMidi_EventPlayed(object sender, MidiEventPlayedEventArgs e)
        {

            if (e.Event.EventType == MidiEventType.NoteOn)
            {


            }
            var evento = (NoteEvent)e.Event;
            int numero = evento.NoteNumber;

        }

        public Playback ReproductorMidi { get => reproductorMidi; set => reproductorMidi = value; }
        public SevenBitNumber Note { get => note; set => note = value; }




    }


}
