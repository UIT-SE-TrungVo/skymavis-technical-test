using System.Collections.Generic;

namespace Assignment.GameAxie
{
    public enum AxieType
    {
        Attacker,
        Defender,
    }

    public class AxieTypeDict
    {
        private static readonly Dictionary<AxieType, string> AxieType2AxieId = new Dictionary<AxieType, string>
        {
            { AxieType.Attacker, "4191804" },
            { AxieType.Defender, "2724598" }
        };

        public static string GetAxieId(AxieType type)
        {
            return AxieType2AxieId[type];
        }
    }
}