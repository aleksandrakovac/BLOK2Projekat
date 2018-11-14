using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Claims;
using System.Security.Principal;

namespace Common
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private string id;
        private object locker = new object();

        public CustomAuthorizationPolicy()
        {
            this.id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return this.id; }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        //prvo evaluete pa onda checkaccesscore pa onda ulazi u metodu
        public bool Evaluate(EvaluationContext evaluationContext, ref object state) 
        {
            object list;

            if (!evaluationContext.Properties.TryGetValue("Identities", out list))
            {
                return false;
            }

            IList<IIdentity> identities = list as IList<IIdentity>;
            if (list == null || identities.Count <= 0)
            {
                return false;
            }

            evaluationContext.Properties["Principal"] = GetPrincipal(identities[0]);

            return true;
        }

        //vraca pri
        protected virtual IPrincipal GetPrincipal(IIdentity identity)
        {
            lock (locker) //zakljucavamo da ne bi moglo da isotvremeno pristupa
            {
                IPrincipal principal = null;
                WindowsIdentity windowsIdentity = identity as WindowsIdentity; 

                if (windowsIdentity != null)
                {
                    principal = new CustomPrincipal(windowsIdentity); //
                }

                return principal;
            }
        }


    }
}
