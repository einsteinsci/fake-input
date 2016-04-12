using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FakeInput
{
	public static class InputFaker
	{
		public const int WM_KEYDOWN = 0x0100;
		private const int WM_SETTEXT = 0x000c;

		internal static readonly IntPtr HWnd;

		static InputFaker()
		{
			HWnd = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
		}

		private static KeyCode? _getTypableSymbolCode(char c)
		{
			switch (c)
			{
			case '\b':
				return KeyCode.BACKSPACE;
			case '\t':
				return KeyCode.TAB;
			case '\n':
			case '\f':
			case '\r':
				return KeyCode.ENTER;
			case ' ':
				return KeyCode.SPACE_BAR;
			case ';':
				return KeyCode.OEM_1; // ; :
			case '=':
				return KeyCode.OEM_PLUS; // = +
			case ',':
				return KeyCode.OEM_COMMA; // , <
			case '-':
				return KeyCode.OEM_MINUS; // - _
			case '.':
				return KeyCode.OEM_PERIOD; // . >
			case '/':
				return KeyCode.OEM_2; // / ?
			case '`':
				return KeyCode.OEM_3; // ` ~
			case '[':
				return KeyCode.OEM_4; // [ {
			case '\\':
				return KeyCode.OEM_5; // \ |
			case ']':
				return KeyCode.OEM_6; // ] }
			case '\'':
				return KeyCode.OEM_7; // ' "
			default:
				return null;
			}
		}

		private static CharacterInput _getSymbolInput(char c)
		{
			switch (c)
			{
			case '\b':
			case '\t':
			case '\n':
			case '\f':
			case '\r':
			case ' ':
			case ';':
			case '=':
			case ',':
			case '-':
			case '.':
			case '`':
			case '[':
			case '\\':
			case ']':
			case '\'':
				return new CharacterInput(_getTypableSymbolCode(c).Value);
			case ':':
				return new CharacterInput(KeyCode.OEM_1, KeyCode.LSHIFT);
			case '+':
				return new CharacterInput(KeyCode.OEM_PLUS, KeyCode.LSHIFT);
			case '<':
				return new CharacterInput(KeyCode.OEM_COMMA, KeyCode.LSHIFT);
			case '_':
				return new CharacterInput(KeyCode.OEM_MINUS, KeyCode.LSHIFT);
			case '>':
				return new CharacterInput(KeyCode.OEM_PERIOD, KeyCode.LSHIFT);
			case '?':
				return new CharacterInput(KeyCode.OEM_2, KeyCode.LSHIFT);
			case '~':
				return new CharacterInput(KeyCode.OEM_3, KeyCode.LSHIFT);
			case '{':
				return new CharacterInput(KeyCode.OEM_4, KeyCode.LSHIFT);
			case '|':
				return new CharacterInput(KeyCode.OEM_5, KeyCode.LSHIFT);
			case '}':
				return new CharacterInput(KeyCode.OEM_6, KeyCode.LSHIFT);
			case '"':
				return new CharacterInput(KeyCode.OEM_7, KeyCode.LSHIFT);
			default:
				return null;
			}
		}

		private static CharacterInput _getInputSet(char c)
		{
			if (c.IsCapital())
			{
				return new CharacterInput((KeyCode)c, KeyCode.LSHIFT);
			}

			if (c.IsAlphanumeric())
			{
				return new CharacterInput((KeyCode)c.ToUpper());
			}

			return _getSymbolInput(c);
		}

		public static void SendCodeByPost(int code)
		{
			bool result = NativeMethods.PostMessage(HWnd, WM_KEYDOWN, code, 0x00);

			if (!result)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		public static void SendKeyByPost(KeyCode key)
		{
			SendCodeByPost((int)key);
		}

		public static bool SendChars(params char[] text)
		{
			return SendCharsSpooky(0, text);
		}
		public static bool SendCharsSpooky(int msDelay, params char[] text)
		{
			CharacterInput[] codes = new CharacterInput[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				CharacterInput n = _getInputSet(text[i]);
				if (n == null)
				{
					return false; // unsendable character
				}

				codes[i] = n;
			}

			foreach (CharacterInput ci in codes)
			{
				Thread.Sleep(msDelay);
				SendInput(ci);
			}
			return true;
		}

		public static bool SendString(string s, int spookyDelay = 0)
		{
			if (spookyDelay == 0)
			{
				return SendChars(s.ToArray());
			}
			else
			{
				return SendCharsSpooky(spookyDelay, s.ToArray());
			}
		}

		public static void SendEnter()
		{
			SendKeyByPost(KeyCode.ENTER);
		}

		public static void SendInput(CharacterInput input)
		{
			if (input.HasModifier)
			{
				SendKeyDown(input.Modifier.Value);
				SendKeyPress(input.MainKey);
				SendKeyUp(input.Modifier.Value);
			}
			else
			{
				SendKeyPress(input.MainKey);
			}
		}

		/// <summary>
		/// Simulate key press
		/// </summary>
		/// <param name="keyCode"></param>
		public static void SendKeyPress(KeyCode keyCode)
		{
			INPUT input = new INPUT { Type = 1 };
			input.Data.Keyboard = new KEYBDINPUT()
			{
				Vk = (ushort)keyCode,
				Scan = 0,
				Flags = 0,
				Time = 0,
				ExtraInfo = IntPtr.Zero,
			};

			INPUT input2 = new INPUT { Type = 1 };
			input2.Data.Keyboard = new KEYBDINPUT()
			{
				Vk = (ushort)keyCode,
				Scan = 0,
				Flags = 2,
				Time = 0,
				ExtraInfo = IntPtr.Zero
			};

			INPUT[] inputs = new INPUT[] { input, input2 };

			if (NativeMethods.SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// Send a key down and hold it down until sendkeyup method is called
		/// </summary>
		/// <param name="keyCode"></param>
		public static void SendKeyDown(KeyCode keyCode)
		{
			INPUT input = new INPUT { Type = 1 };
			input.Data.Keyboard = new KEYBDINPUT();
			input.Data.Keyboard.Vk = (ushort)keyCode;
			input.Data.Keyboard.Scan = 0;
			input.Data.Keyboard.Flags = 0;
			input.Data.Keyboard.Time = 0;
			input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			INPUT[] inputs = new INPUT[] { input };

			if (NativeMethods.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// Release a key that is being hold down
		/// </summary>
		/// <param name="keyCode"></param>
		public static void SendKeyUp(KeyCode keyCode)
		{
			INPUT input = new INPUT { Type = 1 };
			input.Data.Keyboard = new KEYBDINPUT();
			input.Data.Keyboard.Vk = (ushort)keyCode;
			input.Data.Keyboard.Scan = 0;
			input.Data.Keyboard.Flags = 2;
			input.Data.Keyboard.Time = 0;
			input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			INPUT[] inputs = new INPUT[] { input };

			if (NativeMethods.SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		public static void SetWindowTitle(string title)
		{
			NativeMethods.SendMessage(HWnd, WM_SETTEXT, 0, title);
		}
	}
}
