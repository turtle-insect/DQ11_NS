using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ11
{
	class Item
	{
		private readonly uint mAddress;

		public Item(uint address)
		{
			mAddress = address;
		}

		public void Delete()
		{
			uint size = SaveData.Instance().ReadNumber(mAddress, 4) + 13;
			SaveData.Instance().DeleteBlock(mAddress, size);
		}

		public String Name
		{
			get
			{
				uint size = SaveData.Instance().ReadNumber(mAddress, 4);
				return SaveData.Instance().ReadText(mAddress + 4, size, System.Text.Encoding.ASCII);
			}

			set
			{
				uint size = SaveData.Instance().ReadNumber(mAddress, 4);
				if (value.Length + 1 > size) SaveData.Instance().AppendBlock(mAddress + 4, (uint)value.Length - size);
				else if (value.Length + 1 < size) SaveData.Instance().DeleteBlock(mAddress + 4, size - (uint)value.Length);
				SaveData.Instance().WriteNumber(mAddress, 4, (uint)value.Length + 1);
				SaveData.Instance().WriteText(mAddress + 4, (uint)value.Length, value, System.Text.Encoding.ASCII);
			}
		}

		public uint Count
		{
			get
			{
				uint address = CalcAddress();
				return SaveData.Instance().ReadNumber(address, 4);
			}
			set
			{
				uint address = CalcAddress();
				Util.WriteNumber(address, 4, value, 99, 0);
			}
		}

		public uint Max
		{
			get
			{
				uint address = CalcAddress() + 4;
				return SaveData.Instance().ReadNumber(address, 4);
			}
		}

		public uint BBB
		{
			get
			{
				uint address = CalcAddress() + 8;
				return SaveData.Instance().ReadNumber(address, 1);
			}
		}

		private uint CalcAddress()
		{
			// ItemID Length + ItemID Name.
			return mAddress + SaveData.Instance().ReadNumber(mAddress, 4) + 4;
		}
	}
}
