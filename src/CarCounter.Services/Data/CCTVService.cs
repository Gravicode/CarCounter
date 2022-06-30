using CarCounter.Models;
using Microsoft.EntityFrameworkCore;
using CarCounter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCounter.Data
{
    public class CCTVService : ICrud<CCTV>
    {
        CarCounterDB db;

        public CCTVService()
        {
            if (db == null) db = new CarCounterDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.CCTVs.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.CCTVs.Remove(selData);
            db.SaveChanges();
            return true;
        }
        
        public List<CCTV> FindByKeyword(string Keyword)
        {
            var data = from x in db.CCTVs
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<CCTV> GetAllData()
        {
            return db.CCTVs.ToList();
        }

        public CCTV GetDataById(object Id)
        {
            return db.CCTVs.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(CCTV data)
        {
            try
            {
                db.CCTVs.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(CCTV data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if (sel != null)
                {
                    sel.Nama = data.Nama;
                    sel.Keterangan = data.Keterangan;
                    sel.Tanggal = data.Tanggal;
                    sel.DocumentUrl = data.DocumentUrl;
                    sel.StreamUrl = data.StreamUrl;
                    return true;

                }*/
                return true;
            }
            catch
            {

            }
            return false;
        }

        public long GetLastId()
        {
            return db.CCTVs.Max(x => x.Id);
        }
    }

}