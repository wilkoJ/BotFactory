using BotFactory.Models;
using BotFactory.Common;
using BotFactory.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using System.Threading.Tasks;
using BotFactory.Factories;

namespace BotFactory.Tools
{
    public class FactoryDataContext : INotifyPropertyChanged
    {


       private List<Type> _models = new List<Type>() { typeof(Hal) };

        

        private List<Type> subclasses = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                         from type in assembly.GetTypes()
                                         where type.IsSubclassOf(typeof(WorkingUnit))
                                         select type).ToList();






        public List<Type> Models
        {
            get { return subclasses; }
        }

        public String Name { get; set; }

        private IUnitFactory _builder;

        public IUnitFactory Builder
        {
            get
            {
                return _builder;
            }
            set
            {
                SetField(ref _builder, value, "Builder");
                ForceUpdate();
            }

        }

        public int QueueCapacity
        {
            get
            {
                return _builder.QueueCapacity;
            }
            set
            {
                OnPropertyChanged("QueueCapacity");
            }
        }

        public int StorageCapacity
        {
            get
            {
                return _builder.StorageCapacity;
            }
            set
            {
                OnPropertyChanged("StorageCapacity");
            }
        }

        public TimeSpan QueueTime
        {
            get
            {
                return _builder.QueueTime;
            }
            set
            {
                OnPropertyChanged("QueueTime");
            }
        }

        public int QueueFreeSlots
        {
            get
            {
                return _builder.QueueFreeSlots;
            }
            set
            {
                OnPropertyChanged("QueueFreeSlots");
            }
        }

        public int StorageFreeSlots
        {
            get
            {
                return _builder.StorageFreeSlots;
            }
            set
            {
                OnPropertyChanged("StorageFreeSlots");
            }
        }

        public List<FactoryQueueElement> Queue
        {
            get
            {
                return _builder.Queue;
            }
            set
            {
                OnPropertyChanged("Queue");
            }
        }

        public List<ITestingUnit> Storage
        {
            get
            {
                return _builder.Storage;
            }
            set
            {
                OnPropertyChanged("Storage");
            }
        }

        public void ForceUpdate()
        {
            if (Builder != null)
            {
                Queue = null;
                Storage = null;
                QueueFreeSlots = 0;
                StorageFreeSlots = 0;
                QueueTime = TimeSpan.FromSeconds(0);
            }
        }

        #region INotifyPropertyChanged Interface Methods

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName.ToString());
            return true;
        }

        #endregion
    }
}
