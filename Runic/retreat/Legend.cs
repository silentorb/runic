﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using runic.retreat.rhymes;

namespace runic.retreat
{
//    public enum Legend_Type
//    {
//        text,
//        group
//    }

    public abstract class Legend
    {
        public Rhyme rhyme;
//        public Legend_Type type;
        public abstract string text { get; }
        public abstract List<Legend> children { get; }
        public Position start;
        public Position end;

        public virtual string type
        {
            get { return rhyme.name; }
        }
    }

    [DebuggerDisplay("Legend {text}")]
    public class String_Legend : Legend
    {
        private string _text;
        override public string text
        {
            get { return _text; }
        }

        public override List<Legend> children
        {
            get { return null; }
        }

        public String_Legend(Rhyme rhyme, string text, Position start, Position end)
        {
//            type = Legend_Type.text;
            this.rhyme = rhyme;
            _text = text;
            this.start = start;
            this.end = end;
        }
    }

    [DebuggerDisplay("Legend {rhyme.debug_info} [{children.Count}]")]
    public class Group_Legend : Legend
    {
        private List<Legend> _children;
        public List<Legend> dividers;

        override public string text
        {
            get { return null; }
        }

        public override List<Legend> children
        {
            get { return _children; }
        }

        public Group_Legend(Rhyme rhyme, List<Legend> children, Position start, Position end, List<Legend> dividers = null)
        {
//            type = Legend_Type.group;
            this.rhyme = rhyme;
            _children = children;
            this.dividers = dividers;
            this.start = start;
            this.end = end;
        }
    }
}
