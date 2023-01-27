using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using IronMan.Revit.DTO;
using IronMan.Revit.Interfaces;
using IronMan.Revit.Toolkit.Mvvm.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IronMan.Revit.Toolkit.Extension;

namespace IronMan.Revit.Services
{
    public class MepInfomationService : IDataUpdater<Element, IEnumerable<DTO_KeyValue>>
    {
        private readonly IDataContext _dataContext;

        public MepInfomationService(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<DTO_KeyValue> GetInformation(Element element)
        {
            List<DTO_KeyValue> keyValueList = new List<DTO_KeyValue>();
            ParameterSet parameterMap = element.Parameters;
            //ParameterMap parameterMap = element.ParametersMap;
            IEnumerator enumerator = element.Parameters.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Parameter currentParameter = enumerator.Current as Parameter;
                if (!currentParameter.IsReadOnly)
                {
                    StorageType storageType = currentParameter.StorageType;
                    var dto = new DTO_KeyValue()
                    {
                        Name = currentParameter.Definition.Name,
                        NameId = currentParameter.Id
                    };
                    switch (storageType)
                    {
                        case StorageType.None:
                            break;
                        case StorageType.Integer:
                            dto.Value = currentParameter.AsInteger();
                            break;
                        case StorageType.Double:
                            dto.Value = currentParameter.AsDouble().ConvertToMilliMeters();
                            break;
                        case StorageType.String:
                            dto.Value = currentParameter.AsValueString();
                            break;
                        case StorageType.ElementId:
                            dto.Value = currentParameter.AsElementId();
                            dto.Reference = new Reference(element);
                            break;
                        default:
                            dto.Value = currentParameter.AsValueString();
                            break;
                    }
                    keyValueList.Add(dto);
                }
            }
            return keyValueList;
        }

        public Element Update(Element element, IEnumerable<DTO_KeyValue> dto)
        {
            _dataContext.GetDocument().NewTransaction(() =>
            {
                foreach (var item in dto)
                {
                    try
                    {
                        Parameter parameter = element.LookupParameter(item.Name);
                        if (parameter == null) continue;
                        StorageType storageType = parameter.StorageType;
                        switch (storageType)
                        {
                            case StorageType.None:
                                break;
                            case StorageType.Integer:
                                parameter.Set((int)item.Value);
                                break;
                            case StorageType.Double:
                                parameter.Set(((double)item.Value).ConvertToFeet());
                                break;
                            case StorageType.String:
                                parameter.Set((string)item.Value);
                                break;
                            case StorageType.ElementId:
                                //parameter.Set(((ElementId)item.Value));
                                ElementId referenceId = _dataContext.GetDocument().GetElement(item.Reference).LookupParameter(item.Name).AsElementId();
                                parameter.Set(referenceId);
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"{"ErrorParameter:" + item.Name + "\n" + "StorageType:" + element.LookupParameter(item.Name).StorageType + "\n" + e.StackTrace + "\n" + e.Message}");
                    }

                }
            }, "格式刷");
            return element;
        }
    }
}
