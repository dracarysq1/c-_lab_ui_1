using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Numerics;
using System.Dynamic;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;
using System.ComponentModel;

[assembly: InternalsVisibleToAttribute("WPF_LAB1")]
namespace DataLibrary
{
    [Serializable]
    class V3MainCollection : IEnumerable<V3Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<V3Data> list;
        public bool ChangedAfterSave { get; set; }

        public void Add(V3Data item)
        {
            list.Add(item);
            ChangedAfterSave = true;
            onCollectionChanged(NotifyCollectionChangedAction.Add);
        }
        public bool Remove(string id, DateTime dateTime)
        {
            bool ret = false;
            List<V3Data> elems_list = new List<V3Data>();
            foreach (var elem in list)
            {
                if (elem.info == id && elem.t0 == dateTime)
                    elems_list.Add(elem);
            }
            foreach (V3Data elem in elems_list)
            {
                ret |= this.list.Remove(elem);
            }
            ChangedAfterSave = true;
            onCollectionChanged(NotifyCollectionChangedAction.Remove);
            return ret;
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            onCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void onCollectionChanged(NotifyCollectionChangedAction ev)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            OnPropertyChanged("MaxDistance");
        }
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        { return list.GetEnumerator(); }

        public IEnumerator<V3Data> GetEnumerator()
        { return list.GetEnumerator(); }



        public void AddDefaults()
        {
            DateTime date1 = new DateTime(2, 3, 4); ;
            Grid1D gr_x = new Grid1D(1, 2);
            Grid1D gr_y = new Grid1D(1, 3);
            Vector2 v = new Vector2(1.0f, 2.0f);
            //list = new List<V3Data>();


            V3DataOnGrid v3DataOnGrid = new V3DataOnGrid("abc", date1, gr_x, gr_y);
            v3DataOnGrid.InitRandom(1, 9);
            this.Add(v3DataOnGrid);


            DateTime date2 = new DateTime(2020, 10, 20);
            V3DataCollection v3DataCollection = new V3DataCollection("cba", date1);
            v3DataCollection.InitRandom(3, 3, 7, 4, 6);
            this.Add(v3DataCollection);



            v3DataCollection = new V3DataCollection("asd", date2);
            v3DataCollection.InitRandom(4, 1, 5, 1, 6);
            this.Add(v3DataCollection);


        }

        //Свойство типа IEnumerable<DataItem>, которое перечсляет как элементы DataItrem 
        // все результаты измерений из V3MainCollection
        public V3MainCollection()
        {
            this.list = new List<V3Data>();
            ChangedAfterSave = false;
        }
        public void AddDefaultDataCollection()
        {

            V3DataCollection dataCollection = new V3DataCollection("defaultDataCollection", DateTime.Today);
            dataCollection.InitRandom(4, 3, 4, 0, 7);
            this.Add(dataCollection);
        }
        public void AddDefaultDataOnGrid()
        {
            Grid1D gr_x = new Grid1D(1, 2);
            Grid1D gr_y = new Grid1D(1, 3);
            V3DataOnGrid dataOnGrid = new V3DataOnGrid("defaultDataOnGrid", DateTime.Now, gr_x, gr_y);
            dataOnGrid.InitRandom(0, 100);
            this.Add(dataOnGrid);
        }
        public void AddFromFile(string filename)
        {
            V3DataOnGrid dataCollection = new V3DataOnGrid(filename);
            this.Add(dataCollection);
        }
        public IEnumerable<DataItem> ResultsAsDataItem
        {
            get
            {
                // сначала выбираем все элементы типа V3DataCollection;
                // из них выбираем все результаты измерений
                var query1 = from v3data in list
                             where v3data is V3DataCollection
                             from item in v3data as V3DataCollection
                             select item;
                // затем выбираем все элементы типа V3DataOnGrid;
                // из них выбираем все результаты измерений
                var query2 = from v3data in list
                             where v3data is V3DataOnGrid
                             from item in v3data as V3DataOnGrid
                             select item;
                // объединяем
                return query1.Union(query2);

            }
        }


        public IEnumerable<Vector2> ResultAsVector
        {
            get
            {

                var collections = from data in list
                                  where data is V3DataCollection
                                  select (V3DataCollection)data;

                var first = collections.First();
                var notfirst = collections.Skip(1);

                var v = from elem in first
                        from a in notfirst
                        from elema in a
                        where elema.vec == elem.vec
                        select elem.vec;

                return v.Distinct();
            }
        }
        //Метод, возвращающее минимальное расстояние между заданной 
        // точкой с координатами Vector2 v и точками из V3MainCollection, в которых измерено поле.

        // Метод, возвращающее минимальное расстояние между заданной 
        // точкой с координатами Vector2 v и точками из V3MainCollection, в которых измерено поле.
        // То же самое. Другая реализация





        public V3Data this[int index]
        {

            get
            {
                V3Data tmp = null;

                if (index >= 0 && index <= list.Count - 1)
                {
                    tmp = list[index];
                }
                else
                {
                    Console.WriteLine(" ");
                }
                return tmp;
            }
            set
            {
                if (index >= 0 && index <= list.Count - 1)
                {
                    list[index] = value;
                }
            }
        }
        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                if (!File.Exists(filename))
                {
                    fileStream = File.Create(filename);
                }
                else
                {
                    fileStream = File.OpenWrite(filename);
                }
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, list);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Save Error: " + ex.Message);
            //}
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                ChangedAfterSave = false;
            }
        }
        public void Load(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                list = (List<V3Data>)binaryFormatter.Deserialize(fileStream);
            }
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Load Error: " + ex.Message);
            //}
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
                ChangedAfterSave = true;
            }
        }



        public override string ToString()
        {
            string ret = "";
            foreach (V3Data data in list)
            {
                ret += (data.ToString() + '\n');
            }
            return ret;

        }
        public string ToLongString(string format)
        {
            string ret = "";
            foreach (V3Data data in list)
            {
                ret += (data.ToLongString(format) + '\n');
            }
            return ret;
        }
        public IEnumerable<V3DataCollection> getOnlyDataCollectionElems()
        {
            var query = from item in this.list
                        where item is V3DataCollection
                        select item as V3DataCollection;
            return query;
        }

        /* Метод возвращает элементы коллекции MainCollection, которые имеют тип V3DataOnGrid */
        public IEnumerable<V3DataOnGrid> getOnlyDataOnGridElems()
        {
            var query = from item in this.list
                        where item is V3DataOnGrid
                        select item as V3DataOnGrid;
            return query;
        }
        public int Count
        {
            get
            {
                return list.Count;
            }
        }


        /*public DataItem RMinDataItem(Vector2 v)
        {
            //int b = 0;
            //DataItem data = new DataItem();
            return (from item in ResultsAsDataItem
                    where Vector2.Distance(item.vec, v) == MinR(v)
                    select item).Min();


        }*/

        public float MaxDistance
        {
            get
            {

                Vector2 v0 = new Vector2(0, 0);
                float max, a;
                IEnumerable<DataItem> collection = from elem in (from v3data in list
                                                                 where v3data is V3DataCollection
                                                                 select (V3DataCollection)v3data)
                                                   from item in elem
                                                   select item;
                IEnumerable<DataItem> grid = from elem in (from v3data in list
                                                           where v3data is V3DataOnGrid
                                                           select (V3DataOnGrid)v3data)
                                             from item in elem
                                             select item;
                IEnumerable<DataItem> items = collection.Union(grid);
                if (items.Count() != 0)
                {
                    max = Vector2.Distance(items.First().vec, v0);
                    //return items.Average(n => n.Complex.Magnitude);
                    foreach (DataItem dataItem in items)
                    {
                        a = Vector2.Distance(dataItem.vec, v0);
                        if (a > max)
                        {
                            max = a;
                        }
                    }
                    return max;
                }
                else return 0;

            }

        }







    };
}
