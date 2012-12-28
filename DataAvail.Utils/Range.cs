using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAvail.Utils
{
    public struct Range
    {
        public Range(int Number)
            : this(Number, Number)
        {
        }


        public Range(int MinNumber, int MaxNumber)
        {
            if (MinNumber != -1 && MaxNumber != -1 && MinNumber > MaxNumber)
                throw new ArgumentException("MinNumber can't be greater than MaxNumber");

            minNumber = MinNumber;

            maxNumber = MaxNumber;
        }

        public int minNumber;

        public int maxNumber;

        public bool LeftBounded
        {
            get { return minNumber != -1; }
        }

        public bool RightBounded
        {
            get { return maxNumber != -1; }
        }

        public bool Bounded
        {
            get { return LeftBounded || RightBounded; }
        }

        #region overridens

        public override string ToString()
        {
            return string.Format("[{0},{1}]", minNumber, maxNumber);
        }

        public override bool Equals(object obj)
        {
            return minNumber == ((Range)obj).minNumber && maxNumber == ((Range)obj).maxNumber;
        }

        public override int GetHashCode()
        {
            return minNumber.GetHashCode() & maxNumber.GetHashCode();
        }

        #endregion

        public string GetBoundErrorText(string ObjectName, int Number)
        {
            string err = null;

            if (LeftBounded && Number < minNumber)
                err = string.Format("The number of {0} can't be less than {1}, now it contains only {2} elements.", ObjectName, minNumber, Number);

            if (RightBounded && Number > maxNumber)
                err = string.Format("{0}The number of {1} can't be more than {2}, now it contains only {3} elements.",
                    ObjectName, maxNumber, Number, (string.IsNullOrEmpty(err) ? null : "\n" + err));

            return err;
        }


        public static Range NotBound { get { return new Range(-1); } }

    }
}
