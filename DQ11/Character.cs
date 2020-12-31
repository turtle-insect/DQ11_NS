using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ11
{
	class Character
	{
		private readonly uint mAddress;
		public Bag Inventory { get; set; } = new Bag();
		public String Name { get; private set; }

		public Character(uint address)
		{
			Name = SaveData.Instance().ReadText(address + 28, 6 * 2, System.Text.Encoding.Unicode);
			mAddress = address + 28 + (uint)Name.Length * 2;
		}

		public uint Lv
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 8, 4); }
			set { Util.WriteNumber(mAddress + 8, 4, value, 99, 1); }
		}

		public uint Exp
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 64, 4); }
			set { Util.WriteNumber(mAddress + 64, 4, value, 9999999, 0); }
		}

		public uint HP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 12, 4); }
			set { Util.WriteNumber(mAddress + 12, 4, value, 999, 0); }
		}

		public uint MaxHP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 48, 4); }
			set { Util.WriteNumber(mAddress + 48, 4, value, 999, 1); }
		}

		public uint MP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 16, 4); }
			set { Util.WriteNumber(mAddress + 16, 4, value, 999, 0); }
		}

		public uint MaxMP
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 52, 4); }
			set { Util.WriteNumber(mAddress + 52, 4, value, 999, 0); }
		}

		public uint Attack
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 20, 4); }
			set { Util.WriteNumber(mAddress + 20, 4, value, 999, 0); }
		}

		public uint Defense
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 24, 4); }
			set { Util.WriteNumber(mAddress + 24, 4, value, 999, 0); }
		}

		public uint AttackMagic
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 40, 4); }
			set { Util.WriteNumber(mAddress + 40, 4, value, 999, 0); }
		}

		public uint RecoveryMagic
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 44, 4); }
			set { Util.WriteNumber(mAddress + 44, 4, value, 999, 0); }
		}

		public uint Speed
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 28, 4); }
			set { Util.WriteNumber(mAddress + 28, 4, value, 999, 0); }
		}

		public uint Skillful
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 32, 4); }
			set { Util.WriteNumber(mAddress + 32, 4, value, 999, 0); }
		}

		public uint Charm
		{
			get { return SaveData.Instance().ReadNumber(mAddress + 36, 4); }
			set { Util.WriteNumber(mAddress + 36, 4, value, 999, 0); }
		}
	}
}
