using System.ComponentModel;

namespace AutoMotiveProject.cs.Services
{
    public class AuthStateService : INotifyPropertyChanged
    {
        private bool _isAuthenticated = false;
        private string _currentUser = "";
        
        public bool IsAuthenticated 
        { 
            get => _isAuthenticated;
            private set
            {
                _isAuthenticated = value;
                OnPropertyChanged();
                OnAuthStateChanged?.Invoke();
            }
        }
        
        public string CurrentUser 
        { 
            get => _currentUser;
            private set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        
        public event Action? OnAuthStateChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Login(string email)
        {
            IsAuthenticated = true;
            CurrentUser = email;
        }

        public void Logout()
        {
            IsAuthenticated = false;
            CurrentUser = "";
        }
        
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}