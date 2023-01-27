using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronMan.Revit.Toolkit.Extension
{
    public static class DefinitionExtension
    {
        public static DefinitionFile GetDefinitionFile(this ControlledApplication application, string fileName = "IronManSharedParaFile")
        {
            string appFile = application.SharedParametersFilename;
            if (appFile.Length == 0 || !File.Exists(appFile))
            {
                string filePath = Directory.GetCurrentDirectory() + fileName + ".txt";
                StreamWriter sw = new StreamWriter(filePath);
                sw.Close();
                application.SharedParametersFilename = filePath;
            }
            DefinitionFile definitionFile = application.OpenSharedParameterFile();
            return definitionFile;
        }

        public static DefinitionGroup GetDefinitionGroup(this DefinitionFile definitionFile, string groupName)
        {
            if (definitionFile is null)
            {
                throw new ArgumentNullException(nameof(definitionFile));
            }
            if (groupName is null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            var definitionGroup = definitionFile.Groups.ToList();
            DefinitionGroup specifyGroup = definitionGroup.Where(x => x.Name == groupName)?.FirstOrDefault();
            if (specifyGroup == null)
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            return specifyGroup;
        }

        public static Definition GetDefinition(this DefinitionGroup definitionGroup, string definitionName, ParameterType parameterType)
        {
            if (definitionGroup is null)
            {
                throw new ArgumentNullException(nameof(definitionGroup));
            }
            if (definitionName is null)
            {
                throw new ArgumentNullException(nameof(definitionName));
            }
            Definition definition = definitionGroup.Definitions.get_Item(definitionName);
            if (definition == null)
            {
                ExternalDefinitionCreationOptions creationOptions = new ExternalDefinitionCreationOptions(definitionName, parameterType);
                creationOptions.Visible = true;
                creationOptions.UserModifiable = true;
                definition = definitionGroup.Definitions.Create(creationOptions);
            }
            return definition;
        }
    }
}
