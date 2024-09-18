using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitBoardCore
{
    public static class BitUtility
    {
        // Flip a bit in a position
        public static byte FlipBit(byte val, int pos)
        {
            return (byte)(val ^ (1 << pos));
        }

        public static ushort FlipBit(ushort val, int pos)
        {
            return (ushort)(val ^ ((ushort)1 << pos));
        }

        public static uint FlipBit(uint val, int pos)
        {
            return val ^ (1u << pos);
        }

        public static ulong FlipBit(ulong val, int pos)
        {
            return (ulong)(val ^ (1ul << pos));
        }

        // Set the bit in the specified position  to 1
        public static byte SetBit(byte val, int pos)
        {
            return (byte)(val | ((byte)1 << pos));
        }

        public static ushort SetBit(ushort val, int pos)
        {
            return (ushort)(val | ((ushort)1 << pos));
        }

        public static uint SetBit(uint val, int pos)
        {
            return (val | (1u << pos));
        }

        public static ulong SetBit(ulong val, int pos)
        {
            return (ulong)(val | (1ul << pos));
        }

        // Clear the bit in the specified position
        public static byte ClearBit(byte val, int pos)
        {
            return (byte)(val & ~((byte)1 << pos));
        }

        public static ushort ClearBit(ushort val, int pos)
        {
            return (ushort)(val & ~((ushort)1 << pos));
        }

        public static uint ClearBit(uint val, int pos)
        {
            return (val & ~(1u << pos));
        }

        public static ulong ClearBit(ulong val, int pos)
        {
            return (ulong)(val & ~(1ul << pos));
        }

        // Toggle the bit at the specified position
        public static byte ToggleBit(byte val, int pos)
        {
            return (byte)(val ^ ((byte)1 << pos));
        }

        public static ushort ToggleBit(ushort val, int pos)
        {
            return (ushort)(val ^ ((ushort)1 << pos));
        }

        public static uint ToggleBit(uint val, int pos)
        {
            return (val ^ (1u << pos));
        }

        public static ulong ToggleBit(ulong val, int pos)
        {
            return (ulong)(val ^ (1ul << pos));
        }

        // Check a bit at the specified location
        public static uint CheckBit(uint val, int pos)
        {
            return (val >> pos) & 1u;
        }

        public static ushort CheckBit(ushort val, int pos)
        {
            return (ushort)((val >> pos) & (ushort)1);
        }

        public static byte CheckBit(byte val, int pos)
        {
            return (byte)((val >> pos) & (byte)1);
        }

        public static ulong CheckBit(ulong val, int pos)
        {
            return (ulong)((val >> pos) & 1ul);
        }

        // Swap Bit
        /// <summary>
        /// Move a bit from one position to another
        /// </summary>
        /// <param name="val"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static byte MoveBit(byte val, int from, int to)
        {
            return SetBit(ClearBit(val, from), to);
        }
        /// <summary>
        /// Move a bit from one position to another
        /// </summary>
        /// <param name="val"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static ushort MoveBit(ushort val, int from, int to)
        {
            return SetBit(ClearBit(val, from), to);
        }
        /// <summary>
        /// Move a bit from one position to another
        /// </summary>
        /// <param name="val"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static uint MoveBit(uint val, int from, int to)
        {
            return SetBit(ClearBit(val, from), to);
        }
        /// <summary>
        /// Move a bit from one position to another
        /// </summary>
        /// <param name="val"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static ulong MoveBit(ulong val, int from, int to)
        {
            return SetBit(ClearBit(val, from), to);
        }

        // Shift Left
        public static byte ShiftLeft(byte val, int pos)
        {
            return (byte)(val << pos);
        }

        public static ushort ShiftLeft(ushort val, int pos)
        {
            return (ushort)(val << pos);
        }

        public static uint ShiftLeft(uint val, int pos)
        {
            return (val << pos);
        }

        public static ulong ShiftLeft(ulong val, int pos)
        {
            return (ulong)(val << pos);
        }

        // Shift Right
        public static byte ShiftRight(byte val, int pos)
        {
            return (byte)(val >> pos);
        }

        public static ushort ShiftRight(ushort val, int pos)
        {
            return (ushort)(val >> pos);
        }

        public static uint ShiftRight(uint val, int pos)
        {
            return (val >> pos);
        }

        public static ulong ShiftRight(ulong val, int pos)
        {
            return (ulong)(val >> pos);
        }

        /// <summary>
        /// Get the index of the first bit in the bitMask
        /// </summary>
        /// <param name="bitMask"></param>
        /// <returns></returns>
        public static int GetFirstBitPosition(uint bitMask)
        {
            uint oneMask = 1u;
            int index = 0;
            while((oneMask & (bitMask >> index)) == 0)
            {
                index++;
                if(index >= sizeof(uint)*8)
                {
                    return -1;
                }
            }
            return index;
        }

        /// <summary>
        /// Returns true if any bits overlap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool BitMatch(byte a, byte b)
        {
            return (a & b) != 0;
        }
        /// <summary>
        /// Returns true if any bits overlap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool BitMatch(ushort a, ushort b)
        {
            return (a & b) != 0;
        }
        /// <summary>
        /// Returns true if any bits overlap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool BitMatch(uint a, uint b)
        {
            return (a & b) != 0;
        }
        /// <summary>
        /// Returns true if any bits overlap
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool BitMatch(ulong a, ulong b)
        {
            return (a & b) != 0;
        }

        /// <summary>
        /// A method to create a bit mask
        /// The bit mask is created by setting the bits from start to end to 1
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static uint CreateBitMask(int start, int end)
        {
            return (uint)(((1 << (end - start + 1)) - 1) << start);
        }

        /// <summary>
        /// To convert a number to a binary string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToBinaryString(uint val)
        {
            StringBuilder builder = new StringBuilder();

            for(int i = (sizeof(uint)*8)-1; i >= 0; i--)
            {
                builder.Append(CheckBit(val, i) == 1u ? '1' : '0');
                if((i%8)==0)
                {
                    builder.Append(' ');
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// To convert a number to a hexadecimal string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToHexString(uint val)
        {
            return val.ToString("X").PadLeft(8, '0');
        }

    }
}
