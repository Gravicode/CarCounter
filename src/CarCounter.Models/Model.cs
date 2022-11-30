using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace CarCounter.Models
{
    #region helpers
    public class CCTVImage
    {
        public string CctvName { get; set; }
        public byte[] ImageBytes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion
    #region auth
    [DataContract]
    public class AuthenticationModel
    {
        [DataMember(Order = 1)]
        public string ApiKey { get; set; }
    }
    [DataContract]
    public class AuthenticationUserModel
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string Password { get; set; }
    }
    [DataContract]
    public class AuthenticatedUserModel
    {
        [DataMember(Order = 1)]
        public string Username { get; set; }
        [DataMember(Order = 2)]
        public string AccessToken { get; set; }
        [DataMember(Order = 3)]
        public string TokenType { get; set; }
        [DataMember(Order = 4)]
        public DateTime? ExpiredDate { get; set; }
    }
    #endregion
    #region GRPC
    [ServiceContract]
    public interface IAuth
    {
        [OperationContract]
        Task<AuthenticatedUserModel> AuthenticateWithUsername(AuthenticationUserModel data, CallContext context = default);

        [OperationContract]
        Task<AuthenticatedUserModel> AuthenticateWithApiKey(AuthenticationModel data, CallContext context = default);
    }
    [ServiceContract]
    public interface ICCTV : ICrudGrpc<CCTV>
    {

    }
    [ServiceContract]
    public interface IGateway : ICrudGrpc<Gateway>
    {

    }
    [ServiceContract]
    public interface IDataCounter : ICrudGrpc<DataCounter>
    {

    }

    [ServiceContract]
    public interface IUserProfile : ICrudGrpc<UserProfile>
    {
        [OperationContract]
        Task<UserProfile> GetItemByEmail(InputCls input, CallContext context = default);

        [OperationContract]
        Task<UserProfile> GetItemByPhone(InputCls input, CallContext context = default);

        [OperationContract]
        Task<OutputCls> IsUserExists(InputCls input, CallContext context = default);

        [OperationContract]
        Task<OutputCls> GetUserRole(InputCls input, CallContext context = default);
    }
    #endregion

    #region Common
    public interface ICrud<T> where T : class
    {
        Task<bool> InsertData(T data);
        Task<bool> UpdateData(T data);
        Task<List<T>> GetAllData();
        Task<T> GetDataById(long Id);
        Task<bool> DeleteData(long Id);
        Task<long> GetLastId();
        Task<List<T>> FindByKeyword(string Keyword);
    }
    [ServiceContract]
    public interface ICrudGrpc<T> where T : class
    {
        [OperationContract]
        Task<OutputCls> InsertData(T data, CallContext context = default);
        [OperationContract]
        Task<OutputCls> UpdateData(T data, CallContext context = default);
        [OperationContract]
        Task<List<T>> GetAllData(CallContext context = default);
        [OperationContract]
        Task<T> GetDataById(InputCls Id, CallContext context = default);
        [OperationContract]
        Task<OutputCls> DeleteData(InputCls Id, CallContext context = default);
        [OperationContract]
        Task<OutputCls> GetLastId(CallContext context = default);
        [OperationContract]
        Task<List<T>> FindByKeyword(string Keyword, CallContext context = default);
    }
    [DataContract]
    public class InputCls
    {
        [DataMember(Order = 1)]
        public string[] Param { get; set; }
        [DataMember(Order = 2)]
        public Type[] ParamType { get; set; }
    }
    [DataContract]
    public class OutputCls
    {
        [DataMember(Order = 1)]
        public bool Result { get; set; }
        [DataMember(Order = 2)]
        public string Message { get; set; }
        [DataMember(Order = 3)]
        public string Data { get; set; }
    }
    #endregion
    #region database

    [DataContract]
    public class Gateway
    {
        [DataMember(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [DataMember(Order = 2)]
        public string? Nama { get; set; }
        [DataMember(Order = 3)]
        public DateTime? TanggalPasang { get; set; }
        [DataMember(Order = 4)]
        public string? Lokasi { get; set; }

        [DataMember(Order = 5)]
        public string? Keterangan { get; set; }
    }

    [DataContract]
    public class CCTV
    {
        [DataMember(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [DataMember(Order = 2)]
        public string? Nama { get; set; }
        [DataMember(Order = 3)]
        public DateTime? TanggalPasang { get; set; }
        [DataMember(Order = 4)]
        public string? Lokasi { get; set; }
        [DataMember(Order = 5)]
        public string? Gateway { get; set; }
        [DataMember(Order = 6)]
        public string? Merek { get; set; }
    }

    [DataContract]
    public class DataCounter
    {
        [DataMember(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [DataMember(Order = 2)]
        public DateTime Tanggal { get; set; }
        [DataMember(Order = 3)]
        public string? Jenis { get; set; }
        [DataMember(Order = 4)]
        public string? Lokasi { get; set; }
        [DataMember(Order = 5)]
        public string? Gateway { get; set; }
        [DataMember(Order = 6)]
        public string? Merek { get; set; }
        [NotMapped]
        public int Index { get; set; }
    }
    [DataContract]
    public class UserProfile
    {
        [DataMember(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [DataMember(Order = 2)]
        public string Username { get; set; }
        [DataMember(Order = 3)]
        public string Password { get; set; }
        [DataMember(Order = 4)]
        public string FullName { get; set; }
        [DataMember(Order = 5)]
        public string? Phone { get; set; }
        [DataMember(Order = 6)]
        public string? Email { get; set; }
        [DataMember(Order = 7)]
        public string? Alamat { get; set; }
        [DataMember(Order = 8)]
        public string? KTP { get; set; }
        [DataMember(Order = 9)]
        public string? PicUrl { get; set; }
        [DataMember(Order = 10)]
        public bool Aktif { get; set; } = true;

        [DataMember(Order = 11)]
        public Roles Role { set; get; } = Roles.User;

    }

    public enum Roles { Admin, User, Operator }
    #endregion
}