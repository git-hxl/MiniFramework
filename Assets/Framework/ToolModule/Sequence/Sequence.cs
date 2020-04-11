using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MiniFramework
{
    public static class SequenceAction
    {
        public static SequenceNode Sequence(this MonoBehaviour mono)
        {
            return new SequenceNode(mono);
        }
        public static SequenceNode Delay(this SequenceNode sequence, float delay)
        {
            sequence.Append(delay);
            return sequence;
        }
        public static SequenceNode Until(this SequenceNode sequence, Func<bool> condition)
        {
            sequence.Append(condition);
            return sequence;
        }
        public static SequenceNode Event(this SequenceNode sequence, Action action)
        {
            sequence.Append(action);
            return sequence;
        }

        public static SequenceNode Repeat(this SequenceNode sequence, int times, float interval, Action action)
        {
            sequence.Append(times, interval, action);
            return sequence;
        }
    }
}