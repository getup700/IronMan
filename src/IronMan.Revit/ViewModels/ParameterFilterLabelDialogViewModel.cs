using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using IronMan.Revit.Entity;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.ViewModels
{
    public class ParameterFilterLabelDialogViewModel : ViewModelBase
    {
        private readonly IParameterFilterLabelService _service;
        private ObservableCollection<string> _labels;

        public ParameterFilterLabelDialogViewModel(IParameterFilterLabelService service)
        {
            _service = service;
            _labels= new ObservableCollection<string>();
        }

        public List<ParameterFilterProxy> Filters { get; set; }

        public IList<string> LabelLibrary { get; set; } = new List<string>();

        public ObservableCollection<string> Labels
        {
            get => _labels;
            set => Set(ref _labels, value);
        }

        public RelayCommand<IList> AddLabelsCommand => new RelayCommand<IList>(labels =>
        {
            if (labels.Count == 0) return;
            foreach (var label in labels.Cast<string>())
            {
                if (_labels != null)
                {
                    if (!_labels.Contains(label))
                    {
                        _labels.Add(label);
                    }
                }
                else
                {
                    _labels.Add(label);
                }
            }
        });

        public RelayCommand<IList> DeleteLabelsCommand => new RelayCommand<IList>(labels =>
        {
            if (labels.Count == 0) return;
            var deleteLabels = new List<string>();
            foreach (var label in labels.Cast<string>())
            {
                deleteLabels.Add(label);
            }
            if (deleteLabels.Count == 0) return;
            foreach (var label in deleteLabels)
            {
                _labels.Remove(label);
            }
        });

        public RelayCommand SubmitCommand => new RelayCommand(() =>
        {
            if (Filters.Count == 0) return;
            var result = new List<ParameterFilterProxy>();
            foreach (var f in Filters)
            {
                var oldLabels =_service.GetElement(f).Labels;
                var newLabels = _labels.Union(oldLabels).ToList();
                var filter = _service.ReplaceFilterLabels(f, new DTO.DTO_Filter()
                {
                    Labels = newLabels
                });
                result.Add(filter);
            }
            Filters.Clear();
            Filters = result;
            MessengerInstance.Send<bool>(true, Contants.Tokens.CloseFilterLabelDialogView);
        });

        public void InitialFilter(ParameterFilterProxy filter)
        {
            Filters = new List<ParameterFilterProxy>() { filter };
            _labels = new ObservableCollection<string>(_service.GetElement(filter).Labels);
        }
    }
}
