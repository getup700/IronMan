///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/12 0:22:17
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using IronMan.Revit.IServices;
using IronMan.Revit.Toolkit.Extension;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.IOC;
using IronMan.Revit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.ViewModels
{
    internal class ProperitesViewModel : ViewModelBase
    {
        private readonly IDataContext _dataContext;
        private readonly IExternalEventService _externalEventService;
        private string _oldDateTime;
        private TextNote _textNote;
        private string _time;
        private string _propertiesInfo;

        public ProperitesViewModel(IDataContext dataContext,IExternalEventService externalEventService)
        {
            _time = "dddd";
            _oldDateTime = DateTime.Now.ToString();
            _dataContext = dataContext;
            _externalEventService= externalEventService; _dataContext.GetUIApplication().Idling += IDling_ShowSelectInfo;
            _dataContext.GetDocument().NewTransaction(() =>
            {
                ElementId elementId = _dataContext.GetDocument().GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
                _textNote = TextNote.Create(_dataContext.GetDocument(), _dataContext.GetDocument().ActiveView.Id, XYZ.Zero, DateTime.Now.ToString(), elementId);
            });
            _dataContext.GetUIApplication().Idling += IDling_Update;
            var view = SingletonIOC.Current.Container.GetInstance<PropertiesView>();
            _externalEventService.Raise(uiapp =>
            {
                view.Closing += (sender, args) =>
                {
                    _dataContext.GetUIApplication().Idling -= IDling_ShowSelectInfo;
                    _dataContext.GetUIApplication().Idling -= IDling_Update;
                };
            });
        }

        public string Time
        {
            get { return _time; }
            set { _time = value; RaisePropertyChanged(); }
        }

        public string PropertiesInfo
        {
            get { return _propertiesInfo; }
            set { _propertiesInfo = value; RaisePropertyChanged(); }
        }

        private void IDling_ShowSelectInfo(object sender, IdlingEventArgs e)
        {
            UIApplication uIApplication = (UIApplication)sender;
            var elementIds = uIApplication.ActiveUIDocument.Selection.GetElementIds();
            PropertiesInfo = elementIds.First().ToString();
        }

        private void IDling_Update(object sender, IdlingEventArgs eventArgs)
        {
            UIApplication uIApplication = sender as UIApplication;
            Document document = uIApplication.ActiveUIDocument.Document;
            if (DateTime.Now.ToString() != _oldDateTime)
            {
                using (Transaction transaction = new Transaction(document, "更新时间"))
                {
                    transaction.Start();
                    _textNote.Text = DateTime.Now.ToString();
                    transaction.Commit();
                    eventArgs.SetRaiseWithoutDelay();
                }
                _oldDateTime = DateTime.Now.ToString();
                Time = _oldDateTime;

            }
        }

        public RelayCommand UpdateTimeCommand => new RelayCommand(() =>
        {
            Time = DateTime.Now.ToString();
        });

        
    }
}
