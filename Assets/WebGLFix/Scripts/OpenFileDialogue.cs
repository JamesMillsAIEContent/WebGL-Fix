using System;
using System.Runtime.InteropServices;

namespace WebGLFix
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenFileInfo
	{
		public int structSize = 0;
		public IntPtr dlgOwner = IntPtr.Zero;
		public IntPtr instance = IntPtr.Zero;
		public string filter = null;
		public string customFilter = null;
		public int maxCustomFilter = 0;
		public int filterIndex = 0;
		public string file = null;
		public int maxFile = 0;
		public string fileTitle = null;
		public int maxFileTitle = 0;
		public string initialDir = null;
		public string title = null;
		public int flags = 0;
		public short fileOffset = 0;
		public short fileExtension = 0;
		public string defExt = null;
		public IntPtr customData = IntPtr.Zero;
		public IntPtr hook = IntPtr.Zero;
		public string templateName = null;
		public IntPtr reservedPtr = IntPtr.Zero;
		public int reservedInt = 0;
		public int flagsEx = 0;
	}

	public class OpenFileDialogue
	{
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
		public static extern bool GetOpenFileName([In, Out] OpenFileInfo _ofn);

		public static bool GetOpenFileName1([In, Out] OpenFileInfo _ofn) => GetOpenFileName(_ofn);
	}
}