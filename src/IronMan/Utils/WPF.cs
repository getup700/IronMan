///************************************************************************************
///   Author:Tony Stark
///   CretaeTime:2023/3/2 0:20:10
///   Mail:2609639898@qq.com
///   Github:https://github.com/getup700
///
///   Description:
///
///************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IronMan.Revit.Utils
{
    class WPF : INotifyPropertyChanged,ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
