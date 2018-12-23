using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ShoppingAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IShopping
    {

        [OperationContract]
        [WebInvoke(UriTemplate = "/Register?login={login}&email={email}&password={password}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool Register(string login,string email,string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Login?login={login}&password={password}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        Guid Login(string login, string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetUserList?userId={userId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        List<ListUser> GetUserList(Guid userId);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetProductsFromList?ListId={ListId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        List<ListProduct> GetProductsFromList(Guid ListId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateNewList?ListName={ListName}&userId={userId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool CreateNewList(string ListName,Guid userId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/AddProductToList?ProductName={ProductName}&ListId={ListId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool AddProductToList(string ProductName, Guid ListId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Sync?listId={listId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool Sync(Dictionary<Guid, bool> products, Guid listId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Share?listId={listId}&userId={userId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool Share(Guid userId, Guid listId);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetUserId?userName={userName}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        Guid GetUserId(string userName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckLoginIsExist?userName={userName}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool CheckLoginIsExist(string userName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/CheckEmailIsExist?email={email}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool CheckEmailIsExist(string email);

        [OperationContract]
        [WebInvoke(UriTemplate = "/DeleteList?listId={listId}", ResponseFormat = WebMessageFormat.Json, Method = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        bool DeleteList(Guid listId,Guid userId);



        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class ListUser
    {

        [DataMember]
        public Guid ListId { get; set; }
        [DataMember]
        public string ListName { get; set; }

    }
    [DataContract]
    public class ListProduct
    {
        [DataMember]
        public Guid ProductId { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public bool? IsBought { get; set; }
        [DataMember]
        public Guid ListId { get; set; }
    }
}
