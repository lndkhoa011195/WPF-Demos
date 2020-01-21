using CheckboxSelectorForDatagrid.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CheckboxSelectorForDatagrid.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<Employee> _employees;
        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                if (Equals(value, _employees)) return;
                _employees = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            // Just some demo data
            Employees = new List<Employee>
        {
            new Employee(){ Name = "Genji", IsChecked = false},
            new Employee(){ Name = "Hana", IsChecked = false},
            new Employee(){ Name = "Reinhart", IsChecked = false},
            new Employee(){ Name = "Winston", IsChecked = false},
            new Employee(){ Name = "Hanzo", IsChecked = false},
            new Employee(){ Name = "McCree", IsChecked = false},
        };

            // Make sure to listen to changes. 
            // If you add/remove items, don't forgat to add/remove the event handlers too
            foreach (Employee entry in Employees)
            {
                entry.PropertyChanged += EntryOnPropertyChanged;
            }
        }

        private void EntryOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            // Only re-check if the IsChecked property changed
            if (args.PropertyName == nameof(Employee.IsChecked))
                RecheckAllSelected();
        }

        private void AllSelectedChanged()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            if (_allSelectedChanging) return;

            try
            {
                _allSelectedChanging = true;

                // this can of course be simplified
                if (AllSelected == true)
                {
                    foreach (Employee kommune in Employees)
                        kommune.IsChecked = true;
                }
                else if (AllSelected == false)
                {
                    foreach (Employee kommune in Employees)
                        kommune.IsChecked = false;
                }
            }
            finally
            {
                _allSelectedChanging = false;
            }
        }

        private void RecheckAllSelected()
        {
            // Has this change been caused by some other change?
            // return so we don't mess things up
            if (_allSelectedChanging) return;

            try
            {
                _allSelectedChanging = true;

                if (Employees.All(e => e.IsChecked))
                    AllSelected = true;
                else if (Employees.All(e => !e.IsChecked))
                    AllSelected = false;
                else
                    AllSelected = null;
            }
            finally
            {
                _allSelectedChanging = false;
            }
        }

        public bool? AllSelected
        {
            get => _allSelected;
            set
            {
                if (value == _allSelected) return;
                _allSelected = value;

                // Set all other CheckBoxes
                AllSelectedChanged();
                OnPropertyChanged();
            }
        }

        private bool _allSelectedChanging;

        private bool? _allSelected;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
