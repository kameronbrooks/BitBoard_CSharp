using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BitBoardCore
{
    public class BitUtility
    {
        // Flip a bit in a position
        public byte FlipBit(byte val, int pos)
        {
            return (byte)(val ^ (1 << pos));
        }

        public ushort FlipBit(ushort val, int pos)
        {
            return (ushort)(val ^ ((ushort)1 << pos));
        }

        public uint FlipBit(uint val, int pos)
        {
            return val ^ (1u << pos);
        }

        public ulong FlipBit(ulong val, int pos)
        {
            return (ulong)(val ^ (1ul << pos));
        }

        // Set the bit in the specified position  to 1
        public byte SetBit(byte val, int pos)
        {
            return (byte)(val & ((byte)1 << pos));
        }

        public ushort SetBit(ushort val, int pos)
        {
            return (ushort)(val & ((ushort)1 << pos));
        }

        public uint SetBit(uint val, int pos)
        {
            return (val & (1u << pos));
        }

        public ulong SetBit(ulong val, int pos)
        {
            return (ulong)(val & (1ul << pos));
        }

        // Clear the bit in the specified position
        public byte ClearBit(byte val, int pos)
        {
            return (byte)(val & ~((byte)1 << pos));
        }

        public ushort ClearBit(ushort val, int pos)
        {
            return (ushort)(val & ~((ushort)1 << pos));
        }

        public uint ClearBit(uint val, int pos)
        {
            return (val & ~(1u << pos));
        }

        public ulong ClearBit(ulong val, int pos)
        {
            return (ulong)(val & ~(1ul << pos));
        }

        // Toggle the bit at the specified position
        public byte ToggleBit(byte val, int pos)
        {
            return (byte)(val ^ ((byte)1 << pos));
        }

        public ushort ToggleBit(ushort val, int pos)
        {
            return (ushort)(val ^ ((ushort)1 << pos));
        }

        public uint ToggleBit(uint val, int pos)
        {
            return (val ^ (1u << pos));
        }

        public ulong ToggleBit(ulong val, int pos)
        {
            return (ulong)(val ^ (1ul << pos));
        }

        // Check a bit at the specified location
        public uint CheckBit(uint val, int pos)
        {
            return (val >> pos) & 1u;
        }

        public ushort CheckBit(ushort val, int pos)
        {
            return (ushort)((val >> pos) & (ushort)1);
        }

        public byte CheckBit(byte val, int pos)
        {
            return (byte)((val >> pos) & (byte)1);
        }

        public ulong CheckBit(ulong val, int pos)
        {
            return (ulong)((val >> pos) & 1ul);
        }

        // Shift Left
        public byte ShiftLeft(byte val, int pos)
        {
            return (byte)(val << pos);
        }

        public ushort ShiftLeft(ushort val, int pos)
        {
            return (ushort)(val << pos);
        }

        public uint ShiftLeft(uint val, int pos)
        {
            return (val << pos);
        }

        public ulong ShiftLeft(ulong val, int pos)
        {
            return (ulong)(val << pos);
        }

        // Shift Right
        public byte ShiftRight(byte val, int pos)
        {
            return (byte)(val >> pos);
        }

        public ushort ShiftRight(ushort val, int pos)
        {
            return (ushort)(val >> pos);
        }

        public uint ShiftRight(uint val, int pos)
        {
            return (val >> pos);
        }

        public ulong ShiftRight(ulong val, int pos)
        {
            return (ulong)(val >> pos);
        }

        /// <summary>
        /// A method to create a bit mask
        /// The bit mask is created by setting the bits from start to end to 1
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public uint CreateBitMask(int start, int end)
        {
            return (uint)(((1 << (end - start + 1)) - 1) << start);
        }

        /// <summary>
        /// To convert a number to a binary string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ToBinaryString(uint val)
        {
            StringBuilder builder = new StringBuilder();

            for(int i = sizeof(uint)-1; i >= 0; i--)
            {
                builder.Append(this.CheckBit(val, i) == 1u ? '1' : '0');
            }

            return builder.ToString();
        }

        /// <summary>
        /// To convert a number to a hexadecimal string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public string ToHexString(uint val)
        {
            return val.ToString("X");
        }

    }
}
