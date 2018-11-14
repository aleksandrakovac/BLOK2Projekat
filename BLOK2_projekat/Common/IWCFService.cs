using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        List<Alarm> Read();

        [OperationContract]
        bool Generate(DateTime dateTime, string name, string message, int risk);

        [OperationContract]
        bool Delete();

        [OperationContract]
        bool DeleteAll();


    }
}
