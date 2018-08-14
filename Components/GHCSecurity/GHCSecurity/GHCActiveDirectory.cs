using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

namespace Ghc.Utility.Security
{    
    public class GHCActiveDirectory
    {
        public enum PrincipalType { Machine, User };            

        /// <summary>
        /// This is primarily used to get connection strings, which are stored as user descriptions
        /// in the DabaseConnectionStrings OU.
        /// </summary>
        /// <param name="userName">string</param>
        /// <returns>string</returns>
        public static string GetUserDescription(string userName)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain);

            UserPrincipal details = UserPrincipal.FindByIdentity(context, userName);

            return details.Description;
        }
        
        /// <summary>
        /// Public method for verifying if a given principal (user or machine) is a member of a given group.
        /// </summary>
        /// <param name="principalName">string</param>
        /// <param name="type">PrincipalType</param>
        /// <param name="groupName">string</param>
        /// <returns>bool</returns>
        public static bool IsMemberOf(string principalName, PrincipalType type, string groupName)
        {
            bool returnValue = false;
            PrincipalContext context = new PrincipalContext(ContextType.Domain);

            try
            {
                switch (type)
                {
                    case PrincipalType.Machine:
                        using (var comp = ComputerPrincipal.FindByIdentity(context, principalName))
                        {
                            returnValue = IsMemberOf(groupName, comp);
                        }                         
                        break;
                    case PrincipalType.User:
                        using (var user = UserPrincipal.FindByIdentity(context, principalName))
                        {
                            returnValue = IsMemberOf(groupName, user);
                        }
                        break;
                }

                return returnValue;
            }
            catch
            {
                throw;
            }
            finally
            {
                context = null;
            }            
        }

        /// <summary>
        /// A non-public (internal) method used to verify if a principal is a member of a group.
        /// </summary>
        /// <param name="groupName">string</param>
        /// <param name="principal">AuthenticablePrincipal</param>
        /// <returns>bool</returns>
        internal static bool IsMemberOf(string groupName, AuthenticablePrincipal principal)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
            GroupPrincipal groupPrincipal = GroupPrincipal.FindByIdentity(principalContext, groupName);

            if(groupPrincipal != null)
            {
                if (principal.IsMemberOf(groupPrincipal))
                {
                    return true;
                }                
            }
            return false;            
        }        
    }
}
