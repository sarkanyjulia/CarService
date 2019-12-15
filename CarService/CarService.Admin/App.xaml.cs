using System.Windows;
using Microsoft.Win32;
using CarService.Admin.Model;
using CarService.Admin.Persistence;
using CarService.Admin.View;
using CarService.Admin.ViewModel;
using System;

namespace CarService.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ICarServiceModel _model;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private WorksheetEditorWindow _editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new CarServiceModel(new CarServicePersistence("https://localhost:44300/")); // megadjuk a szolgáltatás címét
            ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            //_mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _mainViewModel.Logout += new EventHandler(ViewModel_Logout);
            _mainViewModel.EditingStarted += new EventHandler(MainViewModel_EditingStarted);
            _mainViewModel.EditingFinished += new EventHandler(MainViewModel_EditingFinished);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();

            _loginView.Close();
        }

        private void MainViewModel_EditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
        }

        private void MainViewModel_EditingStarted(object sender, EventArgs e)
        {
            _editorView = new WorksheetEditorWindow(); // külön szerkesztő dialógus az épületekre
            _editorView.DataContext = _mainViewModel;
            _editorView.Closed += new EventHandler(MainViewModel_EditorWindowClosed);
            _editorView.ShowDialog();
        }

        

        private async void ViewModel_Logout(object sender, EventArgs e)
        {
            if (_model.IsUserLoggedIn)
            {
                await _model.LogoutAsync();
            }
            ShowLoginWindow();
            _mainView.Close();
        }

 

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen.", "Szuper Szaki Autószervíz", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn) // amennyiben be vagyunk jelentkezve, kijelentkezünk
            {
                await _model.LogoutAsync();
            }
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Szuper Szaki Autószervíz", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_EditorWindowClosed(object sender, EventArgs e)
        {
            _mainViewModel.WorksheetUnderEdit = null;
        }

        

    }
}
