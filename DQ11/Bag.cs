using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DQ11
{
	class Bag
	{
		public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();

		private uint mCounterAddress;
		private uint mEndAddress;

		public uint Create(uint address)
		{
			mCounterAddress = address;

			uint count = SaveData.Instance().ReadNumber(address, 4);
			address += 4;
			for (uint i = 0; i < count; i++)
			{
				Items.Add(new Item(address));
				address += SaveData.Instance().ReadNumber(address, 4) + 13;
			}
			mEndAddress = address;
			return address;
		}

		public void Update()
		{
			SaveData.Instance().WriteNumber(mCounterAddress, 4, (uint)Items.Count);
		}

		public void Append(String id)
		{
			SaveData.Instance().AppendBlock(mEndAddress, (uint)id.Length + 14);
			SaveData.Instance().WriteNumber(mEndAddress, 4, (uint)id.Length + 1);
			SaveData.Instance().WriteText(mEndAddress + 4, (uint)id.Length, id, System.Text.Encoding.ASCII);
			SaveData.Instance().WriteNumber(mEndAddress + 4 + (uint)id.Length + 1, 4, 1);
			SaveData.Instance().WriteNumber(mEndAddress + 8 + (uint)id.Length + 1, 4, 99);
			SaveData.Instance().WriteNumber(mEndAddress + 12 + (uint)id.Length + 1, 1, 0);

			Items.Add(new Item(mEndAddress));
			mEndAddress += (uint)id.Length + 14;
		}

		public void Delete(int index)
		{
			if (Items.Count <= index) return;
			Items[index].Delete();
		}
	}
}
