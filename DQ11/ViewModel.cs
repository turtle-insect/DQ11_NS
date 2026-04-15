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

			// This really needs to be implemented more properly.
			// Since it was just a quick fix to begin with, I’ll settle for this for now.
			var baseIndex = SaveData.Instance().FindAddress("DLC_00", 0);
			if (baseIndex.Count == 0) return;
			uint address = baseIndex[0] - 8;
			uint dlcCount = SaveData.Instance().ReadNumber(address, 4);
			address += 4 + (4 + 7 + 4) * dlcCount;

			for(int i = 0; i < Party.Count; i++)
			{
				address = Party[i].Inventory.Create(address);
				var value = SaveData.Instance().ReadNumber(address, 1);
				if (value == 0)
				{
					address += 4;
				}
				else
				{
					// dummy?
					// back up item?
					// if this sequence read skip bag.
					var bag = new Bag();
					address = bag.Create(address);
				}
			}

			Items.Create(address);
		}
	}
}
