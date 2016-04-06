using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using WordsTest.Model;

namespace WordTest.Web.API
{
    [ServiceContract]
    public interface ISyncService
    {
        [OperationContract]
        SyncData GetDataUsingDataContract(SyncData data);
     
    }


    [DataContract]
    public class SyncData
    {
        [DataMember]
        List<Test> Tests { get; set; }

        [DataMember]
        List<TestItem> TestItems { get; set; }
    }
}
