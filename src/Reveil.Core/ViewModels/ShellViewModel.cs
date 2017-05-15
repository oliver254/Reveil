using MvvmCross.Core.ViewModels;
using Reveil.Core.Infrastructure;
using Reveil.Core.Models;
using Reveil.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core.ViewModels
{
    public class ShellViewModel : MvxViewModel
    {
        #region Champs
        private readonly ISettingsManager _settingsManager;

        private MenuItem _selection;
        private TimeSpan? _duration;

        #endregion

        #region Constructeurs
        public ShellViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;

            Items = new ObservableCollection<MenuItem>
            {
                new MenuItem()
                {
                    Name = "Sprint",
                    Icon = "\uE102",
                    State = State.Sprint
                },
                new MenuItem()
                {
                    Name = "Courte pause",
                    Icon = "\uE103",
                    State = State.ShortBreak
                },
                new MenuItem()
                {
                    Name = "Longue pause",
                    Icon = "\uE71A",
                    State = State.LongBreak
                }

            };
            PropertyChanged += ViewModel_PropertyChanged;

        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient ou définit la durée
        /// </summary>
        public TimeSpan? Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                RaisePropertyChanged(nameof(Duration));
            }
        }

        /// <summary>
        /// Obtient les items du menu.
        /// </summary>
        public ObservableCollection<MenuItem> Items
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtient ou définit le item du menu sélectionné
        /// </summary>

        public MenuItem SelectedMenuItem
        {
            get
            {
                return _selection;
            }
            set
            {
                _selection = value;
                RaisePropertyChanged(nameof(SelectedMenuItem));

            }
        }
        #endregion

        #region Méthodes
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != nameof(SelectedMenuItem) || 
                SelectedMenuItem == null)
            {
                return;
            }

            Settings settings = _settingsManager.Get();

            switch (SelectedMenuItem.State)
            {
                case State.Sprint:
                    {
                        Duration = TimeSpan.FromMinutes(settings.Sprint);
                        break;
                    }
                case State.LongBreak:
                    {
                        Duration = TimeSpan.FromMinutes(settings.LongBreak);
                        break;
                    }
                case State.ShortBreak:
                    {
                        Duration = TimeSpan.FromMinutes(settings.ShortBreak);
                        break;
                    }
                default:
                    {
                        Duration = null;
                        break;
                    }

            }
        }
        #endregion

    }
}
