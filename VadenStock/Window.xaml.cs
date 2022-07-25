using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;

using Newtonsoft.Json.Linq;

using VadenStock.Core.Http;
using VadenStock.Http;

using VadenStock.Model;
using VadenStock.Model.Types;

using VadenStock.Tools;

using VadenStock.View.Components.Organisms;
using VadenStock.View.Dialogs;
using VadenStock.View.Models;



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

			Loaded += delegate
			{
				_InputMainSearch.OnSearch((result) => InputMainSearch_Changed(result));
			};
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
						Thread.Sleep(6000);

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
			_DialogContainer.Children.Remove(element);
			_DialogContainer.Children.Clear();
			_BorderShadow.Visibility = Visibility.Collapsed;

			OnCloseDialogCallback?.Invoke();
		}



		private void CallCurrentFocusedElement(string method, string action, string property)
		{
			IInputElement element = FocusManager.GetFocusedElement(this);

			if (element != null && element.GetType() != null)
			{
				object? val;

				foreach (PropertyInfo pi in element.GetType().GetProperties())
				{
					if (pi.Name == property)
					{
						val = pi.GetValue(element);

						if (val != null && ((bool)val) == true)
						{
							MethodInfo? mi = element.GetType().GetMethod(method);
							mi?.Invoke(element, new object[] { action });
						}
					}
				}
			}
		}



		private void SearchWhenItsNumber(string search)
        {
			Response response;
			List<ItemType> itens;

			Application.Current.Dispatcher.Invoke(async () =>
			{
				response = await Cliente.Conn
					.Where("cnpj_cpf", "LE", search)
					.Get();

				itens = Item.Model
					.Where("codigo", "LIKE", search)
					.Select();
			});
		}



		private async void SearchWhenItsText(string search)
        {
			Response response = await Cliente.Conn.Where("razao", "L", search).Where("ativo", "S").Get(10);
			List<Cliente>? clientes = response.Registros.ToObject<List<Cliente>>();

			if (clientes != null)
			{
				_StackClientesResultContainer.Visibility = Visibility.Visible;
				_GridLoadingSearch.Visibility = Visibility.Collapsed;
				_StackClientesResult.Children.Clear();

				foreach (Cliente cliente in clientes)
					_StackClientesResult.Children.Add(new ClienteItem(cliente));
			}
		}



		private void SearchWhenNotNumberOrText(string search)
        {
			string clean = search.Replace(":", "");

			List<ItemType> itens = Item.Model
				.Where("mac", "LIKE", clean)
				.Select();

			if (itens.Count > 0)
			{
				_StackProdutosResultContainer.Visibility = Visibility.Visible;
				_GridLoadingSearch.Visibility = Visibility.Collapsed;
				_StackProdutosResult.Children.Clear();

				foreach (ItemType item in itens)
					_StackProdutosResult.Children.Add(new ItemItem(item));
            }
		}



		private void InputMainSearch_Changed(string result)
		{
			_BorderSeach.Visibility = Visibility.Visible;
			_GridLoadingSearch.Visibility = Visibility.Visible;

			if (!string.IsNullOrEmpty(result) && result.Length >= 3)
            {
				if (Str.IsNumber(result))
					SearchWhenItsNumber(result);

				else if (Str.IsText(result))
					SearchWhenItsText(result);

				else
					SearchWhenNotNumberOrText(result);
			}
			else
				_BorderSeach.Visibility = Visibility.Collapsed;
		}



		private void ImageClearSearch_Click(object sender, MouseButtonEventArgs e)
		{
			_InputMainSearch.Clear();
			_BorderSeach.Visibility = Visibility.Collapsed;
			_GridLoadingSearch.Visibility = Visibility.Visible;
		}



		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Delete:
					CallCurrentFocusedElement("FireControlAction", "Delete", "IsSelected");
					break;
			}
		}



		private void ShutdownApplication(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}



        #region Window Control
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
        #endregion
    }
}
