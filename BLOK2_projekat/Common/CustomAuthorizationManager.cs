using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;
            if (principal != null)
            {
                CustomPrincipal cp = principal as CustomPrincipal;

                if (cp != null && (cp.IsInRole(Permissions.Read.ToString()) || cp.IsInRole(Permissions.Generate.ToString()) || cp.IsInRole(Permissions.Delete.ToString()) || cp.IsInRole(Permissions.DeleteAll.ToString())))

                {
                    return true;
                }
            }

            return false;
        }
    }
}
