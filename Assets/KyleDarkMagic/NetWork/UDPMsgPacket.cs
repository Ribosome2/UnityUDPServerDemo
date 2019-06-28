using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace KyleDarkMagic
{
    /// <summary>
    /// 协议抽象基类
    /// </summary>
    public abstract class ByteReaderWriterBase  {

        const int INT32_LENGTH=4;
        private const int SHORT16_LEN = 2;
    
        /// <summary>
        /// 这个协议的协议号
        /// </summary>
        public int protoCode;

        /// <summary>
        /// 要发送的字节链表
        /// </summary>
        protected  List<byte> listBytes=new List<byte>();

        protected byte[] mBytes;
        /// <summary>
        /// 读或者写的时候的字节偏移
        /// </summary>
        int mByteOffset;

        protected void WriteUShort(ushort intValue)
        {
            byte[] bs = BitConverter.GetBytes(intValue);
            listBytes.AddRange(bs);
            mByteOffset += SHORT16_LEN;
        }




        protected void ReadUShort(out ushort outputValue)
        {
            outputValue = BitConverter.ToUInt16(mBytes, mByteOffset);
            mByteOffset += SHORT16_LEN;
        }

        protected void WriteInt(int intValue )
        {
            byte[] bs = BitConverter.GetBytes(intValue);
            listBytes.AddRange(bs);
            mByteOffset += INT32_LENGTH;
        }




        protected void ReadInt32(out int outputValue)
        {
            outputValue = BitConverter.ToInt32(mBytes, mByteOffset);
            mByteOffset += INT32_LENGTH;
        }


        /// <summary>
        /// 写字符传，先写一个数来标记长度，后面的字节才是真正的字符串内容
        /// </summary>
        /// <param name="str"></param>
        protected void  WriteString(string str )
        {
            ushort byteCount=(ushort)Encoding.UTF8.GetByteCount(str);
            WriteUShort(byteCount);
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            listBytes.AddRange(strBytes);

        }

        protected void ReadString(out string str)
        {
            ushort len;
            ReadUShort(out len);
            str = Encoding.UTF8.GetString(mBytes, mByteOffset, len);
            mByteOffset += len;
        }
    }
}
