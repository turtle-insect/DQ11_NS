using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonQuestXI.Cryptography;

namespace DQ11
{
	class SaveData
	{
		private static SaveData mThis;
		private String mFileName = null;
		private Byte[] mHeader = null;
		private Byte[] mBuffer = null;
		public uint Adventure { private get; set; } = 0;
		private const String mKey = "C5VbD9SJxe4FhK7wnWxy_LVSuHfbQjAUHBLxstRi3JBRc5eZVK6jQm9YGXDugs6J";

		private SaveData()
		{ }

		public static SaveData Instance()
		{
			if (mThis == null) mThis = new SaveData();
			return mThis;
		}

		public bool Open(String filename, bool force)
		{
			if (System.IO.File.Exists(filename) == false) return false;

			Byte[] tmp = System.IO.File.ReadAllBytes(filename);
			mHeader = new Byte[8];
			mBuffer = new Byte[tmp.Length - 8];
			Array.Copy(tmp, mHeader, mHeader.Length);
			Array.Copy(tmp, 8, mBuffer, 0, mBuffer.Length);
			DragonKey dKey = new DragonKey(Encoding.ASCII.GetBytes(mKey));
			dKey.Decrypt(mBuffer);

			if(force == false)
			{
				if(ReadNumber((uint)mBuffer.Length - 12, 4) != CalcCheckSum())
				{
					mBuffer = null;
					return false;
				}
			}

			mFileName = filename;
			Backup();
			return true;
		}

		public bool Save()
		{
			if (mFileName == null || mBuffer == null) return false;

			/*
			uint address = FindAddress("Y32_1F_01", (uint)mBuffer.Length - 100)[0];
			address += 26;
			DeleteBlock(address, (uint)mBuffer.Length - address);
			uint mod = (uint)mBuffer.Length % 16;
			if (mod > 0) mod = 16 - mod;
			AppendBlock(address, mod + 16);

			WriteNumber(0x2D, 4, address);
			uint body = 170 + ReadNumber(0x57, 4);
			WriteNumber(body, 4, address - body - 16);
			WriteNumber((uint)mBuffer.Length - 16, 4, address);
			*/

			WriteNumber((uint)mBuffer.Length - 12, 4, CalcCheckSum());
			Byte[] enc = new Byte[mBuffer.Length];
			Array.Copy(mBuffer, enc, enc.Length);
			DragonKey dKey = new DragonKey(Encoding.ASCII.GetBytes(mKey));
			dKey.Encrypt(enc);

			Byte[] tmp = new byte[enc.Length + mHeader.Length];
			Array.Copy(mHeader, tmp, mHeader.Length);
			Array.Copy(enc, 0, tmp, mHeader.Length, enc.Length);
			System.IO.File.WriteAllBytes(mFileName, tmp);

			return true;
		}

		public bool SaveAs(String filename)
		{
			if (mFileName == null || mBuffer == null) return false;
			mFileName = filename;
			return Save();
		}

		public void Import(String filename)
		{
			if (mFileName == null) return;

			mBuffer = System.IO.File.ReadAllBytes(filename);
		}

		public void Export(String filename)
		{
			System.IO.File.WriteAllBytes(filename, mBuffer);
		}

		public uint ReadNumber(uint address, uint size)
		{
			if (mBuffer == null) return 0;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return 0;
			uint result = 0;
			for (int i = 0; i < size; i++)
			{
				result += (uint)(mBuffer[address + i]) << (i * 8);
			}
			return result;
		}

		public Byte[] ReadValue(uint address, uint size)
		{
			Byte[] result = new Byte[size];
			if (mBuffer == null) return result;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return result;
			for (int i = 0; i < size; i++)
			{
				result[i] = mBuffer[address + i];
			}
			return result;
		}

		// 0 to 7.
		public bool ReadBit(uint address, uint bit)
		{
			if (bit < 0) return false;
			if (bit > 7) return false;
			if (mBuffer == null) return false;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return false;
			Byte mask = (Byte)(1 << (int)bit);
			Byte result = (Byte)(mBuffer[address] & mask);
			return result != 0;
		}

		public String ReadText(uint address, uint size, System.Text.Encoding encode)
		{
			if (mBuffer == null) return "";
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return "";

			Byte[] tmp = new Byte[size];
			for (uint i = 0; i < size; i++)
			{
				if (mBuffer[address + i] == 0) break;
				tmp[i] = mBuffer[address + i];
			}
			return encode.GetString(tmp).Trim('\0');
		}

		public void WriteNumber(uint address, uint size, uint value)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = (Byte)(value & 0xFF);
				value >>= 8;
			}
		}

		// 0 to 7.
		public void WriteBit(uint address, uint bit, bool value)
		{
			if (bit < 0) return;
			if (bit > 7) return;
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address > mBuffer.Length) return;
			Byte mask = (Byte)(1 << (int)bit);
			if (value) mBuffer[address] = (Byte)(mBuffer[address] | mask);
			else mBuffer[address] = (Byte)(mBuffer[address] & ~mask);
		}

		public void WriteText(uint address, uint size, String value, System.Text.Encoding encode)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			Byte[] tmp = encode.GetBytes(value);
			Array.Resize(ref tmp, (int)size);
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = tmp[i];
			}
		}

		public void WriteValue(uint address, Byte[] buffer)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + buffer.Length > mBuffer.Length) return;

			for (uint i = 0; i < buffer.Length; i++)
			{
				mBuffer[address + i] = buffer[i];
			}
		}

		public void AppendBlock(uint address, uint size)
		{
			if (mBuffer == null || size == 0) return;
			address = CalcAddress(address);

			Byte[] tmp = new Byte[mBuffer.Length + size];
			Array.Copy(mBuffer, tmp, address);
			Array.Copy(mBuffer, address, tmp, address + size, mBuffer.Length - address);
			mBuffer = tmp;
		}

		public void DeleteBlock(uint address, uint size)
		{
			if (mBuffer == null || size == 0) return;
			address = CalcAddress(address);

			Byte[] tmp = new Byte[mBuffer.Length - size];
			Array.Copy(mBuffer, tmp, address);
			Array.Copy(mBuffer, address + size, tmp, address, mBuffer.Length - address - size);
			mBuffer = tmp;
		}

		public void Fill(uint address, uint size, Byte number)
		{
			if (mBuffer == null) return;
			address = CalcAddress(address);
			if (address + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[address + i] = number;
			}
		}

		public void Copy(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				mBuffer[to + i] = mBuffer[from + i];
			}
		}

		public void Swap(uint from, uint to, uint size)
		{
			if (mBuffer == null) return;
			from = CalcAddress(from);
			to = CalcAddress(to);
			if (from + size > mBuffer.Length) return;
			if (to + size > mBuffer.Length) return;
			for (uint i = 0; i < size; i++)
			{
				Byte tmp = mBuffer[to + i];
				mBuffer[to + i] = mBuffer[from + i];
				mBuffer[from + i] = tmp;
			}
		}

		public List<uint> FindAddress(String name, uint index)
		{
			List<uint> result = new List<uint>();
			if (mBuffer == null) return result;

			for (; index < mBuffer.Length; index++)
			{
				if (mBuffer[index] != name[0]) continue;

				int len = 1;
				for (; len < name.Length; len++)
				{
					if (mBuffer[index + len] != name[len]) break;
				}
				if (len >= name.Length) result.Add(index);
				index += (uint)len;
			}
			return result;
		}

		private uint CalcCheckSum()
		{
			uint size = ReadNumber((uint)mBuffer.Length - 16, 4);
			return (uint)DragonKey.GetChecksum(mBuffer, (int)size);
		}

		private uint CalcAddress(uint address)
		{
			return address;
		}

		private void Backup()
		{
			DateTime now = DateTime.Now;
			String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			path = System.IO.Path.Combine(path, "backup");
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			path = System.IO.Path.Combine(path,
				String.Format("{0:0000}-{1:00}-{2:00} {3:00}-{4:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute));
			System.IO.File.Copy(mFileName, path, true);
		}
	}
}
