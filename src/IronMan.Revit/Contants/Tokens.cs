using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IronMan.Revit.Contants
{
    public static class Tokens
    {
        public static string MaterialsWindow { get => nameof(MaterialsWindow); }

        public static string MaterialDialogWindow { get => nameof(MaterialDialogWindow); }

        public static string ProgressBarMaximum { get => nameof(ProgressBarMaximum); }

        public static string ProgressBarTitle { get => nameof(ProgressBarTitle); }

        public static string FloorTypeManageView { get => nameof(FloorTypeManageView); }

        public static string CadToRevitView { get => nameof(CadToRevitView); }

        public static string CloseWindow => nameof(CloseWindow);

        public static string SmartRoomService => nameof(SmartRoomService);

        public static string FilterView => nameof(FilterView);

        public static string FilterDialogView => nameof(FilterDialogView);

        public static string FilterLabelLibraryView = nameof(FilterLabelLibraryView);

        public static string CloseFilterLabelLibraryView = "CloseFilterLabelLibraryView";

        public static string CloseFilterLabelDialogView = "CloseFilterLabelDialogView";
    }
}
