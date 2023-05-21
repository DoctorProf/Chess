using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Chess.Commands.Base;
using Chess.Constants;
using Chess.Models;
using Chess.ViewModels;

namespace Chess.Commands
{
    class FieldCommand : Command
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            object[] parameters = parameter as object[];
            Field field = parameters[0] as Field;
            Field_ViewModel fvm = field.Parent;
            fvm.ClickField(field, fvm);
        }
    }
}
