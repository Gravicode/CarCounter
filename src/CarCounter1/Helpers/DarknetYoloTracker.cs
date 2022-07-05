using DarknetYolo.Models;
using System.Data;

namespace CarCounter1.Helpers
{
    public class DarknetYoloTracker
    {
        const int DistanceLimit = 100;
        const int TimeLimit = 5; //in seconds
        public List<TrackedObject> TrackedList;
        DataTable table = new DataTable("counter");
        public DarknetYoloTracker()
        {
            TrackedList = new List<TrackedObject>();
            table.Columns.Add("No");
            table.Columns.Add("Waktu");
            table.Columns.Add("Jenis");
            table.AcceptChanges();
        }

        public DataTable GetLogTable()
        {
            return table;
        }
        public void SaveToLog()
        {
            string FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/log_{DateTime.Now.ToString("yyyy_MM_dd")}.csv";
            Logger.SaveAsCsv(table, FileName);
        }

        public void Process(IList<YoloPrediction> Targets, Rectangle SelectArea)
        {
            HashSet<string> Existing = new HashSet<string>();
            bool IsAdded = false;
            foreach (var newItem in Targets)
            {
                IsAdded = true;
                var pos = new PointF(newItem.Rectangle.Left + newItem.Rectangle.Width / 2, newItem.Rectangle.Top + newItem.Rectangle.Height / 2);

                var selTarget = TrackedList.Where(x => Distance(pos, x.Location) < DistanceLimit && !Existing.Contains(x.Id) && newItem.Label == x.Label).FirstOrDefault();
                if (selTarget != null)
                {
                    //tambah ke existing and update
                    Existing.Add(selTarget.Id);
                    selTarget.Update(pos);
                    IsAdded = false;

                }
                else
                {
                    var newObj = new TrackedObject() { Location = pos, Label = newItem.Label };
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
        }

        double TimeGapInSecond(DateTime dt1, DateTime dt2)
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
