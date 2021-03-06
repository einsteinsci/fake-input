﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FakeInput
{
	public class NativeMethods
	{
		[DllImport("User32.dll", EntryPoint = "PostMessageA")]
		internal static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		[DllImport("User32.dll", SetLastError = true)]
		internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct INPUT
	{
		public uint Type;
		public MOUSEKEYBDHARDWAREINPUT Data;
	}

	/// <summary>
	/// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
		public HARDWAREINPUT Hardware;
		[FieldOffset(0)]
		public KEYBDINPUT Keyboard;
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct HARDWAREINPUT
	{
		public uint Msg;
		public ushort ParamL;
		public ushort ParamH;
	}

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct KEYBDINPUT
	{
		public ushort Vk;
		public ushort Scan;
		public uint Flags;
		public uint Time;
		public IntPtr ExtraInfo;
	}

	/// <summary>
	/// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct MOUSEINPUT
	{
		public int X;
		public int Y;
		public uint MouseData;
		public uint Flags;
		public uint Time;
		public IntPtr ExtraInfo;
	}
}
