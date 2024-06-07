#nullable disable
using System.ComponentModel;
using R3;

namespace MauiApp6
{
    public class Option : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsChecked)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Monkey
    {
        public BindableReactiveProperty<string> Name { get; }
        public BindableReactiveProperty<int> Age { get; }

        public BindableReactiveProperty<int> SelectIndex { get; }
        public BindableReactiveProperty<bool> ShowOptions { get; }

        public BindableReactiveProperty<List<Option>> Options { get; }

        public Monkey(string name, int age, List<Option> options, int selectedIndex = -1)
        {
            Name = new BindableReactiveProperty<string>(name);
            Age = new BindableReactiveProperty<int>(age);
            SelectIndex = new BindableReactiveProperty<int>(selectedIndex);
            ShowOptions = SelectIndex.Select(x => x == 1).ToBindableReactiveProperty();
            Options = new BindableReactiveProperty<List<Option>>(options);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Monkey)obj;
            return Name == other.Name && Age == other.Age;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Age.GetHashCode();
        }
    }

    public class MainPageViewModel
    {
        public BindableReactiveProperty<int> SelectIndex { get; }
        public BindableReactiveProperty<bool> ShowOptions { get; }
        public BindableReactiveProperty<List<Option>> Options { get; }

        public BindableReactiveProperty<List<Monkey>> MonkeyList { get; }
        public BindableReactiveProperty<List<Monkey>> MonkeyList2 { get; }

        public MainPageViewModel()
        {
            MonkeyList = new BindableReactiveProperty<List<Monkey>>();
            MonkeyList2 = new BindableReactiveProperty<List<Monkey>>();

            SelectIndex = new BindableReactiveProperty<int>(-1);
            ShowOptions = SelectIndex.Select(x => x == 1).ToBindableReactiveProperty();
            Options = new BindableReactiveProperty<List<Option>>(new List<Option>
            {
                new Option { Name = $"A1" },
                new Option { Name = $"A2" },
                new Option { Name = $"A3" },
            });
        }
    }

    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            var viewModel = new MainPageViewModel();
            viewModel.MonkeyList.Value = new List<Monkey>
            {
                new Monkey("Tom",1, GetOptions(2)),
                new Monkey("Kevi",2,GetOptions(2),1),
                new Monkey("Jone1",3,GetOptions(2)),
                new Monkey("Jone2",3,GetOptions(4)),
                new Monkey("Jone3",3,GetOptions(6)),
                new Monkey("Jone4",3,GetOptions(8)),
            };

            viewModel.MonkeyList2.Value = new List<Monkey>
            {
                new Monkey("Tom-1",1, GetOptions(2)),
                new Monkey("Kevi-1",2,GetOptions(2),1),
                new Monkey("Jone1-1",3,GetOptions(2)),
                new Monkey("Jone2-1",3,GetOptions(4)),
                new Monkey("Jone3-1",3,GetOptions(6)),
                new Monkey("Jone4-1",3,GetOptions(8)),
            };

            BindingContext = viewModel;
        }

        private List<Option> GetOptions(int count)
        {
            var result = new List<Option>();
            for (int i = 1; i <= count; i++)
            {
                result.Add(new Option { Name = $"A{i}" });
            }

            return result;
        }
    }
}
