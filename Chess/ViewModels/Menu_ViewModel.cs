using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Chess.Commands;
using Chess.ViewModels.Base;

namespace Chess.ViewModels
{
    internal class Menu_ViewModel : ViewModel
    {
        public NavigateCommand PlayGameCommand { get => new (PlayGameExec, new Uri("../View/Pages/PlayField.xaml", UriKind.RelativeOrAbsolute)); }
        public LambdaCommand QuitGameCommand { get => new(QuitGameExec); }

        public void PlayGameExec(object parameter)
        {
            MyCommandParameters param = (MyCommandParameters)parameter;
            NavigationService.GetNavigationService(param.Page).Navigate(param.Uri);    
        }
        public void QuitGameExec(object parameter) 
        {
            Application.Current.Shutdown();
        }

    }
}
