using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shared
{
    public class Packet
    {
        //AUTHOR: Mostly taken from the Networking Assignment, with some slight adjustments by Leo



        //To read packets with
        private BinaryReader _reader;

        //To write packets with
        private BinaryWriter _writer;

        //Initialize
        public Packet()
        {
            //Can write a memory stream, to be read later by a reader 
            _writer = new BinaryWriter(new MemoryStream());
        }

        public Packet(byte[] pSource)
        {
            //Can retrieve from memory stream, to convert to data 
            _reader = new BinaryReader(new MemoryStream(pSource));
        }

        //Write things
        public void Write(int pInt) { _writer.Write(pInt); }
        public void Write(string pString) { _writer.Write(pString); }
        public void Write(bool pBool) { _writer.Write(pBool); }
        public void Write(float pFloat) { _writer.Write( (double)pFloat); } //convert to double (because floats arent read)
        public void Write(ASerializable pASerializable)
        {
            //retrieves type from server, while making sure namespace is correct
            Write(pASerializable.GetType().FullName);

            //Serialize it too 
            pASerializable.Serialize(this);
        }

        //Read things
        public int ReadInt() { return _reader.ReadInt32(); }
        public string ReadString() { return _reader.ReadString(); }
        public bool ReadBool() { return _reader.ReadBoolean(); }
        public float ReadFloat() { return (float)_reader.ReadDouble(); } //convert back to float from double

        public ASerializable ReadObject()
        {
            //get the classname from the stream first
            Type type = Type.GetType(ReadString());

            //I actually have no idea what this row does, maybe ask later to teachers
            //Trying to create an object of the same type and then deserialize the values of this packet into the new object :)
            ASerializable obj = (ASerializable)Activator.CreateInstance(type);

            //deserialize object
            obj.Deserialize(this);
            return obj;
        }

        public byte[] GetBytes()
        {
            //in case we need the byte stream
            return ((MemoryStream)_writer.BaseStream).ToArray();
        }

        public bool isReaderEmpty()
        {
            if (_reader.BaseStream.Position == _reader.BaseStream.Length)
            {
                return true;
            }
            return false;
        }
    }
}
