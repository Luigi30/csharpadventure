using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;

namespace TextAdventure
{
    class TextAdventureViewModel : INotifyPropertyChanged
    {
        public GameManager GameManager;

        private String _outputText;
        private String _inputText;
        private ObservableCollection<IVerb> _validVerbs;
        private ObservableCollection<INoun> _localItems;

        private readonly DelegateCommand<string> _submitInputCommand;

        public DelegateCommand<string> SubmitInputCommand
        {
            get { return _submitInputCommand; }
        }

        public TextAdventureViewModel()
        {
            GameManager = new GameManager();

            _submitInputCommand = new DelegateCommand<string>(
            (s) => { SubmitInput(); }, //Execute
            (s) => { return true; } //CanExecute
            );

            OutputText = "";
            InputText = "";
            ValidVerbs = new ObservableCollection<IVerb>(GameManager.ValidVerbs.OrderBy(x => x.Name).ToList());
            LocalItems = new ObservableCollection<INoun>(GameManager.WorldItems.OrderBy(x => x.Name).ToList());

            OutputText += GameManager.StartGame();
        }

        public String OutputText
        {
            get { return _outputText; }
            set { _outputText = value; OnPropertyChanged("OutputText"); }
        }

        public String InputText
        {
            get { return _inputText; }
            set { _inputText = value; OnPropertyChanged("InputText"); }
        }

        public ObservableCollection<IVerb> ValidVerbs
        {
            get { return _validVerbs; }
            set { _validVerbs = value; OnPropertyChanged("ValidVerbs"); }
        }

        public ObservableCollection<INoun> LocalItems
        {
            get { return _localItems; }
            set { _localItems = value; OnPropertyChanged("LocalItems"); }
        }

        public void SubmitInput()
        {
            Console.WriteLine("Command!");
            OutputText += ">" + InputText + "\r\n";

            OutputText += GameManager.Instance.Process(InputText);

            InputText = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
