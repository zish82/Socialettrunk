using System;
using System.Collections.Generic;

namespace Socialet.ViewModels.Twitter
{
    public class TwitterDictionary : SortedDictionary<string, string>
    {
        public TwitterDictionary()
        {
        }

        public TwitterDictionary(string response)
        {
            var qSplit = response.Split('?');
            foreach (var kvp in qSplit[qSplit.Length - 1].Split('&'))
            {
                var kvpSplit = kvp.Split('=');
                if (kvpSplit.Length == 2)
                {
                    Add(kvpSplit[0], kvpSplit[1]);
                }
            }
        }

        public TwitterDictionary(TwitterDictionary src)
        {
            Add(src);
        }

        public void Add(TwitterDictionary src)
        {
            if (src != null)
            {
                foreach (var kvp in src)
                {
                    Add(kvp.Key, kvp.Value);
                }
            }
        }

        public String ToStringA()
        {
            String retVal = String.Empty;
            foreach (var kvp in this)
            {
                retVal += ((retVal.Length > 0) ? "&" : "") + kvp.Key + "=" + kvp.Value;
            }
            return retVal;
        }

        public String ToStringQ()
        {
            String retVal = String.Empty;
            foreach (var kvp in this)
            {
                retVal += ((retVal.Length > 0) ? ", " : "") + kvp.Key + "=" + "\"" + kvp.Value + "\"";
            }
            return retVal;
        }

        public override String ToString()
        {
            String retVal = String.Empty;
            foreach (var kvp in this)
            {
                retVal += ((retVal.Length > 0) ? ", " : "") + kvp.Key + "=" + kvp.Value;
            }
            return retVal;
        }
    }
}