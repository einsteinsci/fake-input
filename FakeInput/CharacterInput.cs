using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeInput
{
	public class CharacterInput
	{
		public KeyCode? Modifier
		{ get; private set; }

		public KeyCode MainKey
		{ get; private set; }

		public bool HasModifier => Modifier != null;

		public CharacterInput(KeyCode main)
		{
			MainKey = main;
		}
		public CharacterInput(KeyCode main, KeyCode modifier) : this(main)
		{
			// only accept valid inputs
			if (modifier == KeyCode.LSHIFT || modifier == KeyCode.LCONTROL || modifier == KeyCode.LALT ||
				modifier == KeyCode.RSHIFT || modifier == KeyCode.RCONTROL || modifier == KeyCode.RALT)
			{
				Modifier = modifier;
			}
		}

		public override string ToString()
		{
			if (HasModifier)
			{
				return Modifier.ToString() + " + " + MainKey.ToString().Replace("KEY_", "");
			}

			return MainKey.ToString().Replace("KEY_", "");
		}
	}
}
