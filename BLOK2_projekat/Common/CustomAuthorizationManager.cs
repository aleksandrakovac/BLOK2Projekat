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
            //preko operationcontext kupimo principal
            //implementacija autorizacione provjere u okviru AuthoriyationManager klase
            //dobavljanje informacije o objektu IPrincipal iz bezbjedonosnog objekta
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;
            if (principal != null)
            {
                //doadto ovo jer pravimo nas customPrincipal
                CustomPrincipal cp = principal as CustomPrincipal;

                //onda provjeravamo da li su permisije date ulogama(tako nesto)
                //gledamo da li je u grupama
                if (cp != null && (cp.IsInRole(Permissions.Read.ToString()) || cp.IsInRole(Permissions.Generate.ToString()) || cp.IsInRole(Permissions.Delete.ToString()) || cp.IsInRole(Permissions.DeleteAll.ToString())))

                {
                    return true;
                }
            }

            return false;
        }
    }
}
