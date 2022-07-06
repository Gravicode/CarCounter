using CarCounter.Models;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CarCounter.UWP
{
    public class CCTVService : ICrud<CCTV>
    {
        ICCTV client;

        public CCTVService(GrpcChannel channel)
        {


            client = channel.CreateGrpcService<ICCTV>();


        }
        public async Task<bool> DeleteData(long Id)
        {
            var res = await client.DeleteData(new InputCls() { Param = new string[] { Id.ToString() } });
            return res.Result;
        }

        public async Task<List<CCTV>> FindByKeyword(string Keyword)
        {
            var data = await client.FindByKeyword(Keyword);
            return data;
        }

        public async Task<List<CCTV>> GetAllData()
        {
            var data = await client.GetAllData();
            return data;
        }



        public async Task<CCTV> GetDataById(long Id)
        {
            var res = await client.GetDataById(new InputCls() { Param = new string[] { Id.ToString() } });
            return res;
        }


        public async Task<bool> InsertData(CCTV data)
        {
            try
            {
                var res = await client.InsertData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> Update(CCTV data)
        {
            try
            {
                var res = await client.UpdateData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }



        public async Task<bool> UpdateData(CCTV data)
        {
            try
            {
                var res = await client.UpdateData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<long> GetLastId()
        {
            var res = await client.GetLastId();
            return long.Parse(res.Data);
        }
    }
}
