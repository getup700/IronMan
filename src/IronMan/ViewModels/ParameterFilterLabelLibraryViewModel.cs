using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.DTO;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace IronMan.Revit.ViewModels
{
    public class ParameterFilterLabelLibraryViewModel : ViewModelBase
    {

        private ObservableCollection<string> _labels;
        private string _inputLabel;

        public string InputLabel
        {
            get => _inputLabel;
            set => Set(ref _inputLabel, value);
        }

        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set { Set(ref _labels, value); }
        }

        public RelayCommand NewLabelCommand => new RelayCommand(() =>
        {
            var key = InputLabel.Trim();
            if (_labels.Contains(key) || key == null) return;
            _labels.Add(key);
            InputLabel = string.Empty;
        });

        public RelayCommand<IList> DeleteLabelCommand => new RelayCommand<IList>(keys =>
        {
            if (keys.Count == 0) return;
            var deleteLabels = new List<string>();
            foreach (var label in keys.Cast<string>())
            {
                if (_labels.Contains(label))
                {
                    deleteLabels.Add(label);
                }
            }
            foreach (var item in deleteLabels)
            {
                _labels.Remove(item);
            }
        });

        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            MessengerInstance.Send<IList<string>>(Labels, Contants.Tokens.CloseFilterLabelLibraryView);
        });

        public void Initial(IList<string> labels)
        {
            _labels = new ObservableCollection<string>(labels);
        }
    }
}
