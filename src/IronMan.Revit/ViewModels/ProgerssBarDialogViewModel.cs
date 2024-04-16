using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronMan.Revit.ViewModels
{
    public class ProgerssBarDialogViewModel:ViewModelBase
    {
        private int _maximum;
        private int _value;
        private string _title;

        public ProgerssBarDialogViewModel()
        {
            MessengerInstance.Register<int>(this, Contants.Tokens.ProgressBarMaximum, (max) =>
            {
                Maximum = max;
            });
            MessengerInstance.Register<string>(this, Contants.Tokens.ProgressBarTitle, (title) =>
            {
                System.Windows.Forms.Application.DoEvents();//把相关UI信息更新，展示ui
                Value++;
                Title = $"{Value}/{Maximum}_{title}";
            });
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }


        public int Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }


        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; RaisePropertyChanged(); }
        }

    }
}
