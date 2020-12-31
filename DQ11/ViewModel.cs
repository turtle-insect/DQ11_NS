﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DQ11
{
	class ViewModel
	{
		public ObservableCollection<Character> Party { get; set; } = new ObservableCollection<Character>();
		public Bag Items { get; set; } = new Bag();

		public ViewModel()
		{
			foreach(var item in SaveData.Instance().FindAddress("JackFriendGameCharacter", 0))
			{
				Party.Add(new Character(item));
			}

			var itemIndex = SaveData.Instance().FindAddress("DLC_07", 0);
			if (itemIndex.Count == 0) return;
			uint address = itemIndex[0] + 11;

			for(int i = 0; i < Party.Count; i++)
			{
				address = Party[i].Inventory.Create(address);
				address += 4;
			}

			Items.Create(address);
		}
	}
}
