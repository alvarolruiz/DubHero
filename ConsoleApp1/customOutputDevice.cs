using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class customOutputDevice : IOutputDevice
    {
        public customOutputDevice()
        {
        }

        public event EventHandler<MidiEventSentEventArgs> EventSent;

        public void PrepareForEventsSending()
        {
        }

        public void SendEvent(MidiEvent midiEvent)
        {
            Console.WriteLine(midiEvent.ToString());
        }
    }
}
