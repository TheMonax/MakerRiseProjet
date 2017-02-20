﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maker.twiyol.Game.WorldDataStruct.Tags
{
    [Serializable]
    public class TagManger
    {

        Dictionary<string, Tag> Tags = new Dictionary<string, Tag>();

        public T GetTag<T>(string name, T defaultValue)
        {
            if (Tags.ContainsKey(name)) {
                return (T)Tags[name].Value;
            }
            else
            {
                return defaultValue;
            }
        }

        public void SetTag(string name, object value)
        {

            if (Tags.ContainsKey(name))
            {

                Tags[name].Value = value;

            }
            else
            {
                Tags.Add(name, new Tag(value));
            }
        }

        public bool HasTag(string name) {
            return Tags.ContainsKey(name);
        }
    }
}