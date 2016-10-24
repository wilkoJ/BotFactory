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
        private object lockObj2;

        public UnitFactory( int queueCapacity, int storageCapacity )
        {
            //Data for the Collections
            QueueCapacity = QueueFreeSlots = queueCapacity;
            StorageCapacity = StorageFreeSlots = storageCapacity;
            //Queue and Storage Collections
            _queue = new List<FactoryQueueElement>();
            _storage = new List<ITestingUnit>();
            QueueTime = new TimeSpan(0,0,0);
            //Thread variables
            lockObj = new object();
            lockObj2 = new object();
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
            if( Monitor.TryEnter( lockObj2 ) )
            {
                if( _queue.Count < QueueCapacity )
                {
                    _queue.Add( new FactoryQueueElement( name, model, parkPosition, workPosition ) );
                    QueueFreeSlots = QueueCapacity - _queue.Count();

                    TimeSpan sp = TimeSpan.FromSeconds(( int )(( ITestingUnit )Activator.CreateInstance( _queue.First().Model )).BuildTime);
                    QueueTime = QueueTime.Add( sp );
                    Monitor.Exit(lockObj2);

                    if( Monitor.TryEnter( lockObj ) )
                    {
                        if( _storage.Count() < StorageCapacity )
                        {
                            Monitor.Pulse( lockObj );
                            Monitor.Exit( lockObj );
                        }
                    }
                    return true;
                }
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

            FactoryQueueElement first = _queue.First();
            ITestingUnit toStore = ( ITestingUnit )Activator.CreateInstance( first.Model );
            toStore.WorkingPos = first.WorkingPos;
            toStore.ParkingPos = first.ParkingPos;
            toStore.Name= first.Name;

            Thread.Sleep(((int)toStore.BuildTime * 1000));
            _storage.Add( toStore );
            StorageFreeSlots = StorageCapacity - _storage.Count();
            _queue.Remove(first);
            QueueFreeSlots = QueueCapacity - _queue.Count();

            TimeSpan sp = TimeSpan.FromSeconds(toStore.BuildTime);
            QueueTime = QueueTime.Subtract(sp);
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
        ~UnitFactory() {
            factoryThread.Abort();
        }
    }
}
