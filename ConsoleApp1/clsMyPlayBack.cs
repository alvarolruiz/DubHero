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
    internal class clsMyPlayBack : Playback
    {
        public clsMyPlayBack(IEnumerable<ITimedObject> timedObjects, TempoMap tempoMap, PlaybackSettings playbackSettings = null) 
            : base(timedObjects, tempoMap, playbackSettings)
        {
        }
        protected override bool TryPlayEvent(MidiEvent midiEvent, object metadata)
        {
            bool sended = false;
            if (midiEvent.EventType==MidiEventType.NoteOn || midiEvent.EventType==MidiEventType.NoteOn ||
                midiEvent.EventType == MidiEventType.EndOfTrack || midiEvent.EventType == MidiEventType.Start)
            {
            
            }
            return base.TryPlayEvent(midiEvent, metadata);
        }
    }
}
