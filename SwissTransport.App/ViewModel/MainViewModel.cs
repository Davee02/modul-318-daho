﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using SwissTransport.App.Model;

namespace SwissTransport.App.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        // Holds all the viewmodels for the individual tabs in the tabcontrol
        public ObservableCollection<object> TabChildren { private get; set; } = new ObservableCollection<object>();

        public ICommand SendResultsAsEmail { get; set; }

        public MainViewModel()
        {
            TabChildren.Add(new ConnectionFinderViewModel());
            TabChildren.Add(new StationBoardViewModel());
            TabChildren.Add(new NearStationsViewModel());

            SendResultsAsEmail = new RelayCommand(x => SendEmailWithSearchResults(x));
        }

        private void SendEmailWithSearchResults(object selectedTabItemIndex)
        {
            if(!(selectedTabItemIndex is int index))
                return;

            var viewModel = (ITransportResultViewModel)TabChildren[index];
            try
            {
                viewModel.SendResultsAsMail();
            }
            catch (Exception e)
            {
                MessageBox.Show("Beim Senden des E-Mails ist der folgende Fehler aufgetreten:\n\n" + e,
                    "Es ist ein Fehler aufgetreten", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
