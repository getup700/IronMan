///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/20 21:51:34
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.DirectContext3D;
using Autodesk.Revit.DB.ExternalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Services
{
    internal class DirectContextService : IDirectContext3DServer
    {
        public bool CanExecute(View dBView)
        {
            throw new NotImplementedException();
        }

        public string GetApplicationId()
        {
            throw new NotImplementedException();
        }

        public Outline GetBoundingBox(View dBView)
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Guid GetServerId()
        {
            throw new NotImplementedException();
        }

        public ExternalServiceId GetServiceId()
        {
            throw new NotImplementedException();
        }

        public string GetSourceId()
        {
            throw new NotImplementedException();
        }

        public string GetVendorId()
        {
            throw new NotImplementedException();
        }

        public void RenderScene(View dBView, DisplayStyle displayStyle)
        {
            throw new NotImplementedException();
        }

        public bool UseInTransparentPass(View dBView)
        {
            throw new NotImplementedException();
        }

        public bool UsesHandles()
        {
            throw new NotImplementedException();
        }
    }
}
