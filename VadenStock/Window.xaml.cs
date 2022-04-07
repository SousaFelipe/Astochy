using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Runtime.InteropServices;

using VadenStock.Model;
using VadenStock.Controller;
using VadenStock.View.Adapters;
using VadenStock.View.Components;



namespace VadenStock
{
    public partial class MainWindow : Window
    {
		private const int WM_GETMINMAXINFO			= 0x0024;
		private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr handle, uint flags);

		[DllImport("user32.dll")]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);



		public MainWindow()
        {
            InitializeComponent();
        }



		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
			/*
			TabsAdapter adapter = new(_TabCategorias);
			adapter.Update(Categorias.Listar(), false);
			adapter.Build(c =>
            {
				TabCategoria firstTab = (TabCategoria)c;
				firstTab.IsChecked = true;
				return true;
            });
			*/
		}



		public static IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == WM_GETMINMAXINFO)
			{
				#pragma warning disable CS8605
                MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
				#pragma warning restore CS8605

                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

				if (monitor != IntPtr.Zero)
				{
					MONITORINFO monitorInfo = new();
					monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));

					GetMonitorInfo(monitor, ref monitorInfo);

					RECT rcWorkArea = monitorInfo.rcWork;
					RECT rcMonitorArea = monitorInfo.rcMonitor;

					mmi.ptMaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
					mmi.ptMaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
					mmi.ptMaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
					mmi.ptMaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);
				}

				Marshal.StructureToPtr(mmi, lParam, true);
			}

			return IntPtr.Zero;
		}



		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				this.Left = left;
				this.Top = top;
				this.Right = right;
				this.Bottom = bottom;
			}
		}



		[StructLayout(LayoutKind.Sequential)]
		public struct MONITORINFO
		{
			public int cbSize;
			public RECT rcMonitor;
			public RECT rcWork;
			public uint dwFlags;
		}



		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}
		}



		[StructLayout(LayoutKind.Sequential)]
		public struct MINMAXINFO
		{
			public POINT ptReserved;
			public POINT ptMaxSize;
			public POINT ptMaxPosition;
			public POINT ptMinTrackSize;
			public POINT ptMaxTrackSize;
		}



        private void ShutdownApplication(object sender, RoutedEventArgs e)
        {
			Application.Current.Shutdown();
        }
    }
}
