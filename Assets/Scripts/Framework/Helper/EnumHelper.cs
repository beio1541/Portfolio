using System;

namespace Framework.Common.Helper
{
    public static partial class EnumHelper
    {
        struct EnumValue<T, E> where T : unmanaged where E : unmanaged, Enum
        {
            public T iValue;
            public E eValue;
        }
        #region ToValue
        public unsafe static T ToValue<T, E>(this E value) where T : unmanaged where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<T, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static byte ToByte<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<byte, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static sbyte ToSByte<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<sbyte, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static short ToShort<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<short, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static ushort ToUShort<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<ushort, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static int ToInt<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<int, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static uint ToUInt<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<uint, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static long ToLong<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<long, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        public unsafe static ulong ToULong<E>(this E value) where E : unmanaged, Enum
        {
            var enumValue = new EnumValue<ulong, E>() { eValue = value };
            return *((&enumValue.iValue) + 1);
        }
        #endregion// ToValue
        #region ToEnum
        public unsafe static E ToEnum<T, E>(this T value) where T : unmanaged where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<T, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this byte value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<byte, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this sbyte value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<sbyte, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this short value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<short, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this ushort value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<ushort, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this int value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<int, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this uint value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<uint, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this long value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<long, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        public unsafe static E ToEnum<E>(this ulong value) where E : unmanaged, Enum
        {
            var enumValue = default(EnumValue<ulong, E>);
            *((&enumValue.iValue) + 1) = value;
            return enumValue.eValue;
        }
        #endregion// ToEnum
    }
}