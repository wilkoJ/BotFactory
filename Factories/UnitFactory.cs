using BotFactory.Common.Tools;
using BotFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using BotFactory.Interface;

namespace BotFactory.Factories
{
    public class UnitFactory : IUnitFactory
    {
        //private static UnitFactory instance;
        public bool flaglock = true;
        public int QueueCapacity { get; set; }
        Thread factoryThread;
        public TimeSpan QueueTime { get; set; }
        public int StorageCapacity { get; set; }
        public int QueueFreeSlots { get; set; }
        public int StorageFreeSlots { get; set; }
        public List<FactoryQueueElement> Queue { get { return _queue.ToList(); } set { } }
        public List<ITestingUnit> Storage { get { return _storage.ToList(); } set { } }
        public event FactoryProgress  FactoryProgress;
        private object lockObj;
        private List<FactoryQueueElement> _queue;
        private List<ITestingUnit> _storage;
        public UnitFactory( int queueCapacity, int storageCapacity )
        {
            QueueCapacity = QueueFreeSlots = queueCapacity;
            StorageCapacity = StorageFreeSlots = storageCapacity;
            _queue = new List<FactoryQueueElement>();
            _storage = new List<ITestingUnit>();
            QueueTime = new TimeSpan(0,0,0);
            lockObj = new object();
            factoryThread = new Thread(new ThreadStart(ThreadRun));
            try
            {
                factoryThread.Start();
            }
            catch( ThreadStateException e )
            {
                Console.WriteLine( e );
            }
            catch( ThreadInterruptedException e )
            {
                Console.WriteLine( e );
            }
        }
       /* public static UnitFactory Instance
        {
            get
            {
                if( instance == null )
                {
                    Console.WriteLine("First instance of UnitFactory");
                    instance = new UnitFactory(7, 7);
                    instance.Queue = new List<Factory_queueElement>();
                    instance.Storage = new List<ITestingUnit>();
                }
                return instance;
            }
        }*/
        public bool AddWorkableUnitToQueue(Type model, string name, Coordinates parkPosition, Coordinates workPosition)
        {
            if( _queue.Count < QueueCapacity)
            {
                _queue.Add( new FactoryQueueElement( name, model, parkPosition, workPosition ) );
                QueueFreeSlots--;
                TimeSpan sp = TimeSpan.FromSeconds(( int )(( ITestingUnit )Activator.CreateInstance( _queue.First().Model )).BuildTime);
                QueueTime = QueueTime.Add( sp );
                if( _storage.Count() < StorageCapacity )
                {
                    if( Monitor.TryEnter( lockObj ) )
                    {
                        Monitor.Pulse( lockObj );
                        Monitor.Exit( lockObj );
                    }
                }
                return true;
            }
            return false;
         }
        public void createRobot()
        {
            Monitor.Enter( lockObj );
            try
            {
                if( _queue.Count() == 0 )
                    Monitor.Wait( lockObj );
            }
            catch( SynchronizationLockException e )
            {
                Console.WriteLine( e );
            }
            catch( ThreadInterruptedException e )
            {
                Console.WriteLine( e );
            }
            finally
            {
                if( _storage.Count() < StorageCapacity && _queue.Count() > 0 )
                {
                    addRobotToStorage();
                    createRobot();
                }
            }
        }

        private void addRobotToStorage()
        {
            StatusChangedEventArgs s = new StatusChangedEventArgs();
            s.NewStatus = "Robot start creation";
            FactoryProgress(this, s );
            ITestingUnit toStore = ( ITestingUnit )Activator.CreateInstance( _queue.First().Model );
            toStore.WorkingPos = _queue.First().WorkingPos;
            toStore.ParkingPos = _queue.First().ParkingPos;
            toStore.Name= _queue.First().Name;
            Thread.Sleep( ( ( int )toStore.BuildTime * 1000 ) );
            _storage.Add( toStore );
            StorageFreeSlots--;
            _queue.Remove( _queue.First() );
            QueueFreeSlots++;
            TimeSpan sp = TimeSpan.FromSeconds(( int )toStore.BuildTime);
            QueueTime = QueueTime.Subtract(sp);
            //StatusChangedEventArgs s = new StatusChangedEventArgs();( int )Storage.Last().BuildTime *
            s.NewStatus = "Robot created";
            FactoryProgress( this, s);
        }

        public object FactoryLogger(object model, string message)
        {
            Console.WriteLine(message);
            return model;
        }
        public void ThreadRun()
        {
            createRobot();
        }
    }
}
