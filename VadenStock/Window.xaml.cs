using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Threading;

using VadenStock.View.Dialogs;



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



		private Func<bool>? OnCloseDialogCallback { get; set; }



		public MainWindow()
        {
            InitializeComponent();
        }



		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
		}



		public void DisplayAlert(AlertDialog alert)
        {
			_AlertContainer.Children.Add(alert);

			Application.Current.Dispatcher.BeginInvoke(
					DispatcherPriority.Background,
					new Action(async () =>
                    {
						await alert.Down();
						CloseAlert(alert);
					})
				);
		}



		public void CloseAlert(AlertDialog alert, bool self = false)
        {
			if (_AlertContainer.Children.Contains(alert))
            {
				Thread thread = new(() =>
				{
					if (!self)
						Thread.Sleep(5000);

					Application.Current.Dispatcher.BeginInvoke(
							DispatcherPriority.Background,
							new Action(async () =>
							{
								await alert.Up();
								_AlertContainer.Children.Remove(alert);
							})
						);
				});

				thread.Start();
			}
		}



		public void DisplayDialog(UIElement element, Func<bool>? onCloseDialogCallback = null)
        {
			_BorderShadow.Visibility = Visibility.Visible;
			_DialogContainer.Children.Add(element);

			OnCloseDialogCallback = onCloseDialogCallback;
		}



		public void CloseDialog(UIElement element)
        {
			OnCloseDialogCallback?.Invoke();

			_DialogContainer.Children.Remove(element);
			_DialogContainer.Children.Clear();
			_BorderShadow.Visibility = Visibility.Collapsed;
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



		private void ClearMainTextBoxSearch(object sender, MouseButtonEventArgs e)
		{
			_TextBoxMainSearch.Clear();
		}



		private void ShutdownApplication(object sender, RoutedEventArgs e)
        {
			Application.Current.Shutdown();
        }
    }
}
