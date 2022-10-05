using Autodesk.Revit.DB.Events;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit
{
    public class AppEvent : IEventManager
    {
        private readonly IUIProvider _uiProvider;

        public AppEvent(IUIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public void Subscribe()
        {
            _uiProvider.GetApplication().DocumentOpened += AppEvent_DocumentOpened;
            _uiProvider.GetApplication().DocumentClosed += AppEvent_DocumentClosed;
            _uiProvider.GetApplication().DocumentCreated += AppEvent_DocumentCreated;
        }

        public void Unsubscribe()
        {
            _uiProvider.GetApplication().DocumentOpened -= AppEvent_DocumentOpened;
            _uiProvider.GetApplication().DocumentClosed -= AppEvent_DocumentClosed;
            _uiProvider.GetApplication().DocumentCreated -= AppEvent_DocumentCreated;
        }

        private void AppEvent_DocumentCreated(object sender, DocumentCreatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AppEvent_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AppEvent_DocumentOpened(object sender, DocumentOpenedEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
