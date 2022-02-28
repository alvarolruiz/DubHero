using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class myPlayBack
    {
        private Playback reproductorMidi;
        private MidiFile midiFile;


        public myPlayBack()
        {
            //Necesitariamos un output device personalizado que simplemente escriba en consola los datos.
            midiFile  = MidiFile.Read("C:/Users/alvar/source/repos/DubHero/ConsoleApp1/sevenNationArmyMap.mid");
            TempoMap tm = midiFile.GetTempoMap();
            var tn = midiFile.GetTimedEvents();
            
            // Se establece el timer del playback
            this.reproductorMidi= midiFile.GetPlayback();
            this.reproductorMidi.NoteCallback = (rawNoteData, rawTime, rawLength, playbackTime) =>
           new NotePlaybackData(
            // Si se quisiera hacer alguna modificacion sobre las notas a la hora de reproducirlas se haria aqui
            (SevenBitNumber)rawNoteData.NoteNumber,6
            rawNoteData.Velocity,
            rawNoteData.OffVelocity,
            rawNoteData.Channel) ;
            /*
             * Si quisieramos bajar la velocidad de reproduccion. 0.5, 1, 2...
            reproductorMidi.Speed = 1;
            */
            reproductorMidi.EventPlayed+= ReproductorMidi_EventPlayed;
            reproductorMidi.Start();
            
        }



        // TODO >_> Este evento será el que se lance y le dará con el tiempo en el que ha sido lanzado (getCurrentTime)
        // midiEventPlayedEventArgs no me da el num de nota tocada, q hasemo

        private void ReproductorMidi_EventPlayed(object sender, MidiEventPlayedEventArgs e)
        {
            if(e.Event.EventType==MidiEventType.NoteOn){
                var eventoNota = (NoteEvent) e.Event;
                eventoNota.NoteNumber;
                //setear una propiedad a el nombre de la nota 
            }else if (e.Event.EventType==MidiEventType.NoteOff){
                Console.WriteLine(e.Event);
            }
        }

    }
}
