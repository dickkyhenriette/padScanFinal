using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace padScanFinal
{
    class sigscan
    {

        /// <summary> usage
        /// string hoek = "\x48\x83\xec\x00\xe8";
        ///byte[] getup = sigscan.ToByteArray(hoek);
        ///string getstring = BitConverter.ToString(getup, 0);
        ///label1.Text = getstring;
        /// </summary>
        public static byte[] ToByteArray(string value)
           {
            char[] charArr = value.ToCharArray();
            byte[] bytes = new byte[charArr.Length];
            for (int i = 0; i < charArr.Length; i++)
            {
                byte current = Convert.ToByte(charArr[i]);
                bytes[i] = current;
            }

            return bytes;
          }

    }
}
