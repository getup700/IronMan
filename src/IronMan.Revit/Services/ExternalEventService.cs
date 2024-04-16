using Autodesk.Revit.UI;
using IronMan.Revit.IServices;
using System;
namespace IronMan.Revit.Services
{
    public class ExternalEventService : IExternalEventService
    {
        private readonly ExternalEvent _externalEvent;

        private Action<UIApplication> _action;

        public ExternalEventService()
        {
            _externalEvent = ExternalEvent.Create(this);
        }

        public void Execute(UIApplication app)
        {
            if (_action != null)
            {
                _action.Invoke(app);
            }
        }

        public string GetName() => "ExternalEventService";

        public void Raise(Action<UIApplication> action)
        {
            _action = action;
            _externalEvent.Raise();
        }
    }
}
