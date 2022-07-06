using Microsoft.AI.Skills.Vision.ObjectDetector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCounter.UWP.Helpers
{
    
    public class TrackedObject
    {
        static Random rnd = new Random(Environment.TickCount);
        public Color Col { get; set; }
        public string Id { get; set; }
        public PointF Location { get; set; }
        public string Label { get; set; }
        public List<PointF> Trails { get; set; }
        public bool HasBeenCounted { get; set; } = false;

        public DateTime LastUpdate { get; set; }
        public DateTime Created { get; set; }
        
        public TrackedObject()
        {
            Col = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));
            Id = Guid.NewGuid().ToString();
            Trails = new List<PointF>();
            LastUpdate= DateTime.Now;   
        }

        public void Update(PointF location)
        {
            Trails.Add(this.Location);
            this.Location = location;
            this.LastUpdate = DateTime.Now;
        }
    }
    public class Tracker
    {
        const int DistanceLimit = 200;
        const double AgeLimit = 30;
        const int TimeLimit = 5; //in seconds
        public List<TrackedObject> TrackedList;
        DataTable table = new DataTable("counter");
        public Tracker()
        {
            TrackedList = new List<TrackedObject>();
            table.Columns.Add("No");
            table.Columns.Add("Waktu");
            table.Columns.Add("Jenis");
            table.AcceptChanges();
        }

        public DataTable GetStatTable()
        {
            DataTable stat = new DataTable("stat");
            stat.Columns.Add("Jenis");
            stat.Columns.Add("Jumlah");
            stat.AcceptChanges();
            var dataJenis = (from product in table.AsEnumerable()
                           select product.Field<string>("Jenis")).Distinct().ToArray();
            foreach(var jenis in dataJenis)
            {
                var jumlah = table.AsEnumerable().Where(x => x.Field<string>("Jenis") == jenis).Count();
                var newRow = stat.NewRow();
                newRow["Jenis"] = jenis;
                newRow["Jumlah"] = jumlah;
                stat.Rows.Add(newRow);
            }
            return stat;
        }

        public DataTable GetLogTable()
        {
            return table;
        }

        public void ClearLogTable()
        {
            table.Rows.Clear();
        }

        public void SaveToLog()
        {
            
            string FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/log_{DateTime.Now.ToString("yyyy_MM_dd")}.csv";
            Logger.SaveAsCsv(table, FileName);
        }
        /*
        public void Process(IReadOnlyList<ObjectDetectorResult> Targets, Rectangle SelectArea)
        {
            HashSet<string> Existing = new HashSet<string>();
            bool IsAdded = false;
            foreach (var newItem in Targets)
            {
                IsAdded = true;
                var pos = new PointF((float)(newItem.Rect.X + newItem.Rect.Width / 2), (float)(newItem.Rect.Y + newItem.Rect.Height / 2));

                var selTarget = TrackedList.Where(x => Distance(pos, x.Location) < DistanceLimit && !Existing.Contains(x.Id) && newItem.Kind.ToString() == x.Label).FirstOrDefault();
                if (selTarget != null)
                {
                    //tambah ke existing and update
                    Existing.Add(selTarget.Id);
                    selTarget.Update(pos);
                    IsAdded = false;

                }
                else
                {
                    var newObj = new TrackedObject() { Location = pos, Label = newItem.Kind.ToString(),Created=DateTime.Now };
                    TrackedList.Add(newObj);
                    Existing.Add(newObj.Id);
                }

            }
            var now = DateTime.Now;
            var removes = TrackedList.Where(x => TimeGapInSecond(now, x.LastUpdate) > TimeLimit).ToList();
            foreach (var item in removes)
            {
                TrackedList.Remove(item);
            }
            //count
            foreach (var item in TrackedList)
            {
                if (SelectArea.Contains(new Point((int)item.Location.X, (int)item.Location.Y)) && !item.HasBeenCounted)
                {
                    var newRow = table.NewRow();
                    newRow["No"] = table.Rows.Count + 1;
                    newRow["Waktu"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    newRow["Jenis"] = item.Label;
                    table.Rows.Add(newRow);

                    item.HasBeenCounted = true;
                }
            }
        }*/

        public void Process(IReadOnlyList<Result> Targets,Rectangle SelectArea)
        {
            HashSet<string> Existing = new HashSet<string>();
            bool IsAdded = false;
            foreach (var newItem in Targets)
            {
                IsAdded = true;
                var pos = new PointF(newItem.BoundingBox[0] + Math.Abs((newItem.BoundingBox[0]- newItem.BoundingBox[2])/2), newItem.BoundingBox[1] + Math.Abs((newItem.BoundingBox[3] - newItem.BoundingBox[1]) / 2));
               
                var selTarget = TrackedList.Where(x=> Distance(pos, x.Location)<DistanceLimit && !Existing.Contains(x.Id) && newItem.Label == x.Label).FirstOrDefault();
                if (selTarget != null)
                {
                    //tambah ke existing and update
                    Existing.Add(selTarget.Id);
                    selTarget.Update(pos);
                    IsAdded = false;

                }else
                {
                    var newObj = new TrackedObject() { Location = pos, Label = newItem.Label,Created=DateTime.Now };
                    TrackedList.Add(newObj);
                    Existing.Add(newObj.Id);
                }
                
            }
            var now = DateTime.Now;
            var removes = TrackedList.Where(x=>TimeGapInSecond(now,x.LastUpdate)>TimeLimit).ToList();
            foreach(var item in removes)
            {
                TrackedList.Remove(item);
            }
            //count
            foreach(var item in TrackedList)
            {
                if(SelectArea.Contains(new Point((int) item.Location.X, (int)item.Location.Y)) && !item.HasBeenCounted)
                {
                    var newRow = table.NewRow();
                    newRow["No"] = table.Rows.Count + 1;
                    newRow["Waktu"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    newRow["Jenis"] = item.Label;
                    table.Rows.Add(newRow);
                    
                    item.HasBeenCounted = true;
                }
            }
            //delete
            var deleted = TrackedList.Where(x => x.HasBeenCounted).ToList();
            foreach(var item in deleted)
            {
                TrackedList.Remove(item);
            }
            //delete aged
            deleted = TrackedList.Where(x => TimeGapInSecond(x.LastUpdate,x.Created) > AgeLimit ).ToList();
            foreach (var item in deleted)
            {
                TrackedList.Remove(item);
            }

        }

        double TimeGapInSecond(DateTime dt1,DateTime dt2)
        {
            var ts = dt1 - dt2;
            return ts.TotalSeconds;
        }

        double Distance(PointF p1, PointF p2)
        {
            var distance = Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
            return distance;
        }
    }
}
