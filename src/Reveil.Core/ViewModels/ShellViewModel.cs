using MvvmCross.Core.ViewModels;
using Reveil.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reveil.Core.ViewModels
{
    public class ShellViewModel : MvxViewModel
    {
        #region Champs
        private MenuItem _selection;
        #endregion

        #region Constructeurs
        public ShellViewModel()
        {
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
        }
        #endregion

        #region Propriétés
        /// <summary>
        /// Obtient les items du menu.
        /// </summary>
        public ObservableCollection<MenuItem> Items
        {
            get;
            private set;
        }


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
        #endregion

    }
}
