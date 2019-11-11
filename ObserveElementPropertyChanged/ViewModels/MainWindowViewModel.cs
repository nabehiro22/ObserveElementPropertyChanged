using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ObserveElementPropertyChanged.ViewModels
{
    public class MainWindowViewModel : BindableBase, INotifyPropertyChanged
	{
		public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("TEST Application");

		private CompositeDisposable Disposable { get; } = new CompositeDisposable();

		/// <summary>
		/// DataGridに表示する情報
		/// </summary>
		public ObservableCollection<People> Member { get; } = new ObservableCollection<People>();

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public MainWindowViewModel()
		{
			// とりあえずメンバーを追加
			Member.Add(new People { Name = "田中一郎", Age = 30, Height = 170 });
			Member.Add(new People { Name = "佐藤二郎", Age = 35, Height = 165 });

			Member.ObserveElementPropertyChanged().Subscribe(Changed).AddTo(Disposable);
		}

		/// <summary>
		/// デストラクタ
		/// </summary>
		~MainWindowViewModel()
		{
			Disposable.Dispose();
		}

		/// <summary>
		/// 内容に変更が発生した
		/// </summary>
		/// <param name="pair"></param>
		private void Changed(SenderEventArgsPair<People, PropertyChangedEventArgs> pair)
		{
			MessageBox.Show("Changed");
		}
	}

	public class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }

			field = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}

	/// <summary>
	/// メンバーの情報
	/// </summary>
	public class People : BindableBase
	{
		private string _Name;
		public string Name
		{
			get { return _Name; }
			set { SetProperty(ref _Name, value); }
		}

		private int _Age;
		public int Age
		{
			// get set シンプルな書き方
			get => _Age;
			set => SetProperty(ref _Age, value);
		}

		private int _Height;
		// get set がシンプルに書ければ1行でいいかも
		public int Height { get => _Height; set => SetProperty(ref _Height, value); }
	}
}
