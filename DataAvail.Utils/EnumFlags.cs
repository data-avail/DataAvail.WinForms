using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    public static class EnumFlags
    {
        #region These methods work only for pure flag enums (int type)

        //http://stackoverflow.com/questions/553905/how-to-expand-flagged-enum
        public static IEnumerable<int> DecomposeEnum(int IntFlagEnum)
        {
            int i = 1;

            while (i > 0)
            {
                if ((IntFlagEnum & i) > 0)
                    yield return i;

                i <<= 1;
            }
        }

        public static T RecomposeEnum<T>(T IntFlagEnum, T ExceptFlagEnum) where T : struct
        {
            return (T)(object)RecomposeEnum((int)(object)IntFlagEnum, (int)(object)ExceptFlagEnum);
        }

        public static T ComposeEnum<T>(IEnumerable<T> Flags)
        {
            int res = 0x00;

            foreach (int flag in Flags.Cast<int>())
            {
                res |= flag;
            }

            return (T)(object)res;
        }


        public static int RecomposeEnum(int IntFlagEnum, int ExceptFlagEnum)
        {
            int res = 0;

            int i = 1;

            while (i > 0)
            {
                if ((IntFlagEnum & i) > 0 && (ExceptFlagEnum & i) <= 0)
                {
                    res |= i;
                }

                i <<= 1;
            }

            return res;
        }

        public static bool IsContain<T>(T FlagEnum, T FlagEnumEntity)
        {
            return ((int)(object)FlagEnum & (int)(object)FlagEnumEntity) == (int)(object)FlagEnumEntity;
        }

        public static bool IsContainMixed<T>(T FlagEnum, T FlagEnumEntity)
        {
            return !FlagEnum.Equals(FlagEnumEntity) &&
                ((int)(object)FlagEnum & (int)(object)FlagEnumEntity) == (int)(object)FlagEnumEntity;

        }

        public static T Parse<T>(string Str, T All)
        {
            IEnumerable<string> strEnties = Str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToUpper());

            if (strEnties.Contains("NONE")) return (T)(object)0;

            if (strEnties.Contains(All.ToString().ToUpper())) return All;

            IEnumerable<T> ets = DecomposeEnum((int)(object)All).Cast<T>().Where(p => strEnties.Contains(p.ToString().ToUpper()));

            return ComposeEnum(ets);
        }

        #endregion
    }
}
