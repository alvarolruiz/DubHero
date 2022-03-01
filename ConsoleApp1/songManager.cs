using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisioForge.MediaFramework.ONVIF;

namespace ConsoleApp1
{
    public class songManager
    {
        public static songManager Instance;
        public AudioSource audioSource;
        public float songDelayInSeconds;
        public double marginOfError; // in seconds

        public int inputDelayInMilliseconds;


        public string fileLocation;
        public float noteTime;
        // Donde aparece la nota
        public float noteSpawnY;
        // Donde deberia ser pulsada
        public float noteTapY;
        //Donde desaparece la nota
        public float noteDespawnY
        {
            get
            {
                return noteTapY - (noteSpawnY - noteTapY);
            }
        }
        public static MidiFile midiFile;
        private void ReadFromFile()
        {
            midiFile = MidiFile.Read("C:/Users/alvar/source/repos/DubHero/ConsoleApp1/sevenNationArmyMap.mid");
            GetDataFromMidi();
        }
        public void GetDataFromMidi()
        {
            var notes = midiFile.GetNotes();
            var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
            notes.CopyTo(array, 0);

            foreach (Note n in array)
            {
                // Comprobar la nota e introducir en un  array de essa nota.
                if (n.NoteNumber==60)
                {

                }
            }
            ;

            //Invoke(nameof(StartSong), songDelayInSeconds);
        }
        public void StartSong()
        {
            //audioSource.Play();
        }
    }

}
