using System;

namespace DQ11
{
	class KeyNameInfo : ILineAnalysis
	{
		public String Key { get; private set; }
		public String Name { get; private set; }


		public virtual bool Line(String[] oneLine)
		{
			if (oneLine.Length != 2) return false;
			Key = oneLine[0];
			Name = oneLine[1];
			return true;
		}
	}
}