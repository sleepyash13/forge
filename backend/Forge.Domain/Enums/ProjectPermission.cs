using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Domain.Enums
{
    public enum ProjectPermission
    {
        ViewProject = 1,
        UpdateProject = 2,
        ManageMembers =3,
        CreateBuild =4,
        Deploy = 5,
        ManageSettings = 6,
        DeleteProject = 7
    }
}
